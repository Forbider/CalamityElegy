using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.IO;
using SubworldLibrary;

namespace CalamityElegy.Content.Subworlds
{
    public class SeaOfElegySub : Subworld
    {
        public override int Width => 800;
        public override int Height => 600;
        public override bool ShouldSave => false;
        public override bool NoPlayerSaving => false;

        // 1. 这里直接引用我们下面定义的自定义生成类
        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SeaOfElegyGenPass("Loading", 1f) 
        };
    }

    // 2. 在这里定义你的专属生成器，不再需要 PassLegacy 这个中间商
    public class SeaOfElegyGenPass : GenPass
    {
        // 构造函数：告诉游戏这个步骤的名字和权重
        public SeaOfElegyGenPass(string name, float loadWeight) : base(name, loadWeight) { }

        // 核心生成逻辑在这里写
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "正在唤醒深海的记忆..."; // 加载时显示的文字

            // 填充世界逻辑
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    // 示例：把整个世界填满你的祭坛方块（仅用于测试，之后可以改成空气或水）
                    Tile tile = Main.tile[x, y];
                    tile.HasTile = true;
                    tile.TileType = (ushort)ModContent.TileType<Tiles.ElegyAltarTile>();
                }
            }
        }
    }
}