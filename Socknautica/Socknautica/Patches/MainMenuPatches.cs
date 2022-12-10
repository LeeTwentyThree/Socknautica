using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(uGUI_MainMenu))]
internal class MainMenuPatches
{
    [HarmonyPatch(nameof(uGUI_MainMenu.Awake))]
    [HarmonyPostfix()]
    public static void uGUI_MainMenu_Awake_Postfix()
    {
        var billboard = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("MainMenuBillboardPrefab"));
        billboard.transform.position = new Vector3(-12.50f, -10.5f, 26);
        billboard.transform.localScale = Vector3.one * 3.4f;
    }
}
