using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(VoidGhostLeviathansSpawner))]
internal class VoidGhostLeviathansSpawnerPatches
{
    [HarmonyPatch(nameof(VoidGhostLeviathansSpawner.IsVoidBiome))]
    [HarmonyPostfix()]
    public static void IsVoidBiomePostfix(ref bool __result)
    {
        if (__result == true)
        {
            if (VoidIslandBiome.bounds.Contains(Player.main.transform.position)) __result = false;
        }
    }
}