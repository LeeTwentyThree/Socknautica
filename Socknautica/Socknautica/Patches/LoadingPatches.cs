using Socknautica.Mono;
using UnityEngine.Video;
using ECCLibrary;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(uGUI_SceneLoading))]
internal class LoadingPatches
{
    public static AssetBundle loadingScreensBundle;

    [HarmonyPatch(nameof(uGUI_SceneLoading.Init))]
    [HarmonyPostfix()]
    public static void Postfix(uGUI_SceneLoading __instance)
    {
        if (loadingScreensBundle == null)
        {
            loadingScreensBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(Main.assembly, "socknauticaloadingscreen");
        }
        if (__instance.gameObject.GetComponentInChildren<VideoPlayer>() != null) return;
        var loadingScreen = Object.Instantiate(loadingScreensBundle.LoadAsset<GameObject>("CustomLoadingScreenCanvas"));
        var originalArtwork = __instance.gameObject.Search("LoadingArtwork");
        int videoToPlay = Random.Range(0, loadingScreen.transform.childCount);
        var video = loadingScreen.transform.GetChild(videoToPlay);
        video.parent = originalArtwork.transform.parent;
        video.SetSiblingIndex(originalArtwork.transform.GetSiblingIndex() + 1);
        video.transform.localPosition = Vector3.zero;
        video.transform.localScale = Vector3.one;
        video.transform.localEulerAngles = Vector3.zero;
        video.gameObject.EnsureComponent<UnloadLoadingScreen>();
        Object.Destroy(loadingScreen.gameObject);
    }
}
