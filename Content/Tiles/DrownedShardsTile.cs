using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// 关键点：引用物品所在的命名空间
using CalamityElegy.Content.Items; 

namespace CalamityElegy.Content.Tiles
{
    public class DrownedShardsTile : ModTile
    {
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

            AddMapEntry(new Color(12, 45, 94));

            DustType = DustID.Cloud;
            HitSound = SoundID.Tink;
            MineResist = 3f;
            MinPick = 100;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            r = 0f;
            g = 0.6f;
            b = 0.8f;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<DrownedShards>());
        }
    }
}