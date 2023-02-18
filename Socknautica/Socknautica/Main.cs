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
            ECCAudio.RegisterClips(assetBundle);
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
            LanguageHandler.SetLanguageLine("UseBoomerangBoomerang", "Throw Boomerang ({0})");

            LanguageHandler.SetLanguageLine("Ency_SocknauticaLore1", "A letter from Alterra");
            LanguageHandler.SetLanguageLine("EncyDesc_SocknauticaLore1", "[AUTOMATED MESSAGE]\n\nWe understand that you have decided against returning home and escaping Planet 4546B. Such an act is understandable given your outrageous debt. While your decision is recognized and respected by Alterra Corporation, we have the obligation to warn you that this planet is not suitable for long term habitation.\n\nBiological scans show a plethora of leviathan-class lifeforms inhabiting an underwater archipelago in the southeastern area, likely having escaped from an abandoned laboratory in the vicinity. Exploration of the area is advised to ensure your own safety.");
        }

        private static void PatchSubRecipes()
        {
            if (TechTypeHandler.TryGetModdedTechType("DadSub", out var dadSub))
            {
                var newDadRecipe = new TechData(new Ingredient(TechType.TitaniumIngot, 2), new Ingredient(TechType.Magnetite, 2), new Ingredient(TechType.ComputerChip, 2), new Ingredient(pressuriumCrystal.TechType, 14));
                CraftDataHandler.SetTechData(dadSub, newDadRecipe);
            }
            if (TechTypeHandler.TryGetModdedTechType("SockTank", out var sockTank))
            {
                var newSockTankRecipe = new TechData(new Ingredient(TechType.PlasteelIngot, 2), new Ingredient(TechType.EnameledGlass, 2), new Ingredient(TechType.AdvancedWiringKit, 1), new Ingredient(TechType.SeamothTorpedoModule, 1), new Ingredient(pressuriumCrystal.TechType, 2), new Ingredient(atmospheriumCrystal.TechType, 10));
                CraftDataHandler.SetTechData(sockTank, newSockTankRecipe);
            }
        }

        public static Atlas.Sprite LoadSprite(string assetName)
        {
            return new Atlas.Sprite(assetBundle.LoadAsset<Sprite>(assetName));
        }
    }
}