namespace Socknautica.Mono;

internal class MainMenuReaperController : MonoBehaviour
{
    private GameObject reaperPrefab;

    private float timeSpawnAgain;

    private mset.Sky sky;

    private void Update()
    {
        if (reaperPrefab == null)
        {
            GetPrefab();
        }
        if (sky == null)
        {
            var marmoSkies = MarmoSkies.main;
            if (marmoSkies)
            {
                var ss = marmoSkies.transform.GetChild(0);
                if (ss != null) sky = ss.GetComponent<mset.Sky>();
            }
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
            if (position.z > 26 && position.z < 43) return;
            var reaper = Instantiate(reaperPrefab, position + Vector3.down * 12f, Quaternion.identity);
            reaper.transform.localEulerAngles = new Vector3(-90, Random.value * 360f, 0);
            reaper.SetActive(true);
            Helpers.RemoveNonEssentialComponents(reaper);
            timeSpawnAgain = Time.time + Random.Range(0.8f, 2f);
            reaper.AddComponent<MainMenuReaper>();
            var sa = reaper.EnsureComponent<SkyApplier>();
            sa.SetSky(Skies.Custom);
            if (sky != null) sa.SetCustomSky(sky);
            var renderer = reaper.GetComponentInChildren<Renderer>();
            var materials = renderer.materials;
            foreach (var m in materials)
            {
                m.SetFloat("_EmissiveLM", 0.3f);
                m.SetFloat("_EmissiveLMNight", 0.3f);
            }
            renderer.materials = materials;
        }
    }

    private void GetPrefab()
    {
        UWE.PrefabDatabase.TryGetPrefab("f78942c3-87e7-4015-865a-5ae4d8bd9dcb", out reaperPrefab);
    }
}
