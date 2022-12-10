using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(Player))]
internal class PlayerPatches
{
    [HarmonyPatch(nameof(Player.Start))]
    [HarmonyPostfix()]
    public static void PlayerStartPostfix(Player __instance)
    {
        __instance.gameObject.AddComponent<IslandGenerator>();
        __instance.gameObject.AddComponent<InitiateArenaSpawnInRange>();
    }
}
