using ECCLibrary;

namespace Socknautica
{
    [QModCore]
    public partial class Main
    {
        internal static AssetBundle assetBundle;
        internal static Assembly assembly = Assembly.GetExecutingAssembly();
        internal static Harmony harmony;

        internal static Config config = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPrePatch]
        public static void Prepatch()
        {
            assetBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(assembly, "socknautica");
            harmony = new Harmony("Socksfor1.Socknautica");
            harmony.PatchAll(assembly);
            PatchPrefabsEarly();
        }

        [QModPatch]
        public static void Patch()
        {
            PatchFMODAudio();
            PatchLanguageLines();
            MaterialUtils.LoadMaterials();
            if (!OtherMods.RotAExists)
            {
                Patches.BiomesPatches.Patch();
            }
        }

        [QModPostPatch]
        public static void PostPatch()
        {
            PatchPrefabs();
            if (OtherMods.SubmarineModExists)
            {
                PatchSubRecipes();
            }
        }

        private static void PatchLanguageLines()
        {
            LanguageHandler.SetLanguageLine("SocksSignalSubtitles", "Detecting an Alien broadcast. Uploading coordinates to PDA.");
            LanguageHandler.SetLanguageLine("RocketSelfDestruct1", "You have chosen not to come home to Alterra.");
            LanguageHandler.SetLanguageLine("RocketSelfDestruct2", "Detecting multiple energy signatures originating from the ecological dead zone. Uploading coordinates to your PDA. Exploration is conducted at your own risk.");
        }

        private static void PatchSubRecipes()
        {
            if (TechTypeHandler.TryGetModdedTechType("DadSub", out var dadSub))
            {
                var newDadRecipe = new TechData(new Ingredient(TechType.TitaniumIngot, 4), new Ingredient(TechType.Magnetite, 2), new Ingredient(TechType.ComputerChip, 2), new Ingredient(pressuriumCrystal.TechType, 6));
                CraftDataHandler.SetTechData(dadSub, newDadRecipe);
            }
            if (TechTypeHandler.TryGetModdedTechType("SockTank", out var sockTank))
            {
                var newSockTankRecipe = new TechData(new Ingredient(TechType.PlasteelIngot, 2), new Ingredient(TechType.EnameledGlass, 2), new Ingredient(TechType.AdvancedWiringKit, 1), new Ingredient(TechType.SeamothTorpedoModule, 1), new Ingredient(pressuriumCrystal.TechType, 10), new Ingredient(atmospheriumCrystal.TechType, 5));
                CraftDataHandler.SetTechData(sockTank, newSockTankRecipe);
            }
        }

        public static Atlas.Sprite LoadSprite(string assetName)
        {
            return new Atlas.Sprite(assetBundle.LoadAsset<Sprite>(assetName));
        }
    }
}