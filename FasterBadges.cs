using HarmonyLib;
using ResoniteModLoader;
using FrooxEngine;
using Elements.Core;
using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine.CommonAvatar;
using System.Runtime.CompilerServices;

namespace FasterBadges
{
    public class Patch : ResoniteMod
    {
        public override String Name => "FasterBadges";
        public override String Author => "zahndy";
        public override String Link => "-";
        public override String Version => "0.0.1";

        public static ModConfiguration Config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BadgesVar = new ModConfigurationKey<string>("BadgesVar", "List of badges(csv)", () => "");

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(SpawnHelper))]
        class SpawnHelper_OnUserSpawn_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("Spawn")]
            static void Postfix(UserRoot userRoot) 
            {
                if (Config.GetValue(ENABLED)) 
                {
                    if (userRoot.ActiveUser.UserName != null)
                    {
                        if (userRoot.ActiveUser.IsLocalUser)
                        {
                            String[] Badges = Config.GetValue(BadgesVar).Split(',');
                            if (Badges.Length > 0)
                            {
                                AvatarManager avatarManager = userRoot.Slot.GetComponent<AvatarManager>();
                                TextureFilterMode textureFilterMode = TextureFilterMode.Bilinear;
                                BlendMode? blendMode = new BlendMode?();
                                colorX? tint = new colorX?();
                                int filterMode = (int)textureFilterMode;
                                int? maxSize = new int?(128);
                                foreach (String customBadge in Badges)
                                {
                                    Uri url = new Uri(customBadge);
                                    avatarManager.AddIconBadge(url, "Extra Custom Badge-"+ customBadge.Substring(customBadge.Length-10,5), blendMode, tint, (TextureFilterMode)filterMode, maxSize);
                                }
                                avatarManager.UpdateBadges();
                            }
                        }
                    }
                }
            }
        }
    }
}
