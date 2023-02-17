﻿namespace Socknautica.Mono;

internal class MainMenuReaperController : MonoBehaviour
{
    private GameObject reaperPrefab;

    private float timeSpawnAgain;

    private void Update()
    {
        if (reaperPrefab == null)
        {
            GetPrefab();
        }
        if (reaperPrefab != null)
        {
            UpdateNormal();
        }
    }

    private void UpdateNormal()
    {
        if (Time.time > timeSpawnAgain)
        {
            var position = Camera.current.transform.position + Vector3.forward * 60f + Random.insideUnitSphere * 50f;
            position.y = 0;
            var reaper = Instantiate(reaperPrefab, position + Vector3.down * 12f, Quaternion.identity);
            reaper.transform.localEulerAngles = new Vector3(-90, Random.value * 360f, 0);
            reaper.SetActive(true);
            Helpers.RemoveNonEssentialComponents(reaper);
            timeSpawnAgain = Time.time + Random.Range(0.8f, 2f);
            reaper.AddComponent<MainMenuReaper>();
            reaper.EnsureComponent<SkyApplier>().SetSky(Skies.BaseInterior);
        }
    }

    private void GetPrefab()
    {
        UWE.PrefabDatabase.TryGetPrefab("f78942c3-87e7-4015-865a-5ae4d8bd9dcb", out reaperPrefab);
    }
}
