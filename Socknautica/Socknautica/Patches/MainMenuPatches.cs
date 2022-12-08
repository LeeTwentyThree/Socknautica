using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
internal class MainMenuPatches
{
    [HarmonyPatch(nameof(uGUI_MainMenu.Awake))]
    [HarmonyPostfix()]
    public static void uGUI_MainMenu_Awake_Postfix(uGUI_MainMenu __instance)
    {
        var billboard = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("MainMenuBillboardPrefab"));
        billboard.transform.position = new Vector3(-11.50f, -9.00f, 26.00f);
        billboard.transform.localScale = Vector3.one * 2.8f;
    }
}
