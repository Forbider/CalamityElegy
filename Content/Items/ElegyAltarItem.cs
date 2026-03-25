using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityElegy.Content.Items
{
    public class ElegyAltarItem : ModItem
    {
        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            
            // 关联到上面的方块
            Item.createTile = ModContent.TileType<Tiles.ElegyAltarTile>();
        }

        // 暂时给一个简单的合成表，方便你进游戏测试
        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1); // 用一根土合成，方便测试
            recipe.Register();
        }
    }
}