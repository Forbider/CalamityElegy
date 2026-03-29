using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using SubworldLibrary;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using CalamityElegy.Content.Tiles; 
using CalamityElegy.Content.Items; 
namespace CalamityElegy.Content.Subworlds
{
    public class SeaOfElegySub : Subworld
    {
        
        public override bool NormalUpdates => false;
        public override int Width => 1000;
        public override int Height => 1500;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => false;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SeaOfElegyGenPass("Loading", 1f) 
        };
    }

    public class SeaOfElegyGenPass : GenPass
    {
        public SeaOfElegyGenPass(string name, float loadWeight) : base(name, loadWeight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // --- 修复点：移除了多余的嵌套 protected override void ... ---
            
            progress.Message = "正在编织往昔的残响...";

            // 1. 填充背景（清空方块并灌满水）
            // 注意：使用 Main.maxTilesX 而不是固定数值
            for (int x = 0; x < Main.maxTilesX; x++) {
                for (int y = Main.maxTilesY; y > 300; y--) {
                    Tile tile = Main.tile[x, y];
                    tile.ClearEverything(); // 清空所有方块
                    tile.LiquidAmount = 255; // 灌满水
                    tile.LiquidType = LiquidID.Water; 
                }
            }
            int islandCount = WorldGen.genRand.Next(5, 9);
            for (int i = 0; i < islandCount; i++)
            {
                // X 坐标在左侧 2/3 区域随机
                int rx = WorldGen.genRand.Next(100, (int)(Main.maxTilesX * 0.7f));
                // Y 坐标在地表层 (300 - 500) 随机
                int ry = WorldGen.genRand.Next(350, 450);
                
                
            }
            bool generatIng = false;
            for (int i = 0; i < 4; i++) {
                if (i==2){
                    generatIng=true;
                }  
                int rx = 150 + i * 150; // 均匀分布群岛中心
                GenerateArchipelago(rx, WorldGen.genRand.Next(280, 300), generatIng);
                generatIng=false;
            }
            // 2. 在中心生成平台
            int mainIslandX = Main.maxTilesX - 150;
            int mainIslandY = 300;                  
            
            // 生成一个平缓岛屿（宽度 200，高度 80）
            GenerateExquisiteIsland(mainIslandX, mainIslandY, 200, 80);
            Main.spawnTileX = 150; 
            Main.spawnTileY = 250;
            int island_x = Main.spawnTileX,cnt=4;

            for(int i=0;i<=6;i++){
                for(int j=10-i;j>=0;j--){
                    Tile tile = Main.tile[island_x-j, 251+i];
                    tile.HasTile = true;
                    tile.TileType = TileID.Grass;
                    tile.LiquidAmount = 0;
                }
            }
            for(int i=0;i<=6;i++){
                for(int j=10-i;j>0;j--){
                    Tile tile = Main.tile[island_x+j, 251+i];
                    tile.HasTile = true;
                    tile.TileType = TileID.Grass;
                    tile.LiquidAmount = 0;
                }
            }
        }
        public void GenerateArchipelago(int centerX, int centerY, bool generatIng)
        {
            // 1. 先生成一个核心中型岛屿
            GenerateExquisiteIsland(centerX, centerY, 80, 50);
            if(generatIng){
                GenerateMeteorCrater(centerX, centerY, 30);
            }
            int offsetX;
            int offsetY;
            // 2. 在周围随机生成微型卫星岛
            int satellites = WorldGen.genRand.Next(20, 41);
            for (int i = 0; i < satellites; i++)
            {
                offsetX = WorldGen.genRand.Next(-70, 71);
                offsetY = WorldGen.genRand.Next(-10, 10);
                //当卫星岛坐标与主岛重合时重新生成
                while(offsetX>=-40&&offsetX<=40){
                    offsetX = WorldGen.genRand.Next(-70, 71);
                }
                // 卫星岛要小很多
                GenerateExquisiteIsland(centerX + offsetX, centerY + offsetY, 15, 10);
            }
            // 横向范围可以稍微宽一点
            offsetX = WorldGen.genRand.Next(-300, 301);
            // 关键：Y 偏移必须是显著的负数，让它“飘”起来
            // 比如在主岛上方 150 到 300 像素之间
            offsetY = WorldGen.genRand.Next(-300, -150); 
            int finalX = centerX + offsetX;
            int finalY = centerY + offsetY;
            // 安全检查
            while (finalX < 50 || finalX > Main.maxTilesX - 50 || finalY < 50) {
                offsetX = WorldGen.genRand.Next(-300, 301);
                offsetY = WorldGen.genRand.Next(-300, -150);
            }
            // 空岛的特征：通常更细长或者更破碎
            int sW = WorldGen.genRand.Next(8, 15);
            int sH = WorldGen.genRand.Next(4, 8); 
            GenerateExquisiteIsland(finalX, finalY, sW, sH);
        }
        public static void GenerateExquisiteIsland(int centerX, int centerY, int width, int maxHeight)
        {
            // 1. 基础参数
            float seed = WorldGen.genRand.NextFloat() * 100f; 

            for (int x = centerX - width / 2; x < centerX + width / 2; x++)
            {
                // 计算相对坐标 (0.0 到 1.0)
                float progress = (float)(x - (centerX - width / 2)) / width;
                
                // 使用正弦半圆罩子：让两头变薄，中间变厚
                float envelope = MathF.Sin(progress * MathF.PI);

                // 2. 多频噪声叠加 (决定顶部高度)
                float noise = MathF.Sin(x * 0.1f + seed) * 5f;       // 中频率起伏
                noise += MathF.Sin(x * 0.4f + seed * 1.2f) * 2f;    // 高频率锯齿
                
                int topY = centerY - (int)(noise * envelope);
                
                // 3. 底部侵蚀算法 (决定厚度)
                // 底部不规则度更高，模拟被从大地“撕裂”的感觉
                float bottomNoise = MathF.Sin(x * 0.15f + seed * 2f) * 8f;
                int thickness = (int)((maxHeight * envelope) + bottomNoise);

                // 4. 开始垂直分层填充
                for (int y = topY; y < topY + thickness; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    Tile tile = Main.tile[x, y];
                    tile.HasTile = true;
                    tile.LiquidAmount = 0; // 岛屿内部排干水

                    // --- 高像素感的分层逻辑 ---
                    int depth = y - topY; // 距离顶部的深度

                    if (depth == 0) {
                        // 顶层：发光草块或亮色瓷砖
                        tile.TileType = TileID.Grass; 
                    }
                    else if (depth < 4) {
                        // 浅层：泥土
                        tile.TileType = TileID.Dirt;
                    }
                    else if (depth < thickness - 3) {
                        // 深层：岩石或自定义的宫殿建材
                        tile.TileType = TileID.Stone;
                    }
                    else {
                        // 底层：变黑的岩石或带有“钟乳石”感的滴落方块
                        tile.TileType = TileID.Ash; 
                    }
                }            
            }
        }
        public void GenerateStalactite(int x, int y, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int currentY = y + i;
                if (!WorldGen.InWorld(x, currentY)) break;

                // 越往下越细
                int width = (length - i) / 3; 
                if (width < 1) width = 1;
                for (int j = -width; j <= width; j++)
                {
                    Main.tile[x + j, currentY].TileType = TileID.Stone;
                    WorldGen.SquareTileFrame(x + j, currentY);
                }
                if (WorldGen.genRand.NextBool(3)) x += WorldGen.genRand.Next(-1, 2);
            }
        }
        public void GenerateMeteorCrater(int centerX, int centerY, int radius)
        {
            // 先获取你的矿石 ID，避免在循环中重复调用
            ushort shardType = (ushort)ModContent.TileType<DrownedShardsTile>();

            // 1. 挖掘核心空洞与替换边缘
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    // 使用平方距离判断（比 Vector2.Distance 快，省去开方运算）
                    float dx = x - centerX;
                    float dy = y - centerY;
                    float distSq = dx * dx + dy * dy;
                    float radSq = radius * radius;

                    Tile tile = Main.tile[x, y]; // 获取当前方块引用

                    // 核心区域：完全挖空 (80% 半径内)
                    if (distSq < radSq * 0.64f) // 0.8 * 0.8 = 0.64
                    {
                        tile.ClearEverything();
                        tile.LiquidAmount = 0; 
                    }
                    // 边缘区域：强行替换为沉溺碎晶 (80% 到 100% 半径)
                    else if (distSq < radSq)
                    {
                        // 不要用 PlaceTile，直接暴力修改数据
                        tile.HasTile = true;
                        tile.TileType = shardType;
                        tile.Slope = 0; // 清除斜坡
                        tile.IsHalfBlock = false; // 清除半砖
                        
                        // 让方块重新计算帧（决定它显示哪一部分贴图）
                        WorldGen.SquareTileFrame(x, y, true);
                    }
                }
            }
        }
    }
}
