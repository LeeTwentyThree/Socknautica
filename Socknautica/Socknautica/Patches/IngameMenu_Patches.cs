using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(IngameMenu))]
public class IngameMenu_Patches
{
    [HarmonyPatch(nameof(IngameMenu.GetAllowSaving))]
    [HarmonyPostfix]
    public static void GetAllowSavingPostfix(ref bool __result)
    {
        if (ArenaSpawner.main != null)
        {
            __result = false;
        }
    }
}