using ECCLibrary;

namespace Socknautica
{
    [QModCore]
    public partial class Main
    {
        internal static AssetBundle assetBundle;
        internal static Assembly assembly = Assembly.GetExecutingAssembly();
        internal static Harmony harmony;

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
            MaterialUtils.LoadMaterials();
            PatchPrefabs();
            PatchFMODAudio();
            PatchLanguageLines();
            if (!OtherMods.RotAExists)
            {
                Patches.BiomesPatches.Patch();
            }
        }

        [QModPostPatch]
        public static void PostPatch()
        {
            if (OtherMods.SubmarineModExists)
            {
                PatchSubRecipes();
            }
        }

        private static void PatchLanguageLines()
        {
            LanguageHandler.SetLanguageLine("SocksSignalSubtitles", "Detecting an Alien broadcast. Uploading coordinates to PDA.");
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
                var newSockTankRecipe = new TechData(new Ingredient(TechType.PlasteelIngot, 2),  new Ingredient(TechType.AdvancedWiringKit, 1), new Ingredient(TechType.SeamothTorpedoModule, 1), new Ingredient(pressuriumCrystal.TechType, 10));
                CraftDataHandler.SetTechData(sockTank, newSockTankRecipe);
            }
        }

        public static Atlas.Sprite LoadSprite(string assetName)
        {
            return new Atlas.Sprite(assetBundle.LoadAsset<Sprite>(assetName));
        }
    }
}