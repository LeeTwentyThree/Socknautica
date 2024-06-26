﻿using Socknautica.Mono;

namespace Socknautica.Patches;

[HarmonyPatch(typeof(Rocket))]
internal class RocketPatches
{
    [HarmonyPatch(nameof(Rocket.Start))]
    [HarmonyPostfix()]
    public static void RocketStartPostfix(Rocket __instance)
    {
        var stage3 = __instance.transform.Find("Stage03");
        var button = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("SelfDestructButton"));
        button.AddComponent<SelfDestructButton>();
        MaterialUtils.ApplySNShaders(button);
        button.transform.parent = stage3;
        button.transform.localPosition = new Vector3(0, 35.7f, -1.33f);
        button.transform.localEulerAngles = Vector3.right * -90;
        button.transform.localScale = Vector3.one * .25f;
    }
}
[HarmonyPatch(typeof(LaunchRocket))]
internal class LaunchRocketPatches
{
    [HarmonyPatch(nameof(LaunchRocket.OnHandClick))]
    [HarmonyPrefix()]
    public static bool OnHandClickPrefix(LaunchRocket __instance)
    {
        if (LaunchRocket.launchStarted)
        {
            return true;
        }
        if (SaveWithoutSaving.GetBossDefeated())
        {
            return true;
        }
        LaunchRocket.SetLaunchStarted();
        HandReticle.main.RequestCrosshairHide();
        MultigargRocketCinematicController.PlayCinematic(__instance.GetComponentInParent<Rocket>());
        return false;
    }
}