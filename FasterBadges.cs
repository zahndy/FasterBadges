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
        public override String Link => "https://github.com/zahndy/FasterBadges";
        public override String Version => "1.1.0";

        public static ModConfiguration Config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BadgesVar = new ModConfigurationKey<string>("BadgesVar", "List of badges(csv)", () => "");

        private static HashSet<AvatarManager> Avatars;
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            Avatars = new HashSet<AvatarManager>();
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(AvatarBadgeManager))]
        class AvatarBadgeManager_OnAttach_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnAttach")]
            static void Prefix(AvatarBadgeManager __instance) 
            {
                if (Config.GetValue(ENABLED)) 
                {
                    User user = __instance.Slot.ActiveUser;
                    if (user.UserName != null)
                    {
                        if (user.IsLocalUser)
                        {
                            String[] Badges = Config.GetValue(BadgesVar).Split(',');
                            if (Badges.Count() > 0)
                            {
                                UserRoot userRoot = user.Root;
                                AvatarManager avatarManager = userRoot.Slot.GetComponent<AvatarManager>();
                                if (avatarManager != null)
                                {  
                                    if (!Avatars.Contains(avatarManager))
                                    {
                                        avatarManager.Disposing += (field) => { Avatars.Remove(avatarManager); };
                                        Avatars.Add(avatarManager);
                                        Msg(" --- Adding Custom Badges --- ");
                                        BlendMode? blendMode = new BlendMode?();
                                        colorX? tint = new colorX?();
                                        int? maxSize = new int?(128);
                                        foreach (String customBadge in Badges)
                                        {
                                            Uri url = new Uri(customBadge);
                                            avatarManager.AddIconBadge(url, "Extra Custom Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                        }
                                    }
                                    else 
                                    { 
                                        Msg(" --- Badges already added to this avatarManager --- "); 
                                    }
                                    // avatarManager.UpdateBadges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
