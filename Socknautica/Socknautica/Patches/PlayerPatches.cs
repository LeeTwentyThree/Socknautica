using UnityEngine.SceneManagement;

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
        __instance.gameObject.AddComponent<UnlockTrophyOnGameStart>();
    }

    [HarmonyPatch(nameof(Player.OnKill))]
    [HarmonyPostfix()]
    public static void PlayerOnKill(Player __instance)
    {
        if (ArenaSpawner.main != null)
        {
            SceneManager.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
        }
    }
}