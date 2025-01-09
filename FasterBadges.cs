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
        //https://wiki.resonite.com/Resonite_Bot#Assignable_Badges
        public delegate void ConfigurationChangedHandler(ConfigurationChangedEvent configurationChangedEvent);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY = new ModConfigurationKey<dummy>("dummy", "For Accessibility badges please see https://wiki.resonite.com/Resonite_Bot#Assignable_Badges");
        [AutoRegisterConfigKey] 
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY1 = new ModConfigurationKey<dummy>("dummyLine", "---------------------------------------------------------------------------------------------------------------------------------");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY2 = new ModConfigurationKey<dummy>("DUMMY_1", $"<color={HEADER_TEXT_COLOR}>[Pride]</color>", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Abrosexual = new ModConfigurationKey<bool>("Abrosexual", "Abrosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Aegosexual = new ModConfigurationKey<bool>("Aegosexual", "Aegosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Aliquasexual = new ModConfigurationKey<bool>("Aliquasexual", "Aliquasexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Androsexual = new ModConfigurationKey<bool>("Androsexual", "Androsexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Asexual = new ModConfigurationKey<bool>("Asexual", "Asexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Autosexual = new ModConfigurationKey<bool>("Autosexual", "Autosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Bisexual = new ModConfigurationKey<bool>("Bisexual", "Bisexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Ceterosexual = new ModConfigurationKey<bool>("Ceterosexual", "Ceterosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Demisexual = new ModConfigurationKey<bool>("Demisexual", "Demisexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Fraysexual = new ModConfigurationKey<bool>("Fraysexual", "Fraysexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> GayGilbert = new ModConfigurationKey<bool>("GayGilbert", "Gay (Gilbert)", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> GayMaleFull = new ModConfigurationKey<bool>("GayMaleFull", "Gay Male (Full)", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> GayMaleSimple = new ModConfigurationKey<bool>("GayMaleSimple", "Gay Male (Simple)", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Gay = new ModConfigurationKey<bool>("Gay", "Gay", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Graysexual = new ModConfigurationKey<bool>("Graysexual", "Graysexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Gynesexual = new ModConfigurationKey<bool>("Gynesexual", "Gynesexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Iculasexual = new ModConfigurationKey<bool>("Iculasexual", "Iculasexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Kalossexual = new ModConfigurationKey<bool>("Kalossexual", "Kalossexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Lesbian = new ModConfigurationKey<bool>("Lesbian", "Lesbian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Omnisexual = new ModConfigurationKey<bool>("Omnisexual", "Omnisexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Pansexual = new ModConfigurationKey<bool>("Pansexual", "Pansexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Polysexual = new ModConfigurationKey<bool>("Polysexual", "Polysexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Proligosexual = new ModConfigurationKey<bool>("Proligosexual", "Proligosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Queer = new ModConfigurationKey<bool>("Queer", "Queer", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> TEST_DUMMY3 = new ModConfigurationKey<dummy>("DUMMY_2", $"<color={HEADER_TEXT_COLOR}>[Identity]</color>", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Agender = new ModConfigurationKey<bool>("Agender", "Agender", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Aromantic = new ModConfigurationKey<bool>("Aromantic", "Aromantic", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Demiromantic = new ModConfigurationKey<bool>("Demiromantic", "Demiromantic", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Genderfluid = new ModConfigurationKey<bool>("Genderfluid", "Genderfluid", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Genderqueer = new ModConfigurationKey<bool>("Genderqueer", "Genderqueer", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Intersex = new ModConfigurationKey<bool>("Intersex", "Intersex", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Naturist = new ModConfigurationKey<bool>("Naturist", "Naturist", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Nonbinary = new ModConfigurationKey<bool>("Nonbinary", "Nonbinary", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Polyamorous = new ModConfigurationKey<bool>("Polyamorous", "Polyamorous", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Transgender = new ModConfigurationKey<bool>("Transgender", "Transgender", () => false);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> CustomBadges = new ModConfigurationKey<string>("CustomBadges", "List of custom badges(csv)", () => "");

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BadgesVarInternalNames = new ModConfigurationKey<string>("BadgesVarInternalNames", "(csv)", () => "", internalAccessOnly: true);

        private static List<String> BadgesListNames;

        private static ModConfigurationKey<bool> changedvar = null;
        private static Uri url = null;
        private static bool skip = false;
        private static HashSet<AvatarManager> Avatars;

        private static BlendMode? blendMode = new BlendMode?();
        private static colorX? tint = new colorX?();
        private static int? maxSize = new int?(128);
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.OnThisConfigurationChanged += OnThisConfigurationChanged;
            Config.Save(true);
            BadgesListNames = Config.GetValue(BadgesVarInternalNames).Split(',').ToList();
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            Avatars = new HashSet<AvatarManager>();
            harmony.PatchAll();
        }

        private static void BadgesSwitch(string Name)
        {
            switch (Name)
            {
                case "CustomBadges":
                    skip = true;
                    break;
                case "Abrosexual":
                    changedvar = Abrosexual;
                    url = new Uri("resdb:///7397dfa5f6eee2fa8e1a5c2cede16d858c09bd60adc376fef6c0ca0727bbdbc9.webp");
                    break;
                case "Aegosexual":
                    changedvar = Aegosexual;
                    url = new Uri("resdb:///86da1550f4615939a96f86048c879a8cd60255173831a6218a6e80606035019d.png");
                    break;
                case "Aliquasexual":
                    changedvar = Aliquasexual;
                    url = new Uri("resdb:///3782b6a2f2cd194ec9e3da23f8b09ce348d2c98c3fc6fd79570c42ac79393642.webp");
                    break;
                case "Androsexual":
                    changedvar = Androsexual;
                    url = new Uri("resdb:///27f3cf42c0fc39f4b2e9911e056909319a44fdcaeb7a43beb1e8e555ebf1ee0a.webp");
                    break;
                case "Asexual":
                    changedvar = Asexual;
                    url = new Uri("resdb:///880fe58cae85e2a74cf491b00cfb818e023b40ff4a135a588be6114b8af95b80.webp");
                    break;
                case "Autosexual":
                    changedvar = Autosexual;
                    url = new Uri("resdb:///e0d1f42247998d5878820b97a3e072f66fa22e93e777d9df85c5e0350e0bcbf4.webp");
                    break;
                case "Bisexual":
                    changedvar = Bisexual;
                    url = new Uri("resdb:///11c7f8a885c0ae089a1e60e0cf4e9d14da7c2f8f497b7c0865091675c4ce2c6d.webp");
                    break;
                case "Ceterosexual":
                    changedvar = Ceterosexual;
                    url = new Uri("resdb:///bb618bc8933179128f615ce890b78d18510fb51af704d7ddfdf93b1444b80725.webp");
                    break;
                case "Demisexual":
                    changedvar = Demisexual;
                    url = new Uri("resdb:///a9119f05cef46e8c10218b2b16954df0c082cb1e1f7b0ce4abd3b2088ed17bf8.webp");
                    break;
                case "Fraysexual":
                    changedvar = Fraysexual;
                    url = new Uri("resdb:///cb5c469859d7830774efb0d4ddc0d3dfa6b32e7944259be2b0cae9cac5eb827f.webp");
                    break;
                case "GayGilbert":
                    changedvar = GayGilbert;
                    url = new Uri("resdb:///537f62fdbdf1e6f4d807c3525ac19f7d6b959f2fe7c5ede2b6ac17e6fa06d773.webp");
                    break;
                case "GayMaleFull":
                    changedvar = GayMaleFull;
                    url = new Uri("resdb:///7215acb9ae87241e122e5ecd5eecd96320e5a261f7421634b3b4c1f287ba26f9.webp");
                    break;
                case "GayMaleSimple":
                    changedvar = GayMaleSimple;
                    url = new Uri("resdb:///7fe18282b2cfee7e9e1c0a9aed96e01aacc239d128261a28cff1797095d253a9.webp");
                    break;
                case "Gay":
                    changedvar = Gay;
                    url = new Uri("resdb:///114e25ca8b823f670a3ba7c5bea1b1f100f28153c0637535ee36dd0a80bfd79e.webp");
                    break;
                case "Graysexual":
                    changedvar = Graysexual;
                    url = new Uri("resdb:///5abbc8354acd9d8b997aaac98ac1489a4050325a18e5bb4a26338c63ae29febf.webp");
                    break;
                case "Gynesexual":
                    changedvar = Gynesexual;
                    url = new Uri("resdb:///46627201f0c3063048180d77f6317643f9fa4793e94171e0ecd19f51d4ab77df.png");
                    break;
                case "Iculasexual":
                    changedvar = Iculasexual;
                    url = new Uri("resdb:///1e245bd9c409a3c72c6382785b9d1b3f477a3fefe1b11d118001e66d7eeeefbe.webp");
                    break;
                case "Kalossexual":
                    changedvar = Kalossexual;
                    url = new Uri("resdb:///64faa58dfbde77d7fb3f985404c700068d25e7f03030bdf72f44712dd9fd9fd3.webp");
                    break;
                case "Lesbian":
                    changedvar = Lesbian;
                    url = new Uri("resdb:///25bf8ee7717cdd0bf919e653526deb97066e380d58bdc6dc0bec6d790218b78a.webp");
                    break;
                case "Omnisexual":
                    changedvar = Omnisexual;
                    url = new Uri("resdb:///403b80574f33d18c66dd8b8442d687b7458bdee109de5dcec0890b7364ee9843.png");
                    break;
                case "Pansexual":
                    changedvar = Pansexual;
                    url = new Uri("resdb:///287ded390e7dc3cc39d7edec3b8bf9ee2fd7ef5390df8cae036c326580f6971d.webp");
                    break;
                case "Polysexual":
                    changedvar = Polysexual;
                    url = new Uri("resdb:///515d6210069e5e0a498894d93735ab14fbd68a183a47c1d71a96caa3aba1a786.webp");
                    break;
                case "Proligosexual":
                    changedvar = Proligosexual;
                    url = new Uri("resdb:///c303e649d2b5fdbe966210350cbca4d394c155ac862a2a188c4a600c9e1418f5.webp");
                    break;
                case "Queer":
                    changedvar = Queer;
                    url = new Uri("resdb:///6a59a796a3762bf0fb3e89623dbfd02086089770fc4f04e4abcb99185e1420cd.webp");
                    break;
                case "Agender":
                    changedvar = Agender;
                    url = new Uri("resdb:///921a4dd7f98e5c9ac0bd030184cd6d573a4f21cd9d74db7c145edf03c412def6.webp");
                    break;
                case "Aromantic":
                    changedvar = Aromantic;
                    url = new Uri("resdb:///8d7ac4b84b5b382688d24e6dd16d05a6e32dda22e09c6fbab48691f881c0bbc4.webp");
                    break;
                case "Demiromantic":
                    changedvar = Demiromantic;
                    url = new Uri("resdb:///02f17173f19ee93bc9fd49cb5114a771832de982b6e2bd6189274b073d7d6999.webp ");
                    break;
                case "Genderfluid":
                    changedvar = Genderfluid;
                    url = new Uri("resdb:///d4c84d4bc7df6a2f81a51cde3d841dcd7553477cd22be85154f7ef266b57cc05.webp");
                    break;
                case "Genderqueer":
                    changedvar = Genderqueer;
                    url = new Uri("resdb:///95c0a6bf841facff7d06dd317db281cd4a116e3e45fbc73e959eee977f235028.webp");
                    break;
                case "Intersex":
                    changedvar = Intersex;
                    url = new Uri("resdb:///6b65cb069fb631f8fab2d68ca895293622dadd70ccbad3c712a149222127cfa4.webp");
                    break;
                case "Naturist":
                    changedvar = Naturist;
                    url = new Uri("resdb:///0083d37b8bee3ccff5d4a1b493368eb23ed85dc5e17a4c8e3fa2dda1f94bbafb.webp");
                    break;
                case "Nonbinary":
                    changedvar = Nonbinary;
                    url = new Uri("resdb:///071cf2ec3f64978eba387b56954ed60b40d5765ca93ab695d7e0c5ac127d197b.webp");
                    break;
                case "Polyamorous":
                    changedvar = Polyamorous;
                    url = new Uri("resdb:///c599763137416fee3601cba1e69c621403eb5283579e20bde6bdf44a2babbe58.webp");
                    break;
                case "Transgender":
                    changedvar = Transgender;
                    url = new Uri("resdb:///94c9fd36191472b0473dd6f4dbad2de0baeab0b6d83b72ca2285f0cd36e8cd4c.webp");
                    break;
                default:
                    break;

            }
        }
        private void OnThisConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {

            Msg(" --- configurationChangedEvent.Key.Name: " + configurationChangedEvent.Key.Name + " --- ");
            if (configurationChangedEvent.Key == ENABLED) 
            {
                if (Config.GetValue(ENABLED))
                {
                    Msg(" --- ENABLED: {0} --- ", Config.GetValue(ENABLED));

                    foreach (string badge in BadgesListNames) 
                    {
                        BadgesSwitch(badge);
                        UpdateBadges(badge); 
                    }

                    List<String> CustomBadgesList = Config.GetValue(CustomBadges).Split(',').ToList();
                    Msg(" --- Adding  CustomBadges --- ");
                    Msg(" --- CustomBadgesList.Count: {0} --- ", CustomBadgesList.Count);
                    if (CustomBadgesList.Count > 0)
                    {
                        foreach (String customBadge in CustomBadgesList)
                        {
                            Msg(" --- customBadge.Length:{0} --- ", customBadge.Length);
                            if (customBadge.Length > 1)
                            {
                                Uri burl = new Uri(customBadge);
                                foreach (AvatarManager av in Avatars)
                                {
                                    av.Slot.RunSynchronously(delegate
                                    {
                                        av.AddIconBadge(burl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                    });
                                }
                            }
                        }
                    }
                }
                else 
                {
                    CleanBadges();
                }
                
            }
            else if (configurationChangedEvent.Key != BadgesVarInternalNames)
            { 
                String badgeName = configurationChangedEvent.Key.Name;
                BadgesSwitch(badgeName);
                UpdateBadges(badgeName);
                //Config.Save();
            }
        }

        private static void UpdateBadges(string _badgeName)
        {
            if (!skip)
            {
                bool KeyEnabled = Config.GetValue(changedvar);
                if (KeyEnabled)
                {
                    if (!BadgesListNames.Contains(_badgeName))
                    {
                        BadgesListNames.Add(_badgeName);
                    }
                }
                else
                {
                    if (BadgesListNames.Contains(_badgeName))
                    {
                        BadgesListNames.Remove(_badgeName);
                    }
                }

                if (url != null)
                {
                    foreach (AvatarManager av in Avatars)
                    {
                        av.Slot.RunSynchronously(delegate
                        {
                            HashSet<string> hashSet = Pool.BorrowHashSet<string>();
                            foreach (Slot child in av.BadgeTemplates.Children)
                            {
                                hashSet.Add(child.Name);
                            }
                            String BadgeNameID = "Extra Custom Badge-" + url.ToString().Substring(url.ToString().Length - 10, 5);
                            if (!hashSet.Contains(BadgeNameID))
                            {
                                if (KeyEnabled)
                                {
                                    BlendMode? blendMode = new BlendMode?();
                                    colorX? tint = new colorX?();
                                    int? maxSize = new int?(128);
                                    av.AddIconBadge(url, BadgeNameID, blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                }
                            }
                            else
                            {
                                if (!KeyEnabled)
                                {
                                    av.BadgeTemplates.FindChild(BadgeNameID).Destroy();
                                }
                            }
                            //Config.Set(BadgesVarInternalSlots, String.Join(",", BadgesListSlots));

                            av.UpdateBadges();
                        });
                    }
                }
            }
            else
            {
                foreach (AvatarManager av in Avatars)
                {
                    av.Slot.RunSynchronously(delegate
                    {
                        List<String> CustomBadgesList = Config.GetValue(CustomBadges).Split(',').ToList();
                        foreach (String customBadge in CustomBadgesList)
                        {
                            av.BadgeTemplates.FindChild("Extra Badge-", true, true, 1).Destroy();
                        }

                        foreach (String customBadge in CustomBadgesList)
                        {
                            if (customBadge.Length > 1)
                            {
                                Uri burl = new Uri(customBadge);
                                av.AddIconBadge(burl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                            }
                        }
                        av.UpdateBadges();
                    });

                }
            }
            Config.Set(BadgesVarInternalNames, String.Join(",", BadgesListNames).Trim(','));
            
        }
        private static void CleanBadges()
        {
            foreach (AvatarManager av in Avatars)
            {
                User user = av.Slot.ActiveUser;
                if (user.IsLocalUser)
                {
                    av.Slot.RunSynchronously(delegate
                    {

                            foreach (Slot child in av.BadgeTemplates.Children)
                            {
                            //av.BadgeTemplates.FindChild("Extra ", true, true, 1).Destroy();
                            if (child.Name.StartsWith("Extra "))
                                child.Destroy();
                        }

                        /* foreach (Slot child in av.BadgeTemplates.Children)
                         {
                            // av.BadgeTemplates.FindChild("Extra Badge-", true, true, 1).Destroy();
                             if (child.Name.StartsWith("Extra Badge-"))
                                 child.Destroy();
                         }*/
                        av.UpdateBadges();
                    });
                }
            }
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
                            if (Badges.Count() > 0 && Badges[0].Length > 1)
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

                                        foreach (String customBadge in Badges)
                                        {
                                            if (customBadge.Length > 1)
                                            {
                                                Uri lurl = new Uri(customBadge);
                                                avatarManager.AddIconBadge(lurl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                            }
                                        }

                                        foreach (string badge in BadgesListNames) {
                                            BadgesSwitch(badge); 
                                            UpdateBadges(badge); 
                                        }

                                        /*foreach (Uri BadgeName in BadgesListUri)
                                        {
                                            BadgesSwitch(BadgeName);
                                            avatarManager.AddIconBadge(url, BadgeName, blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                        }*/
                                        
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
