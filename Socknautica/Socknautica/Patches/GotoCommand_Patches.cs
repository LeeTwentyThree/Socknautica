using System.Collections;
using System.Collections.Generic;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(GotoConsoleCommand))]
public static class GotoCommand_Patches
{
    [HarmonyPatch(nameof(GotoConsoleCommand.Awake))]
    [HarmonyPostfix]
    public static void Awake_Patch(GotoConsoleCommand __instance)
    {
        AddTeleportPosition(ref __instance, "aquariumbase", new Vector3(1520, -1000, -1480));
        AddTeleportPosition(ref __instance, "coordbase", new Vector3(1700, -1891, -173));
        AddTeleportPosition(ref __instance, "arenateleporter", new Vector3(648, -2201, -1540));
    }

    private static void AddTeleportPosition(ref GotoConsoleCommand gotoCmd, string name, Vector3 pos, bool forceWalk = false)
    {
        var locations = new List<TeleportPosition>(gotoCmd.data.locations);
        locations.Add(new TeleportPosition() { name = name, position = pos });
        gotoCmd.data.locations = locations.ToArray();
    }
}