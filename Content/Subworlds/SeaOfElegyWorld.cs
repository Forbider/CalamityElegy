using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using SubworldLibrary;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
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
            int mainIslandX = Main.maxTilesX - 150; // 靠近右侧边缘
            int mainIslandY = 300;                  // 固定的高度
            
            // 调用函数：生成一个巨大的平缓岛屿（宽度 120，高度 40）
            GenerateExquisiteIsland(mainIslandX, mainIslandY, 200, 80);

            // 4. 设置玩家出生点
            // 我们可以让玩家出生在最左边的一个小岛上，或者直接出生在中间
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

            // 2. 在周围随机生成 2-4 个微型卫星岛
            int satellites = WorldGen.genRand.Next(20, 30);
            for (int i = 0; i < satellites; i++)
            {
                int offsetX = WorldGen.genRand.Next(-70, 71);
                int offsetY = WorldGen.genRand.Next(-40, 41);
                
                // 卫星岛要小很多
                GenerateExquisiteIsland(centerX + offsetX, centerY + offsetY, 15, 10);
            }
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

                // 5. 装饰：在边缘添加“抖动”碎片
                if (WorldGen.genRand.NextBool(5)) {
                    WorldGen.PlaceTile(x, topY + thickness + WorldGen.genRand.Next(1, 3), TileID.Cloud, true);
                }
            }
        }
        public void GenerateMeteorCrater(int centerX, int centerY, int radius)
        {
            // 1. 挖掘核心空洞
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));

                    // 核心区域：完全挖空
                    if (dist < radius * 0.8f)
                    {
                        Main.tile[x, y].ClearEverything();
                        Main.tile[x, y].LiquidAmount = 0; // 冲击力排开了水
                    }
                    // 边缘区域：将原有方块替换为陨石矿或焦土
                    else if (dist < radius)
                    {
                        if (Main.tile[x, y].HasTile) 
                        {
                            Main.tile[x, y].TileType = TileID.Meteorite; // 或者是你自定义的“挽歌碎片”
                            WorldGen.SquareTileFrame(x, y);
                        }
                    }
                }
            }

            // 2. 生成撞击喷溅的“碎片岛”
            for (int i = 0; i < 5; i++)
            {
                int scatterX = centerX + WorldGen.genRand.Next(-radius * 2, radius * 2);
                int scatterY = centerY + WorldGen.genRand.Next(-radius * 2, radius * 2);
                
                // 在坑周围生成微小的陨铁簇
                WorldGen.TileRunner(scatterX, scatterY, WorldGen.genRand.Next(3, 6), 3, TileID.Meteorite, false, 0f, 0f, false, true);
            }
        }
    }
}