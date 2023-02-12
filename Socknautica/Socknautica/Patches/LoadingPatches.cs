using Socknautica.Mono;
using UnityEngine.Video;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(uGUI_SceneLoading))]
internal class LoadingPatches
{
    [HarmonyPatch(nameof(uGUI_SceneLoading.Init))]
    [HarmonyPostfix()]
    public static void Postfix(uGUI_SceneLoading __instance)
    {
        if (__instance.gameObject.GetComponentInChildren<VideoPlayer>() != null) return;
        var loadingScreen = Object.Instantiate(Main.loadingScreensBundle.LoadAsset<GameObject>("CustomLoadingScreenCanvas"));
        var originalArtwork = __instance.gameObject.Search("LoadingArtwork");
        var video = loadingScreen.transform.GetChild(0);
        video.parent = originalArtwork.transform.parent;
        video.SetSiblingIndex(originalArtwork.transform.GetSiblingIndex() + 1);
        video.transform.localPosition = Vector3.zero;
        video.transform.localScale = Vector3.one;
        video.transform.localEulerAngles = Vector3.zero;
        video.gameObject.EnsureComponent<UnloadLoadingScreen>();
    }
}
