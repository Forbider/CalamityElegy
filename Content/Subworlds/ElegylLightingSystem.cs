using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;

namespace CalamityElegy.Content.Subworlds
{
    public class ElegyLightingSystem : ModSystem
    {
        // 这个方法每帧都会运行，用来修改光照
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            // 检查玩家当前是否在你的子世界里
            if (SubworldSystem.IsActive<SeaOfElegySub>())
            {
                // 将背景光调暗，并染成深蓝色
                // 这里的数值范围是 0 到 255
                backgroundColor = new Color(10, 20, 60); 
                
                // 将物块表面的自然光调暗
                tileColor = new Color(100, 100, 150);
            }
        }
    }
}