using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// 关键点：引用方块所在的命名空间
using CalamityElegy.Content.Tiles; 

namespace CalamityElegy.Content.Items
{
    public class DrownedShards : ModItem
    {
        public override void SetStaticDefaults() {
            // 这里可以留空，或者在 .hjson 里写名称
        }

        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            // 关键点：确保类名拼写完全一致
            Item.createTile = ModContent.TileType<DrownedShardsTile>(); 
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Blue;
        }
    }
}