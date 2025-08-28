using Elements.Core;
using FrooxEngine;
using FrooxEngine.CommonAvatar;
using HarmonyLib;
using Renderite.Shared;
using ResoniteModLoader;
using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FasterBadges
{
    public class BadgeResourceManager
    {
        private readonly Dictionary<string, (Uri url, ModConfigurationKey<bool> configKey)> _resourceMap;

        public Dictionary<string, (Uri url, ModConfigurationKey<bool> configKey)> badgesMap { get { return _resourceMap; } }
        public BadgeResourceManager()
        {
            _resourceMap = new Dictionary<string, (Uri, ModConfigurationKey<bool>)>();
        }

        public void RegisterBadge(string name, Uri url, ModConfigurationKey<bool> configKey)
        {
            _resourceMap[name] = (url, configKey);
        }

        public (Uri? url, ModConfigurationKey<bool>? configKey, bool skip) GetBadgeData(string name)
        {
            if (_resourceMap.TryGetValue(name, out var data))
            {
                return (data.url, data.configKey, false);
            }
            return (null, null, name == "CustomBadges");
        }
    }

    public class AvatarBadgeHandler
    {
        private readonly HashSet<AvatarManager> _avatars;
        private readonly BlendMode? _blendMode;
        private readonly colorX? _tint;
        private readonly int? _maxSize;

        public HashSet<AvatarManager> avatars
        {
            get
            {
                return _avatars;
            }
        }
        public AvatarBadgeHandler(BlendMode? blendMode, colorX? tint, int? maxSize)
        {
            _avatars = new HashSet<AvatarManager>();
            _blendMode = blendMode;
            _tint = tint;
            _maxSize = maxSize;
        }
        public int AvatarCount() { return _avatars.Count; }
        public void AddAvatar(AvatarManager avatar)
        {
            User user = avatar.Slot.ActiveUser;
            if (user.IsLocalUser)
            {
                avatar.Disposing += (field) => { _avatars.Remove(avatar); };
                _avatars.Add(avatar);
            }
        }
        public IEnumerable<AvatarManager> GetAvatars()
        {
            return _avatars;
        }

        public void UpdateBadge(string badgeName, Uri url, ModConfiguration modConfiguration, ModConfigurationKey<bool> ckey)
        {
            if (modConfiguration == null || ckey == null)
            {
                return;
            }
            bool keyEnabled = modConfiguration.GetValue(ckey);
            foreach (AvatarManager av in _avatars)
            {
                av.Slot.RunSynchronously(() =>
                {
                    bool badgesChanged = false;

                    if (!(ckey == Patch.DALL || ckey == Patch.SUPP || ckey == Patch.HOST))
                    {
                        HashSet<string> hashSet = Pool.BorrowHashSet<string>();
                        foreach (Slot child in av.BadgeTemplates.Children)
                        {
                            hashSet.Add(child.Name);
                        }
                        String badgeNameId = "Extra Badge-" + url.ToString().Substring(url.ToString().Length - 10, 5);
                        if (!hashSet.Contains(badgeNameId))
                        {
                            if (keyEnabled)
                            {
                                av.AddIconBadge(url, badgeNameId, _blendMode, _tint, TextureFilterMode.Bilinear, _maxSize);
                                badgesChanged = true;
                            }
                        }
                        else
                        {
                            if (!keyEnabled)
                            {
                                av.BadgeTemplates.FindChild(badgeNameId, true, true, 1).Destroy();
                                badgesChanged = true;
                            }
                        }
                    }
                    if (badgesChanged)
                    {
                        av.UpdateBadges();
                    }
                });
            }
        }
        public void CleanBadges()
        {
            foreach (AvatarManager av in _avatars)
            {
                User user = av.Slot.ActiveUser;
                if (user.IsLocalUser)
                {
                    av.Slot.RunSynchronously(() =>
                    {
                        var badgesToRemove = av.BadgeTemplates.Children
                            .Where(child => child.Name.StartsWith("Extra "))
                            .ToList();

                        foreach (var badge in badgesToRemove)
                        {
                            badge.Destroy();
                        }
                        av.UpdateBadges();
                    });
                }
            }
        }

    }
    public class Patch : ResoniteMod
    {
        public override String Name => "FasterBadges";
        public override String Author => "zahndy";
        public override String Link => "https://github.com/zahndy/FasterBadges";
        public override String Version => "1.6.1";

        private static readonly BadgeResourceManager _resourceManager = new BadgeResourceManager();
        private static readonly AvatarBadgeHandler _avatarHandler = new AvatarBadgeHandler(blendMode, tint, maxSize);
        private static List<String> BadgesListNames = new List<string>();
        public static ModConfiguration Config;

        private const string HEADER_TEXT_COLOR = "#BA64F2";
        private static Predicate<string> checkNull = delegate (string str) { return (str != null); };

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY_ = new ModConfigurationKey<dummy>("dummy_", "<size=150>For Accessibility badges please see https://wiki.resonite.com/Resonite_Bot#Assignable_Badges</size> ");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY = new ModConfigurationKey<dummy>("dummy", $"<size=400>\nDynamically adding and removing badges is difficult due to the AvatarBadgeManager animating the badges.\nyou may end up with overlapping badges until you respawn.</size> ");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY0 = new ModConfigurationKey<dummy>("dummy0", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY1 = new ModConfigurationKey<dummy>("dummy1Line", $"<color={HEADER_TEXT_COLOR}>---------------------------------------------------------------------------------------------------------------------------------</color>");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY2 = new ModConfigurationKey<dummy>("DUMMY_2", $"<align=center><color={HEADER_TEXT_COLOR}>[ Age Related ]</color></align> ", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Under18 = new ModConfigurationKey<bool>("Under18", "Under 18", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Over18 = new ModConfigurationKey<bool>("Over18", "Over 18", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Minor = new ModConfigurationKey<bool>("Minor", "Minor", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Adult = new ModConfigurationKey<bool>("Adult", "Adult", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> VeryOld = new ModConfigurationKey<bool>("VeryOld", "Very old", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Fossil = new ModConfigurationKey<bool>("Fossil", "Fossil", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY17 = new ModConfigurationKey<dummy>("DUMMY_17", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY3 = new ModConfigurationKey<dummy>("DUMMY_3", $"<align=center><color={HEADER_TEXT_COLOR}>[ Various ]</color></align> ", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Avali = new ModConfigurationKey<bool>("Avali", "Avali", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ADHD = new ModConfigurationKey<bool>("ADHD", "ADHD", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ADHDFlag = new ModConfigurationKey<bool>("ADHDFlag", "ADHD Flag", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> NOLewd = new ModConfigurationKey<bool>("NOLewd", "NO Lewd", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> STOPhantom = new ModConfigurationKey<bool>("STOPhantom", "STOP! Phantom Pain", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> PhantomCircle = new ModConfigurationKey<bool>("PhantomCircle", "! Phantom Sense", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> USFN = new ModConfigurationKey<bool>("USFN", "USFN", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY18 = new ModConfigurationKey<dummy>("DUMMY_18", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY4 = new ModConfigurationKey<dummy>("DUMMY_4", $"<align=center><color={HEADER_TEXT_COLOR}>[ Heart Pride ]</color></align> ", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Abrosexual = new ModConfigurationKey<bool>("Abrosexual", "Abrosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Aliquasexual = new ModConfigurationKey<bool>("Aliquasexual", "Aliquasexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Androsexual = new ModConfigurationKey<bool>("Androsexual", "Androsexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Autosexual = new ModConfigurationKey<bool>("Autosexual", "Autosexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Ceterosexual = new ModConfigurationKey<bool>("Ceterosexual", "Ceterosexual", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY5 = new ModConfigurationKey<dummy>("DUMMY_5", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Fraysexual = new ModConfigurationKey<bool>("Fraysexual", "Fraysexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> GayMaleFull = new ModConfigurationKey<bool>("GayMaleFull", "Gay Male (Full)", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> GayMaleSimple = new ModConfigurationKey<bool>("GayMaleSimple", "Gay Male (Simple)", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Gay = new ModConfigurationKey<bool>("Gay", "Gay", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Gynesexual = new ModConfigurationKey<bool>("Gynesexual", "Gynesexual", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY7 = new ModConfigurationKey<dummy>("DUMMY_7", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Iculasexual = new ModConfigurationKey<bool>("Iculasexual", "Iculasexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Kalossexual = new ModConfigurationKey<bool>("Kalossexual", "Kalossexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Omnisexual = new ModConfigurationKey<bool>("Omnisexual", "Omnisexual", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Proligosexual = new ModConfigurationKey<bool>("Proligosexual", "Proligosexual", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY19 = new ModConfigurationKey<dummy>("DUMMY_19", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());     
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY9 = new ModConfigurationKey<dummy>("DUMMY_9", $"<align=center><color={HEADER_TEXT_COLOR}>[ Diamond Identity ]</color></align> ", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Demiromantic = new ModConfigurationKey<bool>("Demiromantic", "Demiromantic", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Naturist = new ModConfigurationKey<bool>("Naturist", "Naturist", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> Polyamorous = new ModConfigurationKey<bool>("Polyamorous", "Polyamorous", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY20 = new ModConfigurationKey<dummy>("DUMMY_29", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());       
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY11 = new ModConfigurationKey<dummy>("DUMMY_11", $"<align=center><color={HEADER_TEXT_COLOR}>[ Languages ]</color></align> ", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> AF = new ModConfigurationKey<bool>("AF", "Afrikaans", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> AR = new ModConfigurationKey<bool>("AR", "Arabic", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> BN = new ModConfigurationKey<bool>("BN", "Bengali", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ZH = new ModConfigurationKey<bool>("ZH", "Chinese", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> DA = new ModConfigurationKey<bool>("DA", "Danish", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY12 = new ModConfigurationKey<dummy>("DUMMY_12", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> NL = new ModConfigurationKey<bool>("NL", "Dutch", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EN = new ModConfigurationKey<bool>("EN", "English", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> FI = new ModConfigurationKey<bool>("FI", "Finnish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> FR = new ModConfigurationKey<bool>("FR", "French", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> DE = new ModConfigurationKey<bool>("DE", "German", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY13 = new ModConfigurationKey<dummy>("DUMMY_13", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> HI = new ModConfigurationKey<bool>("HI", "Hindi", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> HU = new ModConfigurationKey<bool>("HU", "Hungarian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> IT = new ModConfigurationKey<bool>("IT", "Italian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> JA = new ModConfigurationKey<bool>("JA", "Japanese", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> KO = new ModConfigurationKey<bool>("KO", "Korean", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY14 = new ModConfigurationKey<dummy>("DUMMY_14", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> LT = new ModConfigurationKey<bool>("LT", "Lithuanian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> MR = new ModConfigurationKey<bool>("MR", "Marathi", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> NO = new ModConfigurationKey<bool>("NO", "Norwegian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> PL = new ModConfigurationKey<bool>("PL", "Polish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> PT = new ModConfigurationKey<bool>("PT", "Portuguese", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY15 = new ModConfigurationKey<dummy>("DUMMY_15", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> RU = new ModConfigurationKey<bool>("RU", "Russian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ES = new ModConfigurationKey<bool>("ES", "Spanish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> SV = new ModConfigurationKey<bool>("SV", "Swedish", () => false);


        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY21 = new ModConfigurationKey<dummy>("DUMMY_21", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY16 = new ModConfigurationKey<dummy>("DUMMY_16", $"<align=center><color={HEADER_TEXT_COLOR}>[ Custom Badges ]</color></align> ", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> CustomBadges = new ModConfigurationKey<string>("CustomBadges", "List of custom badges(csv of urls: \"url1,url2,url3\" resdb or http)", () => "");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY22 = new ModConfigurationKey<dummy>("DUMMY_22", $"<color={HEADER_TEXT_COLOR}></color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY23 = new ModConfigurationKey<dummy>("DUMMY_23", $"<align=center><color={HEADER_TEXT_COLOR}>[ Remove Default Badges if present. (Requires respawn to revert) ]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        public static ModConfigurationKey<bool> HOST = new ModConfigurationKey<bool>("HOST", "Host", () => false);
        [AutoRegisterConfigKey]
        public static ModConfigurationKey<bool> SUPP = new ModConfigurationKey<bool>("SUPP", "Supporter", () => false);
        [AutoRegisterConfigKey]
        public static ModConfigurationKey<bool> DALL = new ModConfigurationKey<bool>("DALL", "Disable ALL", () => false);

        private static BlendMode? blendMode = new BlendMode?();
        private static colorX? tint = new colorX?();
        private static int? maxSize = new int?(128);

        public Patch()
        {
        }

        public override void OnEngineInit()
        {
            Config = GetConfiguration()!;
            InitializeBadgeResources();
            Config.OnThisConfigurationChanged += OnThisConfigurationChanged;
            Config.Save(true);

            InitializeActiveBadges();

            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            harmony.PatchAll();
        }

        private static void InitializeBadgeResources()
        {
            // Register all badges with the resource manager
            _resourceManager.RegisterBadge("Under18",
                new Uri("resdb:///030a337b2f1038c4e833dbef2c53bea30e47117c22ec5b01e65cb59c7e76380b.png"),
                 Under18);
            _resourceManager.RegisterBadge("Over18",
                new Uri("resdb:///874e0c62cf6a8bda5a65ffe7518617e5742339d0c362d5717e8dc6d7e05c5eac.png"),
                Over18);
            _resourceManager.RegisterBadge("Minor",
                new Uri("resdb:///9dff86e3142f439ee273c57a67e5706ec20d60c5e5179dd59f238c8a24e6c923.png"),
                Minor);
            _resourceManager.RegisterBadge("Adult",
                new Uri("resdb:///bf80832420136d2d1029011dd2295a3871c97e4624b90e3d628a935f0301f087.png"),
                Adult);
            _resourceManager.RegisterBadge("VeryOld",
                new Uri("resdb:///830b795065d5ea8f0458f7390c8b0bac4b0df6f453bcec539fd08f46ab99e88b.png"),
                VeryOld);
            _resourceManager.RegisterBadge("Fossil",
                new Uri("resdb:///6815fa0f9656d94cf108054331c4fff47904426eac29044dc71406dba1085c37.png"),
                Fossil);
            _resourceManager.RegisterBadge("Avali",
                new Uri("resdb:///6548f96f2b16bbeb8538dddb7c5c94ff2645823de54cf85b37da97e6b9a8f5c8.png"),
                Avali);
            _resourceManager.RegisterBadge("ADHD",
                new Uri("resdb:///3b57b6ce48b8d1fbe295942ab4883d830d039faaba6be2251d32baebbbbfc71c.png"),
                ADHD);
            _resourceManager.RegisterBadge("ADHDFlag",
                new Uri("resdb:///5c24a24f980300d9066c2eafdf2d57d328404f7bebe37472365a19ec6d2a77f6.png"),
                ADHDFlag);
            _resourceManager.RegisterBadge("NOLewd",
                new Uri("resdb:///c6f7561c0f5b0ca7d986c23a36d0af1681e2779ae1c1e1db9e98b10866345fbf.png"),
                NOLewd);
            _resourceManager.RegisterBadge("STOPhantom",
                new Uri("resdb:///d84873aa4025c12a26b10d51d86bde48caa4a2b8f7c4eba96fff6c324c8ba5cd.png"),
                STOPhantom);
            _resourceManager.RegisterBadge("PhantomCircle",
                new Uri("resdb:///8c8066dc639d9235f0de37a66e90f647534dd670e2166eaf6db2e8753a049266"),
                PhantomCircle);
            _resourceManager.RegisterBadge("USFN",
                new Uri("resdb:///7886e38f5d36f41d7ee3fdcbd520867bbe737a2bfb6cbdde2d0af9c0d20d1d3d.png"),
                USFN);
            _resourceManager.RegisterBadge("Abrosexual",
                new Uri("resdb:///7397dfa5f6eee2fa8e1a5c2cede16d858c09bd60adc376fef6c0ca0727bbdbc9.webp"),
                Abrosexual);
            _resourceManager.RegisterBadge("Aliquasexual",
                new Uri("resdb:///3782b6a2f2cd194ec9e3da23f8b09ce348d2c98c3fc6fd79570c42ac79393642.webp"),
                Aliquasexual);
            _resourceManager.RegisterBadge("Androsexual",
                new Uri("resdb:///27f3cf42c0fc39f4b2e9911e056909319a44fdcaeb7a43beb1e8e555ebf1ee0a.webp"),
                Androsexual);
            _resourceManager.RegisterBadge("Autosexual",
                new Uri("resdb:///e0d1f42247998d5878820b97a3e072f66fa22e93e777d9df85c5e0350e0bcbf4.webp"),
                Autosexual);
            _resourceManager.RegisterBadge("Ceterosexual",
                new Uri("resdb:///bb618bc8933179128f615ce890b78d18510fb51af704d7ddfdf93b1444b80725.webp"),
                Ceterosexual);
            _resourceManager.RegisterBadge("Fraysexual",
                new Uri("resdb:///cb5c469859d7830774efb0d4ddc0d3dfa6b32e7944259be2b0cae9cac5eb827f.webp"),
                Fraysexual);
            _resourceManager.RegisterBadge("GayMaleFull",
                new Uri("resdb:///7215acb9ae87241e122e5ecd5eecd96320e5a261f7421634b3b4c1f287ba26f9.webp"),
                GayMaleFull);
            _resourceManager.RegisterBadge("GayMaleSimple",
                new Uri("resdb:///7fe18282b2cfee7e9e1c0a9aed96e01aacc239d128261a28cff1797095d253a9.webp"),
                GayMaleSimple);
            _resourceManager.RegisterBadge("Gay",
                new Uri("resdb:///114e25ca8b823f670a3ba7c5bea1b1f100f28153c0637535ee36dd0a80bfd79e.webp"),
                Gay);
            _resourceManager.RegisterBadge("Gynesexual",
                new Uri("resdb:///46627201f0c3063048180d77f6317643f9fa4793e94171e0ecd19f51d4ab77df.png"),
                Gynesexual);
            _resourceManager.RegisterBadge("Iculasexual",
                new Uri("resdb:///1e245bd9c409a3c72c6382785b9d1b3f477a3fefe1b11d118001e66d7eeeefbe.webp"),
                Iculasexual);
            _resourceManager.RegisterBadge("Kalossexual",
                new Uri("resdb:///64faa58dfbde77d7fb3f985404c700068d25e7f03030bdf72f44712dd9fd9fd3.webp"),
                Kalossexual);
            _resourceManager.RegisterBadge("Omnisexual",
                new Uri("resdb:///403b80574f33d18c66dd8b8442d687b7458bdee109de5dcec0890b7364ee9843.png"),
                Omnisexual);
            _resourceManager.RegisterBadge("Proligosexual",
                new Uri("resdb:///c303e649d2b5fdbe966210350cbca4d394c155ac862a2a188c4a600c9e1418f5.webp"),
                Proligosexual);
            _resourceManager.RegisterBadge("Demiromantic",
                new Uri("resdb:///02f17173f19ee93bc9fd49cb5114a771832de982b6e2bd6189274b073d7d6999.webp "),
                Demiromantic);
            _resourceManager.RegisterBadge("Naturist",
                new Uri("resdb:///0083d37b8bee3ccff5d4a1b493368eb23ed85dc5e17a4c8e3fa2dda1f94bbafb.webp"),
                Naturist);
            _resourceManager.RegisterBadge("Polyamorous",
                new Uri("resdb:///c599763137416fee3601cba1e69c621403eb5283579e20bde6bdf44a2babbe58.webp"),
                Polyamorous);
            _resourceManager.RegisterBadge("AF",
                new Uri("resdb:///f9f1c2142d6cce45f38729480a3a457f70774f48fcb915cf9e7db59f9dee5d54.png"),
                AF);
            _resourceManager.RegisterBadge("AR",
                new Uri("resdb:///f110617fe3fc8fc26372ef0ee88f469a4d02186ab1cc42b51fda1261d8ae03b2.png"),
                AR);
            _resourceManager.RegisterBadge("BN",
                new Uri("resdb:///8f1d00595cd2722cda4bfb14eaf632033cf914d9af9709a321c66a8b04312d84.png"),
                BN);
            _resourceManager.RegisterBadge("DA",
                new Uri("resdb:///0f4497240ebac60bf8553a08766a9657660387387da5d0db378316102ec23029.png"),
                DA);
            _resourceManager.RegisterBadge("DE",
                new Uri("resdb:///9f8a177ae178a8ac426b97ea0e58ede1143ca32ef3a2a05aef1e2f14e7bd7080.png"),
                DE);
            _resourceManager.RegisterBadge("EN",
                new Uri("resdb:///ec1225cb99da4205f5f37dcabd72bc0ae7ebc2b8e706a9817d236f8ef18a90bd.png"),
                EN);
            _resourceManager.RegisterBadge("ES",
                new Uri("resdb:///b241f3e7ed8bfc158bd2ba67fd91c33dd710e8ef504e844b41ccb9fc68d29520.png"),
                ES);
            _resourceManager.RegisterBadge("FI",
                new Uri("resdb:///b5ba76422dec60db608f68ad7d40af8583e3e59653889707410b0acaef729d4f.png"),
                FI);
            _resourceManager.RegisterBadge("FR",
                new Uri("resdb:///c6b29e2ec07370deca43067d3a928b8dd6a4b4d7a50f97106becae49869fb267.png"),
                FR);
            _resourceManager.RegisterBadge("HI",
                new Uri("resdb:///e91793ee1cdfa821ead79bb72eb4c00e5e1658fdcc35ac705f4c26d28496fdb6.png"),
                HI);
            _resourceManager.RegisterBadge("HU",
                new Uri("resdb:///0c4113b6ba52d09b1882a510d3634c38a9c88bb3d42caf7e3050b26c0a52ca5e.png"),
                HU);
            _resourceManager.RegisterBadge("IT",
                new Uri("resdb:///9e46fd023008e6e7cee2b2217ffe325c92604f72730eacbb63ca46b956cc73aa.png"),
                IT);
            _resourceManager.RegisterBadge("JA",
                new Uri("resdb:///e520e3d7c0ee42d353f79607614db651643294ea6af750fba9ac4a860b268268.png"),
                JA);
            _resourceManager.RegisterBadge("KO",
                new Uri("resdb:///0c5d8c10070c89b26aabdb2c15e9976de4658ce8cdac9187c9becb029af3bcd1.png"),
                KO);
            _resourceManager.RegisterBadge("LT",
                new Uri("resdb:///c49ba42d38d6e9e210acd44fd8be07428ab84bf42374ec533cbc4eeaf262d20c.png"),
                LT);
            _resourceManager.RegisterBadge("MR",
                new Uri("resdb:///8f78afb4be2944336aa7dc2a066576d12df72a4c8929af9ca46187cc7464d446.png"),
                MR);
            _resourceManager.RegisterBadge("NL",
                new Uri("resdb:///3a4b56f165aad42de75432680fae47dd06dd2a35d79d249498e828c21f0a9293.png"),
                NL);
            _resourceManager.RegisterBadge("NO",
                new Uri("resdb:///ef322f0e9dd3352809739c280e70ae65ae342332a93c65586f770f94814f418c.png"),
                NO);
            _resourceManager.RegisterBadge("PL",
                new Uri("resdb:///f6f37a9a7823e1fc4057a9673f244b68ca36455c45cb5433d9a8d64cb75a3db7.png"),
                PL);
            _resourceManager.RegisterBadge("PT",
                new Uri("resdb:///56ec0c6846bf0484615ddf8b4fe1e38038255b71b8e9e17270a1b7a590cac868.png"),
                PT);
            _resourceManager.RegisterBadge("RU",
                new Uri("resdb:///0ee18103333617e52e4b5607a1631c5655c38a3a9f632737bf0ac2d52842fabd.png"),
                RU);
            _resourceManager.RegisterBadge("SV",
                new Uri("resdb:///d7dd94e50e366757491fd2f695f5fb45061925e6ac59636a957b3a85c7ead6f5.png"),
                SV);
            _resourceManager.RegisterBadge("ZH",
                new Uri("resdb:///5a59c0ac93743f931e93b3889736fd776a2122744bf4dbed3a8b343f04f2974b.png"),
                ZH);
            _resourceManager.RegisterBadge("HOST",
                new Uri("resdb:///cef8313f2418512a52c718a505c8882684dfa6556bdf2af1da655d3e6a0f878e.png"),
                HOST);
            _resourceManager.RegisterBadge("SUPP",
                new Uri("about:blank"),
                SUPP);
            _resourceManager.RegisterBadge("DALL",
                new Uri("about:blank"),
                DALL);
        }

        private void InitializeActiveBadges()
        {
            foreach (ModConfigurationKey configurationItemDefinition in Config.ConfigurationItemDefinitions)
            {
                if (configurationItemDefinition.ValueType() != typeof(dummy) && configurationItemDefinition != ENABLED && configurationItemDefinition != DALL && configurationItemDefinition != SUPP && configurationItemDefinition != HOST)
                {
                    if (Config.GetValue(configurationItemDefinition).GetType() == typeof(bool))
                    {
                        bool value = (bool)Config.GetValue(configurationItemDefinition);
                        if (value)
                        {
                            //Msg("Adding badge: " + configurationItemDefinition.Name);
                            BadgesListNames.Add(configurationItemDefinition.Name);
                        }
                    }
                }
            }
        }

        private void OnThisConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
           // Msg("Configuration Key Changed: " + configurationChangedEvent.Key.Name);
            if (configurationChangedEvent.Key == ENABLED)
            {
                HandleEnabledStateChange();
            }
            else if (configurationChangedEvent.Key == CustomBadges)
            {
                HandleCustomBadgesChange();
            }
            else if (configurationChangedEvent.Key == HOST || configurationChangedEvent.Key == DALL || configurationChangedEvent.Key == SUPP)
            {
                HandleStockBadgesRemoval();
            }
            else
            {
                HandleIndividualBadgeChange(configurationChangedEvent.Key.Name);
            }

            string customBadgesValue = Config.GetValue(CustomBadges)!;
            if (string.IsNullOrEmpty(customBadgesValue) || customBadgesValue.Length < 10) //clear custom badges
            {
                foreach (AvatarManager av in _avatarHandler.GetAvatars())
                {
                    if (av?.BadgeTemplates == null) continue; // Skip if avatar or badges are null

                    av.Slot.RunSynchronously(() =>
                    {
                        try
                        {
                            var badgesToRemove = av.BadgeTemplates.Children.Any()
                               ? av.BadgeTemplates.Children
                                   .Where(child => child != null && child.Name.Contains("Extra Custom Badge-"))
                                   .ToList()
                               : new List<Slot>();

                            // Remove the badges after collection
                            foreach (var badge in badgesToRemove)
                            {
                                if (badge != null)
                                {
                                   // Msg("Removing Custom Badge: " + badge.Name + " From: " + (av.Slot.ActiveUser?.UserName ?? "Unknown User"));
                                    badge.Destroy();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Msg("Error cleaning custom badges: " + ex.Message);
                        }
                    });
                }
            }

        }

        private void HandleEnabledStateChange()
        {
            if (Config.GetValue(ENABLED))
            {
                _avatarHandler.CleanBadges();
                RefreshAllBadges();
            }
            else
            {
                _avatarHandler.CleanBadges();
            }
        }

        private void HandleCustomBadgesChange()
        {
            if (!String.IsNullOrEmpty(Config.GetValue(CustomBadges)))
            {
                Userspace.Current.RunSynchronously(() =>
                {
                    RefreshCustomBadges();
                });

            }
        }

        private void HandleIndividualBadgeChange(string badgeName)
        {
            Userspace.Current.RunSynchronously(() =>
            {
                var badgeData = _resourceManager.GetBadgeData(badgeName);
                if (badgeData.url == null || badgeData.configKey == null) return;

                _avatarHandler.UpdateBadge(badgeName, badgeData.url, Config, badgeData.configKey);
            });
        }

        private void RefreshAllBadges()
        {
            foreach (string badge in BadgesListNames)
            {
                var badgeData = _resourceManager.GetBadgeData(badge);
                if (badgeData.url != null && badgeData.configKey != null)
                {
                    _avatarHandler.UpdateBadge(badge, badgeData.url, Config, badgeData.configKey);
                }
            }
        }

        private static void HandleStockBadgesRemoval()
        {
            if (Config.GetValue(HOST))
            {
                foreach (var av in _avatarHandler.GetAvatars())
                {
                    if (av?.BadgeTemplates == null) continue;

                    av.Slot.RunSynchronously(() =>
                    {
                        var hostBadge = av.BadgeTemplates.FindChild("Host", true, true, 1);
                        if (hostBadge != null)
                        {
                           // Msg("Deleting Host Badge: " + hostBadge.Name + " From: " + (av.Slot.ActiveUser?.UserName ?? "Unknown User"));
                            hostBadge.Destroy();
                        }
                        av.UpdateBadges();
                    });
                }
            }
            if (Config.GetValue(DALL))
            {
                foreach (var av in _avatarHandler.GetAvatars())
                {
                    av.Slot.RunSynchronously(() =>
                    {
                        var badgesToRemove = av.BadgeTemplates.Children
                        .Where(badge => badge != null && !badge.Name.Contains("Extra "))
                        .ToList();

                        foreach (var badge in badgesToRemove)
                        {
                            badge.Destroy();
                        }
                        av.UpdateBadges();
                    });
                }
            }
            if (Config.GetValue(SUPP))
            {
               // Msg("Attempting to delete Supporter Badge");
                foreach (var av in _avatarHandler.GetAvatars())
                {
                    av.Slot.RunSynchronously(() =>
                    {
                        var suppBadgeTemplate = av.BadgeTemplates.FindChild("Supporter", true, true, 1);
                        if (suppBadgeTemplate != null)
                        {
                           // Msg("Deleting Supporter Badge: " + suppBadgeTemplate.Name + " From: " + (av.Slot.ActiveUser?.UserName ?? "Unknown User"));
                            suppBadgeTemplate.Destroy();
                        }
                        av.UpdateBadges();
                    });
                }
            }
        }

        private void RefreshCustomBadges()
        {
            string customBadgesStr = Config.GetValue(CustomBadges)!.Trim(',', ' ');
            List<String> customBadges = customBadgesStr.Split(',').ToList();

            if (customBadgesStr.Length > 10)
            {
                foreach (AvatarManager av in _avatarHandler.GetAvatars())
                {
                    av.Slot.RunSynchronously(() =>
                    {
                        bool badgesChanged = false;

                        HashSet<string> hashSet = Pool.BorrowHashSet<string>();

                        var existingBadges = av.BadgeTemplates.Children
                           .Where(child => child.Name.Contains("Extra Custom Badge-"))
                           .ToList();

                        foreach (var child in existingBadges)
                        {
                            if (customBadges.Contains(child.Name))
                            {
                                if (customBadges.Contains(child.Name))
                                {
                                    hashSet.Add(child.Name);
                                }
                                else
                                {
                                    child.Destroy();
                                    badgesChanged = true;
                                }
                            }
                        }

                        foreach (String customBadge in customBadges)
                        {
                            if (customBadge.Length > 10)
                            {
                                Uri badgeUrl = new Uri(customBadge);

                                String badgeNameId = "Extra Custom Badge-" + customBadge.Substring(customBadge.Length - 10, 5);
                                if (!hashSet.Contains(badgeNameId))
                                {
                                    av.AddIconBadge(badgeUrl,
                                        "Extra Custom Badge-" + customBadge.Substring(customBadge.Length - 10, 5),
                                        blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                    badgesChanged = true;
                                   // Msg("Added Custom Badge to: " + av.Slot.Name + " From: " + av.Slot.ActiveUser.UserName);
                                }
                            }

                        }
                        if (badgesChanged)
                        {
                            av.UpdateBadges();
                        }
                    });
                }
            }


        }
        private static void InitCustomBadgesForAvatar(AvatarManager avatarManager)
        {
            if (String.IsNullOrEmpty(Config.GetValue(CustomBadges))) return;

            String[] badges = Config.GetValue(CustomBadges)!.Trim(',').Split(',');
            if (badges.Length == 0 || badges[0].Length < 10) return;

            foreach (String customBadge in badges)
            {
                if (customBadge.Length > 10)
                {
                    Uri lurl = new Uri(customBadge);
                    avatarManager.AddIconBadge(lurl, "Extra Custom Badge-" + customBadge.Substring(customBadge.Length - 10, 5),
                        blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                }
            }
        }

        private static void InitAllBadgesForAvatar(AvatarManager avatarManager)
        {
            foreach (string badge in BadgesListNames)
            {
                var badgeData = _resourceManager.GetBadgeData(badge);
                if (!badgeData.skip && badgeData.url != null && badgeData.configKey != null)
                {
                    bool keyEnabled = Config.GetValue(badgeData.configKey);

                    if (keyEnabled)
                    {
                        String badgeNameId = "Extra Badge-" + badgeData.url.ToString().Substring(badgeData.url.ToString().Length - 10, 5);
                        avatarManager.AddIconBadge(badgeData.url, badgeNameId,
                            blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                    }

                }
            }
        }
        private static void HandleAvatarAttachment(AvatarManager avatarManager)
        {
            avatarManager.Slot.RunSynchronously(() =>
            {
                if (!_avatarHandler.avatars.Contains(avatarManager))
                {
                    _avatarHandler.AddAvatar(avatarManager);

                    HandleStockBadgesRemoval();

                    InitCustomBadgesForAvatar(avatarManager);
                    InitAllBadgesForAvatar(avatarManager);
                }
            });
        }

        [HarmonyPatch(typeof(AvatarBadgeManager))]
        class AvatarBadgeManager_OnAttach_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("OnAttach")]
            static void Prefix(AvatarBadgeManager __instance)
            {
                if (!Config.GetValue(ENABLED)) return;

                User user = __instance.Slot.ActiveUser;
                if (user?.UserName == null || !user.IsLocalUser) return;

                if (String.IsNullOrEmpty(CustomBadges.ToString()) &&
                    CustomBadges == null &&
                    BadgesListNames.Count == 0) return;

                UserRoot userRoot = user.Root;
                AvatarManager avatarManager = userRoot.Slot.GetComponent<AvatarManager>();
                if (avatarManager == null) return;

                if (avatarManager.Slot.Name == "UserRoot") return;

                HandleAvatarAttachment(avatarManager);
            }
        }
    }
}
