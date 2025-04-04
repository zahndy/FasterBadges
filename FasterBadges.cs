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
using static FrooxEngine.FullBodyCalibratorDialog;

namespace FasterBadges
{
    public class Patch : ResoniteMod
    {
        public override String Name => "FasterBadges";
        public override String Author => "zahndy";
        public override String Link => "https://github.com/zahndy/FasterBadges";
        public override String Version => "1.2.0";

        public static ModConfiguration Config;

        const string HEADER_TEXT_COLOR = "#BA64F2";
        private static Predicate<string> checkNull = delegate (string str) { return (str != null); };

        public delegate void ConfigurationChangedHandler(ConfigurationChangedEvent configurationChangedEvent);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY = new ModConfigurationKey<dummy>("dummy", "For Accessibility badges please see https://wiki.resonite.com/Resonite_Bot#Assignable_Badges");
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY1 = new ModConfigurationKey<dummy>("dummy1Line", $"<color={HEADER_TEXT_COLOR}>---------------------------------------------------------------------------------------------------------------------------------</color>");

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY2 = new ModConfigurationKey<dummy>("DUMMY_2", $"<color={HEADER_TEXT_COLOR}>[ Age Related ]</color>", () => new dummy());

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
        private static readonly ModConfigurationKey<dummy> DUMMY3 = new ModConfigurationKey<dummy>("DUMMY_3", $"<color={HEADER_TEXT_COLOR}>[ Various ]</color>", () => new dummy());

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
        private static readonly ModConfigurationKey<dummy> DUMMY4 = new ModConfigurationKey<dummy>("DUMMY_4", $"<color={HEADER_TEXT_COLOR}>[ Heart Pride ]</color>", () => new dummy());

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
        private static readonly ModConfigurationKey<dummy> DUMMY5 = new ModConfigurationKey<dummy>("DUMMY_5", $"<color={HEADER_TEXT_COLOR}>[ Diamond Identity ]</color>", () => new dummy());
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
        private static readonly ModConfigurationKey<dummy> DUMMY6 = new ModConfigurationKey<dummy>("DUMMY_6", $"<color={HEADER_TEXT_COLOR}>[ Languages ]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> AF = new ModConfigurationKey<bool>("AF", "Afrikaans", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> AR = new ModConfigurationKey<bool>("AR", "Arabic", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> BN = new ModConfigurationKey<bool>("BN", "Bengali", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> DA = new ModConfigurationKey<bool>("DA", "Danish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> DE = new ModConfigurationKey<bool>("DE", "German", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EN = new ModConfigurationKey<bool>("EN", "English", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ES = new ModConfigurationKey<bool>("ES", "Spanish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> FI = new ModConfigurationKey<bool>("FI", "Finnish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> FR = new ModConfigurationKey<bool>("FR", "French", () => false);
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
        private static ModConfigurationKey<bool> LT = new ModConfigurationKey<bool>("LT", "Lithuanian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> MR = new ModConfigurationKey<bool>("MR", "Marathi", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> NL = new ModConfigurationKey<bool>("NL", "Dutch", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> NO = new ModConfigurationKey<bool>("NO", "Norwegian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> PL = new ModConfigurationKey<bool>("PL", "Polish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> PT = new ModConfigurationKey<bool>("PT", "Portuguese", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> RU = new ModConfigurationKey<bool>("RU", "Russian", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> SV = new ModConfigurationKey<bool>("SV", "Swedish", () => false);
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> ZH = new ModConfigurationKey<bool>("ZH", "Chinese", () => false);

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<dummy> DUMMY7 = new ModConfigurationKey<dummy>("DUMMY_7", $"<color={HEADER_TEXT_COLOR}>[ Custom Badges ]</color>", () => new dummy());
        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> CustomBadges = new ModConfigurationKey<string>("CustomBadges", "List of custom badges(csv of urls: \"url1,url2,url3\" resdb or http)", () => "",false ,checkNull);

        private static List<String> BadgesListNames;

        private static HashSet<AvatarManager> Avatars;

        private static BlendMode? blendMode = new BlendMode?();
        private static colorX? tint = new colorX?();
        private static int? maxSize = new int?(128);
        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.OnThisConfigurationChanged += OnThisConfigurationChanged;
            Config.Save(true);
            BadgesListNames = new List<string>();
            foreach (ModConfigurationKey configurationItemDefinition in Config.ConfigurationItemDefinitions)
            {
                if (configurationItemDefinition.ValueType() != typeof(dummy) && configurationItemDefinition != ENABLED)
                {
                    if (Config.GetValue(configurationItemDefinition).GetType() == typeof(bool))
                    {
                        bool value = (bool)Config.GetValue(configurationItemDefinition);
                        if (value == true)
                        {
                            BadgesListNames.Add(configurationItemDefinition.Name);
                        }

                    }
                }
            }
            Harmony harmony = new Harmony("com.zahndy.FasterBadges");
            Avatars = new HashSet<AvatarManager>();
            harmony.PatchAll();
        }
        private static (Uri, ModConfigurationKey<bool>, bool) BadgesSwitch(string Name)
        {
            Uri url = null;
            ModConfigurationKey<bool> changedvar = null;
            bool skip = false;
            switch (Name)
            {
                case "Under18": 
                    changedvar = Under18;
                    url = new Uri("resdb:///030a337b2f1038c4e833dbef2c53bea30e47117c22ec5b01e65cb59c7e76380b.png");
                    break;
                case "Over18":
                    changedvar = Over18;
                    url = new Uri("resdb:///874e0c62cf6a8bda5a65ffe7518617e5742339d0c362d5717e8dc6d7e05c5eac.png");
                    break;
                case "Minor":
                    changedvar = Minor;
                    url = new Uri("resdb:///9dff86e3142f439ee273c57a67e5706ec20d60c5e5179dd59f238c8a24e6c923.png");
                    break;
                case "Adult":
                    changedvar = Adult;
                    url = new Uri("resdb:///bf80832420136d2d1029011dd2295a3871c97e4624b90e3d628a935f0301f087.png");
                    break;
                case "VeryOld":
                    changedvar = VeryOld;
                    url = new Uri("resdb:///830b795065d5ea8f0458f7390c8b0bac4b0df6f453bcec539fd08f46ab99e88b.png");
                    break;
                case "Fossil":
                    changedvar = Fossil;
                    url = new Uri("resdb:///6815fa0f9656d94cf108054331c4fff47904426eac29044dc71406dba1085c37.png"); 
                    break;
                case "Avali":
                    changedvar = Avali;
                    url = new Uri("resdb:///6548f96f2b16bbeb8538dddb7c5c94ff2645823de54cf85b37da97e6b9a8f5c8.png");
                    break;
                case "ADHD":
                    changedvar = ADHD;
                    url = new Uri("resdb:///3b57b6ce48b8d1fbe295942ab4883d830d039faaba6be2251d32baebbbbfc71c.png");
                    break;
                case "ADHDFlag":
                    changedvar = ADHDFlag;
                    url = new Uri("resdb:///5c24a24f980300d9066c2eafdf2d57d328404f7bebe37472365a19ec6d2a77f6.png");
                    break;
                case "NOLewd":
                    changedvar = NOLewd;
                    url = new Uri("resdb:///c6f7561c0f5b0ca7d986c23a36d0af1681e2779ae1c1e1db9e98b10866345fbf.png"); 
                    break;
                case "STOPhantom":
                    changedvar = STOPhantom;
                    url = new Uri("resdb:///d84873aa4025c12a26b10d51d86bde48caa4a2b8f7c4eba96fff6c324c8ba5cd.png");
                    break;
                case "PhantomCircle":
                    changedvar = PhantomCircle;
                    url = new Uri("resdb:///8c8066dc639d9235f0de37a66e90f647534dd670e2166eaf6db2e8753a049266");
                    break;
                case "USFN":
                    changedvar = USFN;
                    url = new Uri("resdb:///7886e38f5d36f41d7ee3fdcbd520867bbe737a2bfb6cbdde2d0af9c0d20d1d3d.png");
                    break;
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
                case "AF":
                    changedvar = AF;
                    url = new Uri("resdb:///f9f1c2142d6cce45f38729480a3a457f70774f48fcb915cf9e7db59f9dee5d54.png");
                    break;
                case "AR":
                    changedvar = AR;
                    url = new Uri("resdb:///f110617fe3fc8fc26372ef0ee88f469a4d02186ab1cc42b51fda1261d8ae03b2.png");
                    break;
                case "BN":
                    changedvar = BN;
                    url = new Uri("resdb:///8f1d00595cd2722cda4bfb14eaf632033cf914d9af9709a321c66a8b04312d84.png");
                    break;
                case "DA":
                    changedvar = DA;
                    url = new Uri("resdb:///0f4497240ebac60bf8553a08766a9657660387387da5d0db378316102ec23029.png");
                    break;
                case "DE":
                    changedvar = DE;
                    url = new Uri("resdb:///9f8a177ae178a8ac426b97ea0e58ede1143ca32ef3a2a05aef1e2f14e7bd7080.png");
                    break;
                case "EN":
                    changedvar = EN;
                    url = new Uri("resdb:///ec1225cb99da4205f5f37dcabd72bc0ae7ebc2b8e706a9817d236f8ef18a90bd.png");
                    break;
                case "ES":
                    changedvar = ES;
                    url = new Uri("resdb:///b241f3e7ed8bfc158bd2ba67fd91c33dd710e8ef504e844b41ccb9fc68d29520.png");
                    break;
                case "FI":
                    changedvar = FI;
                    url = new Uri("resdb:///b5ba76422dec60db608f68ad7d40af8583e3e59653889707410b0acaef729d4f.png");
                    break;
                case "FR":
                    changedvar = FR;
                    url = new Uri("resdb:///c6b29e2ec07370deca43067d3a928b8dd6a4b4d7a50f97106becae49869fb267.png");
                    break;
                case "HI":
                    changedvar = HI;
                    url = new Uri("resdb:///e91793ee1cdfa821ead79bb72eb4c00e5e1658fdcc35ac705f4c26d28496fdb6.png");
                    break;
                case "HU":
                    changedvar = HU;
                    url = new Uri("resdb:///0c4113b6ba52d09b1882a510d3634c38a9c88bb3d42caf7e3050b26c0a52ca5e.png");
                    break;
                case "IT":
                    changedvar = IT;
                    url = new Uri("resdb:///9e46fd023008e6e7cee2b2217ffe325c92604f72730eacbb63ca46b956cc73aa.png");
                    break;
                case "JA":
                    changedvar = JA;
                    url = new Uri("resdb:///e520e3d7c0ee42d353f79607614db651643294ea6af750fba9ac4a860b268268.png");
                    break;
                case "KO":
                    changedvar = KO;
                    url = new Uri("resdb:///0c5d8c10070c89b26aabdb2c15e9976de4658ce8cdac9187c9becb029af3bcd1.png");
                    break;
                case "LT":
                    changedvar = LT;
                    url = new Uri("resdb:///c49ba42d38d6e9e210acd44fd8be07428ab84bf42374ec533cbc4eeaf262d20c.png");
                    break;
                case "MR":
                    changedvar = MR;
                    url = new Uri("resdb:///8f78afb4be2944336aa7dc2a066576d12df72a4c8929af9ca46187cc7464d446.png");
                    break;
                case "NL":
                    changedvar = NL;
                    url = new Uri("resdb:///3a4b56f165aad42de75432680fae47dd06dd2a35d79d249498e828c21f0a9293.png");
                    break;
                case "NO":
                    changedvar = NO;
                    url = new Uri("resdb:///ef322f0e9dd3352809739c280e70ae65ae342332a93c65586f770f94814f418c.png");
                    break;
                case "PL":
                    changedvar = PL;
                    url = new Uri("resdb:///f6f37a9a7823e1fc4057a9673f244b68ca36455c45cb5433d9a8d64cb75a3db7.png");
                    break;
                case "PT":
                    changedvar = PT;
                    url = new Uri("resdb:///56ec0c6846bf0484615ddf8b4fe1e38038255b71b8e9e17270a1b7a590cac868.png");
                    break;
                case "RU":
                    changedvar = RU;
                    url = new Uri("resdb:///0ee18103333617e52e4b5607a1631c5655c38a3a9f632737bf0ac2d52842fabd.png");
                    break;
                case "SV":
                    changedvar = SV;
                    url = new Uri("resdb:///d7dd94e50e366757491fd2f695f5fb45061925e6ac59636a957b3a85c7ead6f5.png");
                    break;
                case "ZH":
                    changedvar = ZH;
                    url = new Uri("resdb:///5a59c0ac93743f931e93b3889736fd776a2122744bf4dbed3a8b343f04f2974b.png");
                    break;
                default:
                    break;
            }
            return (url,changedvar, skip);
        }
        private void OnThisConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
            if (configurationChangedEvent.Key == ENABLED) 
            {
                if (Config.GetValue(ENABLED))
                {
                    Msg(" --- ENABLED --- ");
                    CleanBadges();
                    foreach (string badge in BadgesListNames) 
                    {
                        var BadgeData = BadgesSwitch(badge);
                        UpdateBadges(badge, BadgeData.Item1, BadgeData.Item2, BadgeData.Item3);
                    }
                    List<String> CustomBadgesList = Config.GetValue(CustomBadges).Trim(',').Split(',').ToList();
                    if (CustomBadgesList.Count > 0)
                    {
                        foreach (String customBadge in CustomBadgesList)
                        {
                            if (customBadge.Length > 10)
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
                    Msg(" --- DISABLED --- ");
                    CleanBadges();
                }               
            }
            else if(configurationChangedEvent.Key == CustomBadges) //custom string has updated
            {
                
                Avatars.ElementAt(0).Slot.RunSynchronously(delegate 
                { 
                    CleanBadges();
                    if(CustomBadges != null) 
                    { 
                        string newstr = Config.GetValue(CustomBadges).Trim(',', ' ');
                        List<String> newList = newstr.Split(',').ToList();
                        foreach (AvatarManager avatarManager in Avatars)
                        {
                            avatarManager.RunSynchronously(delegate
                            {
                                Msg(" --- Re-Adding Custom Badges --- ");
                                if (newList.Count > 0)
                                {
                                    foreach (String customBadge in newList)
                                    {
                                        if (customBadge.Length > 10)
                                        {
                                            Uri lurl = new Uri(customBadge);
                                            avatarManager.AddIconBadge(lurl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                        }
                                    }
                                }                            
                            });
                        }
                    }
                    Msg(" --- Re-Adding Badges --- ");
                    foreach (string badge in BadgesListNames)
                    {
                        var BadgeData = BadgesSwitch(badge);
                        UpdateBadges(badge, BadgeData.Item1, BadgeData.Item2, BadgeData.Item3);
                    }
                });            
            }
            else
            {
                String badgeName = configurationChangedEvent.Key.Name;
                var BadgeData = BadgesSwitch(badgeName);
                UpdateBadges(badgeName, BadgeData.Item1, BadgeData.Item2, BadgeData.Item3);
            }
        }
        private static void UpdateBadges(string _badgeName,Uri url, ModConfigurationKey<bool> changedvar, bool skip)
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
                            av.UpdateBadges();
                        });
                    }
                }
            }
            else //CustomBadges csv
            {
                foreach (AvatarManager av in Avatars)
                {
                    av.Slot.RunSynchronously(delegate
                    {
                        if (CustomBadges != null)
                        {
                            List<String> CustomBadgesList = Config.GetValue(CustomBadges).Trim(',').Split(',').ToList();
                            foreach (String customBadge in CustomBadgesList)
                            {
                                av.BadgeTemplates.FindChild("Extra Badge-", true, true, 1).Destroy();
                            }
                            foreach (String customBadge in CustomBadgesList)
                            {
                                if (customBadge.Length > 10)
                                {
                                    Uri burl = new Uri(customBadge);
                                    av.AddIconBadge(burl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                }
                            }
                            av.UpdateBadges();
                        }
                    });

                }
            }           
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
                        foreach (string child in BadgesListNames)
                        {
                            av.BadgeTemplates.FindChild("Extra ", true, true, 1).Destroy(); 
                        }
                        if (CustomBadges != null)
                        {
                            String[] badges = Config.GetValue(CustomBadges).Trim(',').Split(',');
                            foreach (String customBadge in badges)
                            {
                                av.BadgeTemplates.FindChild("Extra ", true, true, 1).Destroy();
                            }
                        }
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
                            if ((!String.IsNullOrEmpty(CustomBadges.ToString()) && CustomBadges != null) || BadgesListNames.Count > 0)
                            {                               
                                UserRoot userRoot = user.Root;
                                AvatarManager avatarManager = userRoot.Slot.GetComponent<AvatarManager>();
                                if (avatarManager != null)
                                {
                                    if (!Avatars.Contains(avatarManager))
                                    {
                                        avatarManager.RunSynchronously(delegate
                                        {
                                            avatarManager.Disposing += (field) => { Avatars.Remove(avatarManager); };
                                            Avatars.Add(avatarManager);
                                            Msg(" --- Adding Custom Badges --- ");
                                            String[] badges = Config.GetValue(CustomBadges).Trim(',').Split(',');
                                            if (badges.Count() > 0 && badges[0].Length > 10)
                                            {
                                                foreach (String customBadge in badges)
                                                {
                                                    if (customBadge.Length > 10)
                                                    {
                                                        Uri lurl = new Uri(customBadge);
                                                        avatarManager.AddIconBadge(lurl, "Extra Badge-" + customBadge.Substring(customBadge.Length - 10, 5), blendMode, tint, TextureFilterMode.Bilinear, maxSize);
                                                    }
                                                }
                                            }
                                            foreach (string badge in BadgesListNames)
                                            {
                                                var badgeData = BadgesSwitch(badge);
                                                UpdateBadges(badge, badgeData.Item1, badgeData.Item2, badgeData.Item3);
                                            }
                                        });
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
        }
    }
}
