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

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(AvatarBadgeManager))]
        class AvatarBadgeManager_OnAttach_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnAttach")]
            static void Prefix(AvatarBadgeManager __instance) 
            {
                User user = __instance.Slot.ActiveUser;
                UserRoot userRoot = user.Root;

                if (Config.GetValue(ENABLED)) 
                {
                    if (user.UserName != null)
                    {
                        if (user.IsLocalUser)
                        {
                            String[] Badges = Config.GetValue(BadgesVar).Split(',');
                            if (Badges.Count() > 0)
                            {
                                AvatarManager avatarManager = userRoot.Slot.GetComponent<AvatarManager>();
                                if (avatarManager != null)
                                {                       
                                    int getFlag = user.LocalUserRoot.Slot.GetChildrenWithTag("CustomBadgesFlag").Count();
                                    if (getFlag < 1)
                                    {
                                        Slot FlagSlot = user.LocalUserRoot.Slot.AddSlot("CustomBadgesFlag");
                                        FlagSlot.Tag = "CustomBadgesFlag";
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
