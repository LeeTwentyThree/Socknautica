using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(Creature))]
public static class Creature_Patches
{
    [HarmonyPatch(nameof(Creature.Start))]
    [HarmonyPostfix()]
    public static void Start_Postfix(Creature __instance)
    {
        if (!(__instance is ReaperLeviathan))
        {
            return;
        }
        if (Main.config.MoreViciousReapers)
        {
            var melee = __instance.gameObject.GetComponentInChildren<MeleeAttack>();
            if (melee != null)
            {
                melee.canBiteVehicle = true;
            }
        }
    }
}

[HarmonyPatch(typeof(FleeOnDamage))]
public static class FleeOnDamage_Patches
{
    [HarmonyPatch(nameof(FleeOnDamage.StartPerform))]
    [HarmonyPrefix()]
    public static bool StartPerform_Prefix(FleeOnDamage __instance, Creature creature)
    {
        if (Main.config.DisableLeviathanFear == false)
        {
            return true;
        }
        if (IsCreatureLeviathan(creature))
        {
            __instance.timeNextSwim = float.MaxValue;
            return false;
        }
        return true;
    }

    private static bool IsCreatureLeviathan(Creature creature)
    {
        if (creature.liveMixin != null && creature.liveMixin.maxHealth >= 3000)
        {
            return true;
        }
        return false;
    }
}

[HarmonyPatch(typeof(uGUI_RadiationWarning))]
public static class uGUI_RadiationWarning_Patches
{
    [HarmonyPatch(nameof(uGUI_RadiationWarning.Update))]
    [HarmonyPrefix()]
    public static void Update(uGUI_RadiationWarning __instance)
    {
        if (WarningUI.prefabObject == null)
        {
            WarningUI.prefabObject = __instance.gameObject;
        }
    }
}

public static class LiveMixin_Patches
{
    [HarmonyPatch(nameof(LiveMixin.Kill))]
    [HarmonyPostfix()]
    public static void Kill(LiveMixin __instance)
    {
        KilledLeviathanTracker.OnKill(__instance.gameObject);
    }
}

[HarmonyPatch(typeof(PrecursorTeleporter))]
public static class DeactivateExitPortal
{
    [HarmonyPatch(nameof(PrecursorTeleporter.InitializeDoor))]
    [HarmonyPrefix()]
    public static bool InitializeDoor(PrecursorTeleporter __instance)
    {
        if (ShouldDisable(__instance))
        {
            if (__instance.portalFxControl != null)
            {
                __instance.portalFxControl.Toggle(false);
            }
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(PrecursorTeleporter.ToggleDoor))]
    [HarmonyPrefix()]
    public static bool ToggleDoor(PrecursorTeleporter __instance)
    {
        if (ShouldDisable(__instance))
        {
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(PrecursorTeleporter.OnActivateTeleporter))]
    [HarmonyPrefix()]
    public static bool OnActivateTeleporter(PrecursorTeleporter __instance)
    {
        if (ShouldDisable(__instance))
        {
            return false;
        }
        return true;
    }

    private static bool ShouldDisable(PrecursorTeleporter teleporter)
    {
        return teleporter.teleporterIdentifier == "ArenaTeleporter" && Mathf.Approximately(teleporter.warpToPos.x, 647f);
    }
}