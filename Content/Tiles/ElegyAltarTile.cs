using Microsoft.Xna.Framework; // 删掉那些多余的下划线和字母
using Microsoft.Xna.Framework.Graphics; // 如果需要画图功能
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SubworldLibrary;
using CalamityElegy.Content.Subworlds;
namespace CalamityElegy.Content.Tiles
{
    public class ElegyAltarTile : ModTile
    {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4); // 借用 5x4 的模板
            TileObjectData.newTile.Width = 5;  // 宽度 5 格
            TileObjectData.newTile.Height = 5; // 高度 5 格
            
            // 关键：告诉游戏每一行切多少像素高。5个16加上间隙，通常写成：
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16 };
            
            // 关键：告诉游戏每一列切多少像素宽。
            TileObjectData.newTile.CoordinateWidth = 16;
            
            // 关键：告诉游戏格与格之间的填充距离（通常是 2 像素）
            TileObjectData.newTile.CoordinatePadding = 2; 

            TileObjectData.newTile.Origin = new Point16(2, 4); // 设置脚底中心点
            TileObjectData.addTile(Type);
        }

        // 控制发光颜色 (R, G, B)
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = 0.2f; // 淡淡的青光
            g = 0.6f;
            b = 0.9f;
        }

        // 核心：设置为不可破坏
        public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            return false; // 永远返回 false，玩家用什么镐子都挖不动
        }

        // 核心：防爆
        public override bool CanExplode(int i, int j) {
            return false; // 炸药也炸不掉
        }
        public override bool RightClick(int i, int j) {
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                // 触发进入维度的动画和逻辑
                SubworldSystem.Enter<SeaOfElegySub>();
            }
            return true;
        }
    }
}