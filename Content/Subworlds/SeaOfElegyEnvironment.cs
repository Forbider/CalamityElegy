using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SubworldLibrary;
using Terraria.ID;

namespace CalamityElegy.Content.Subworlds
{
    // 1. 处理环境氛围（SceneEffect）
    public class SeaOfElegyScene : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => SubworldSystem.IsActive<SeaOfElegySub>();
        
        public override SceneEffectPriority Priority => SceneEffectPriority.BossLow;

        // 调整背景权重
        public override float GetWeight(Player player) => 1f;
    }

    // 2. 处理光照颜色与水风格
    public class ElegyLightingSystem : ModSystem
    {
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (SubworldSystem.IsActive<SeaOfElegySub>())
            {
                // 背景：幽暗深蓝
                backgroundColor = new Color(10, 20, 60); 

                // 物块：冷色调
                tileColor = new Color(90, 100, 140);
            }
        }

        // 修正：使用 PostUpdateWorld 或 PreUpdateWorld 来强制锁定水风格
        public override void PreUpdateWorld()
        {
            if (SubworldSystem.IsActive<SeaOfElegySub>())
            {
                // 12 是原版深渊/地下水风格 ID
                Main.waterStyle = 12; 
            }
        }
    }
}