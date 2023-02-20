using System;
using System.Collections;
using Socknautica.Mono.Creatures;

namespace Socknautica.Mono;

internal class ArenaExplosion : MonoBehaviour
{
    private GameObject sphere;
    private float sphereScale = 1000f;
    private Light light;
    private float lightScale = 5f;

    private bool started;

    public static void Explode()
    {
        new GameObject("ArenaExplosion").AddComponent<ArenaExplosion>();
    }

    private IEnumerator Start()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<Collider>());
        sphere.GetComponent<Renderer>().material = MaterialUtils.IonCubeMaterial;
        sphere.transform.position = ArenaSpawner.main.center.position;

        light = new GameObject("Light").AddComponent<Light>();
        light.range = 0f;
        light.intensity = 5f;
        light.color = Color.green;
        light.transform.position = ArenaSpawner.main.center.position;

        WarningUI.Show("IMMINENT EXPLOSION - AVOID REACTOR AT ALL COSTS", Main.assetBundle.LoadAsset<Sprite>("genericwarning"), 15f);
        for (int i = 0; i < 15; i++)
        {
            MainCameraControl.main.ShakeCamera(3f, 2f);
            yield return new WaitForSeconds(1f);
        }
        started = true;
        SpawnExplosion(ArenaSpawner.main.center.position);
        yield return new WaitForSeconds(5f);
        KillBoss();
        KillAll();
        FadingOverlay.PlayFX(Color.green, 0.1f, 2f, 0.1f);
        yield return new WaitForSeconds(3f);
        Destroy(sphere);
        Destroy(light);
        Destroy(gameObject);

    }

    private void SpawnExplosion(Vector3 pos)
    {
        var destruction = ArenaSpawner.cyclopsObj.GetComponent<CyclopsDestructionEvent>();
        var spawned = Instantiate(destruction.fxControl.gameObject);
        foreach (var system in spawned.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        spawned.transform.localScale *= 50;
        spawned.transform.position = pos;
        spawned.GetComponent<VFXController>().Play(1);
    }

    private void KillAll()
    {
        var creatures = Object.FindObjectsOfType<Creature>();
        foreach (var c in creatures)
        {
            if (c != null)
            {
                var lm = c.gameObject.GetComponent<LiveMixin>();
                if (lm)
                    lm.TakeDamage(99999);
            }
        }
    }

    private void SpawnPortal()
    {

    }

    private void KillBoss()
    {
        var boss = Boss.main;
        if (boss)
        {
            Utils.PlayFMODAsset(Helpers.GetFmodAsset("BossDeath"), boss.transform.position);
            boss.GetComponent<LiveMixin>().Kill();
            var rb = boss.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            boss.transform.localEulerAngles = new Vector3(180, 0, 0);
        }
    }

    private void Update()
    {
        if (!started) return;
        light.range = lightScale;
        sphere.transform.localScale = Vector3.one * sphereScale;
        sphereScale += 90 * Time.deltaTime;
        lightScale += 90 * Time.deltaTime;
    }
}
