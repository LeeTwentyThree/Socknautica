using System.Collections;
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
            UWE.CoroutineHost.StartCoroutine(KillCoroutine());
        }
    }

    private static IEnumerator KillCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
    }
}