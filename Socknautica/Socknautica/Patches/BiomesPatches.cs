using ECCLibrary;
using HarmonyLib;

namespace Socknautica.Patches;

internal class BiomesPatches
{
    public static void Patch()
    {
        var h = Main.harmony;
        var waterAmbienceStartOrig = AccessTools.Method(typeof(WaterAmbience), nameof(WaterAmbience.Start));
        var waterAmbienceStartPostfix = new HarmonyMethod(AccessTools.Method(typeof(BiomesPatches), nameof(BiomesPatches.WaterAmbience_Start_Postfix)));
        h.Patch(waterAmbienceStartOrig, null, waterAmbienceStartPostfix);

        var waterBiomeManagerStartOrig = AccessTools.Method(typeof(WaterBiomeManager), nameof(WaterBiomeManager.Start));
        var waterBiomeManagerStartPostfix = new HarmonyMethod(AccessTools.Method(typeof(BiomesPatches), nameof(BiomesPatches.WaterBiomeManager_Start_Postfix)));
        h.Patch(waterBiomeManagerStartOrig, null, waterBiomeManagerStartPostfix);

        var getBiomeIndexOrig = AccessTools.Method(typeof(WaterBiomeManager), nameof(WaterBiomeManager.GetBiomeIndex));
        var getBiomeIndexPostfix = new HarmonyMethod(AccessTools.Method(typeof(BiomesPatches), nameof(BiomesPatches.WaterBiomeManager_GetBiomeIndex_Postfix)));
        h.Patch(getBiomeIndexOrig, null, getBiomeIndexPostfix);

    }

    private const string voidBiomeName = "void";

    private static int voidBiomeIndex = -1;
        
    private static readonly WaterscapeVolume.Settings precursorBaseWaterscapeSettings = new ()
    {
        absorption = Vector3.zero,
        ambientScale = 0f,
        emissiveScale = 0f,
        sunlightScale = 0f,
        murkiness = 0.001f,
        startDistance = 25f,
        scatteringColor = Color.white,
        temperature = 34f,
        scattering = 0.0001f,
    };

    public static void WaterAmbience_Start_Postfix(WaterAmbience __instance)
    {
        PatchBiomeSounds(__instance.gameObject, voidBiomeName, "arcticAmbience", "dunes");
        //PatchBiomeSounds(__instance.gameObject, "PrecursorSecretBase", "WreckAmbience", "WreckAmbience");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaterBiomeManager), nameof(WaterBiomeManager.Start))]
    public static void WaterBiomeManager_Start_Postfix(WaterBiomeManager __instance)
    {
        if (__instance.biomeSkies.Count < 3) //a check to see if the main menu water biome manager is loaded, rather than the main one. if we don't end the method here, the game will throw an exception.
        {
            return;
        }
        var voidWaterscapeSettings = new WaterscapeVolume.Settings()
        {
            absorption = new Vector3(40, 15f, 9f) / 5f,
            ambientScale = 0.5f,
            emissiveScale = 0f,
            sunlightScale = 1.1f,
            murkiness = 0.82f,
            startDistance = 100f,
            scatteringColor = new Color(0.3f, 0.3f, 0.3f),
            temperature = 5f,
            scattering = 0.25f
        };
            
        PatchBiomeFog(__instance, voidBiomeName, voidWaterscapeSettings, __instance.biomeSkies[37]);

        PatchPrecursorBiomesFog(__instance, "PrecursorSmallCache", "PrecursorVoidBase", "PrecursorVoidBaseHallway", "PrecursorSecretBase");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaterBiomeManager), nameof(WaterBiomeManager.GetBiomeIndex))]
    public static void WaterBiomeManager_GetBiomeIndex_Postfix(ref int __result)
    {
        if (__result == -1 || Mono.ArenaSpawner.main != null)
        {
            __result = voidBiomeIndex;
        }
    }

    private static void PatchBiomeSounds(GameObject waterAmbienceParent, string biomeName, string ambienceReference, string musicReference)
    {
        var ambienceParent = GameObjectExtensions.SearchChild(waterAmbienceParent, "background");
        var biomeAmbience = Object.Instantiate(GameObjectExtensions.SearchChild(ambienceParent, ambienceReference), ambienceParent.transform);
        biomeAmbience.name = $"{biomeName}Ambience";
        biomeAmbience.GetComponent<FMODGameParams>().onlyInBiome = biomeName;

        var musicParent = GameObjectExtensions.SearchChild(waterAmbienceParent, "music");
        var referenceMusic = Object.Instantiate(GameObjectExtensions.SearchChild(musicParent, musicReference), musicParent.transform);
        referenceMusic.name = $"{biomeName}Ambience";
        referenceMusic.GetComponent<FMODGameParams>().onlyInBiome = biomeName;
    }

    private static void PatchPrecursorBiomesFog(WaterBiomeManager waterBiomeManager, params string[] biomeNames)
    {
        foreach (var biomeName in biomeNames)
        {
            if (string.IsNullOrEmpty(biomeName))
                return;
                
            PatchBiomeFog(waterBiomeManager, biomeName, precursorBaseWaterscapeSettings, waterBiomeManager.biomeSkies[131]);
        }
    }

    private static void PatchBiomeFog(WaterBiomeManager waterBiomeManager, string biomeName, WaterscapeVolume.Settings waterScapeSettings, mset.Sky sky)
    {
        if (!waterBiomeManager.biomeLookup.ContainsKey(biomeName))
        {
            GameObject skyPrefab = null;
            if (sky)
            {
                skyPrefab = sky.gameObject;
            }
            WaterBiomeManager.BiomeSettings biomeSettings = new WaterBiomeManager.BiomeSettings()
            {
                name = biomeName,
                skyPrefab = skyPrefab,
                settings = waterScapeSettings
            };
            waterBiomeManager.biomeSkies.Add(sky);
            waterBiomeManager.biomeSettings.Add(biomeSettings);
            var biomeIndex = waterBiomeManager.biomeSettings.Count - 1;
            if (biomeName == voidBiomeName)
                voidBiomeIndex = biomeIndex;
                
            waterBiomeManager.biomeLookup.Add(biomeName.ToLower(), biomeIndex);
        }
    }
}