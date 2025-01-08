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
using static FrooxEngine.AppEnder;
using static OfficialAssets.Graphics;

namespace FasterBadges
{
    public class Patch : ResoniteMod
    {
        public override String Name => "FasterBadges";
        public override String Author => "zahndy";
        public override String Link => "https://github.com/zahndy/FasterBadges";
        public override String Version => "1.1.0";

        public static ModConfiguration Config;

        const string HEADER_TEXT_COLOR = "green";

        public delegate void ConfigurationChangedHandler(ConfigurationChangedEvent configurationChangedEvent);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);

        [AutoRegisterConfigKey] 
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY = new ModConfigurationKey<dummy>("dummy", "---------------------------------------------------------------------------------------------------------------------------------");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY2 = new ModConfigurationKey<dummy>("DUMMY_SEP_1_1", $"<color={HEADER_TEXT_COLOR}>[Pride]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY3 = new ModConfigurationKey<dummy>("DUMMY_SEP_1_1", $"<color={HEADER_TEXT_COLOR}>[Identity]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY4 = new ModConfigurationKey<dummy>("DUMMY_SEP_1_1", $"<color={HEADER_TEXT_COLOR}>[Kinks]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY5 = new ModConfigurationKey<dummy>("DUMMY_SEP_1_1", $"<color={HEADER_TEXT_COLOR}>[Role]</color>", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Bisexual = new ModConfigurationKey<bool>("Bisexual", "Bisexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Gay = new ModConfigurationKey<bool>("Gay", "Gay", () => false);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> CustomBadges = new ModConfigurationKey<string>("CustomBadges", "List of custom badges(csv)", () => "");   
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BadgesVarInternal = new ModConfigurationKey<string>("BadgesVarInternal", "List of custom badges(csv)", () => "", internalAccessOnly: true);

        List<String> BadgesList;

        private static HashSet<AvatarManager> Avatars;
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.OnThisConfigurationChanged += OnThisConfigurationChanged;
            Config.Save(true);
            BadgesList = Config.GetValue(BadgesVarInternal).Split(',').ToList();
            
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            Avatars = new HashSet<AvatarManager>();
            harmony.PatchAll();
        }

        private void OnThisConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
            ModConfigurationKey<bool> changedvar = null;
            Uri url = null;
            bool skip = false;
            Msg(" --- configurationChangedEvent.Key.Name: "+ configurationChangedEvent.Key.Name + " --- ");
            switch (configurationChangedEvent.Key.Name) //map existing ModConfigurationKeys
            {
                case "CustomBadges":
                    skip = true;
                    break;
                case "Bisexual":
                    changedvar = Bisexual;
                    url = new Uri("resdb:///11c7f8a885c0ae089a1e60e0cf4e9d14da7c2f8f497b7c0865091675c4ce2c6d.webp");
                    break;
                case "Gay":
                    changedvar = Gay;
                    url = new Uri("resdb:///114e25ca8b823f670a3ba7c5bea1b1f100f28153c0637535ee36dd0a80bfd79e.webp");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(configurationChangedEvent.Key.Name);
                                   
            }
           
            foreach (AvatarManager av in Avatars)
            {
                User user = av.Slot.ActiveUser;
                if (user.IsLocalUser)
                {
                    av.Slot.RunSynchronously(delegate
                    {
                        if (!skip)
                        {
                            if (url != null)
                            {
                                bool KeyEnabled = Config.GetValue(changedvar); //end up with the changed ModConfigurationKey
                                String BadgeName = "Extra Custom Badge-" + url.ToString().Substring(url.ToString().Length - 10, 5);
                                HashSet<string> hashSet = Pool.BorrowHashSet<string>();
                                foreach (Slot child in av.BadgeTemplates.Children)
                                {
                                    hashSet.Add(child.Name);
                                }

                                if (!hashSet.Contains(BadgeName))
                                {
                                    if (KeyEnabled)
                                    {
                                        BlendMode? blendMode = new BlendMode?();
                                        colorX? tint = new colorX?();
                                        int? maxSize = new int?(128);
                                        av.AddIconBadge(url, BadgeName, blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                        if (!BadgesList.Contains(BadgeName))
                                        {
                                            BadgesList.Add(BadgeName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!KeyEnabled)
                                    {
                                        av.BadgeTemplates.FindChild(BadgeName).Destroy();
                                        if (BadgesList.Contains(BadgeName))
                                        {
                                            BadgesList.Remove(BadgeName);
                                        }
                                    }
                                }
                                Config.Set(BadgesVarInternal, String.Join(",", BadgesList));
                            }
                        }
                        else
                        {
                            List<String> CustomBadgesList = Config.GetValue(CustomBadges).Split(',').ToList();

                            foreach (String customBadge in CustomBadgesList)
                            {
                                av.BadgeTemplates.FindChild("Extra Badge-", true, true, 1);
                            }
                            
                            BlendMode? blendMode = new BlendMode?();
                            colorX? tint = new colorX?();
                            int? maxSize = new int?(128);
                            foreach (String customBadge in CustomBadgesList)
                            {
                                Uri burl = new Uri(customBadge);
                                av.AddIconBadge(burl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                            }

                        }
                        Config.Save();
                        av.UpdateBadges();
                    });
                }

            }
            
            
            // avatarManager.UpdateBadges();
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
                            String[] Badges = Config.GetValue(CustomBadges).Split(',');
                            Badges.AddRangeToArray(Config.GetValue(CustomBadges).Split(','));
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
                                            avatarManager.AddIconBadge(url, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
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
