using System;
using System.Collections;
using Socknautica.Mono.Creatures;

namespace Socknautica.Mono;

internal class ArenaExplosion : MonoBehaviour
{
    //private GameObject sphere;
    private float sphereScale = 0f;
    private Light light;
    private float lightScale = 5f;

    public bool started;

    public static ArenaExplosion main;

    public static void Explode()
    {
        main = new GameObject("ArenaExplosion").AddComponent<ArenaExplosion>();
    }

    private IEnumerator Start()
    {
        //sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Destroy(sphere.GetComponent<Collider>());
        //sphere.GetComponent<Renderer>().material = MaterialUtils.IonCubeMaterial;
        //sphere.transform.position = ArenaSpawner.main.center.position;

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
        SaveWithoutSaving.OnDefeatBoss();
        for (int i = 0; i < 10; i++)
        {
            SpawnExplosion(ArenaSpawner.main.center.position + Random.insideUnitSphere * 20, Vector3.up * (i * 36));
        }
        FadingOverlay.PlayFX(Color.green, 0, 0.1f, 0.5f);
        yield return null;
        KillBoss();
        KillAll();
        ArenaSpawner.main.KnockOverLights();
        MainCameraControl.main.ShakeCamera(5f, 5f);
        LoopingMusic.StopCurrent();
        ArenaSpawner.main.arena.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(10f);
        //Destroy(sphere);
        Destroy(gameObject);
        started = false;
        SpawnPortal();
    }

    private void SpawnExplosion(Vector3 pos, Vector3 angle)
    {
        var destruction = ArenaSpawner.cyclopsObj.GetComponent<CyclopsDestructionEvent>();
        var spawned = Instantiate(destruction.fxControl.gameObject);
        spawned.transform.localScale *= 50;
        spawned.transform.position = pos;
        spawned.transform.eulerAngles = angle;
        var vfx = spawned.GetComponent<VFXController>();
        vfx.Play(1);
        vfx.emitters[1].fxPS.transform.localScale *= 10;
        foreach (var system in vfx.emitters[1].fxPS.GetComponentsInChildren<ParticleSystem>(true))
        {
            var main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }

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
        var energyOrbs = Object.FindObjectsOfType<EnergyBallDamageInRange>();
        foreach (var o in energyOrbs)
        {
            if (o != null)
            {
                Destroy(o.gameObject);
            }
        }
    }

    private void SpawnPortal()
    {
        SeaMoth seamoth = CraftData.GetPrefabForTechType(TechType.Seamoth).GetComponent<SeaMoth>();
        var vfx = Instantiate(seamoth.torpedoTypes[0].prefab.GetComponent<SeamothTorpedo>().explosionPrefab.GetComponent<PrefabSpawn>().prefab);
        var electricity = Helpers.SpawnPrecursorElectricSparks();
        electricity.transform.parent = vfx.transform;
        electricity.transform.localPosition = Vector3.zero;
        electricity.transform.localScale = Vector3.one * 2f;
        Helpers.MakeParticleSystemLoopable(vfx);
        Helpers.MakeParticleSystemScaleable(vfx);
        vfx.transform.localScale = Vector3.one * 15f;
        vfx.transform.position = ArenaSpawner.main.center.transform.position;
        vfx.gameObject.AddComponent<Mono.Alien.BeatGameOnTouch>();
    }

    private void KillBoss()
    {
        var boss = Boss.main;
        if (boss)
        {
            Utils.PlayFMODAsset(Helpers.GetFmodAsset("BossDeath"), Player.main.transform.position);
            boss.GetComponent<LiveMixin>().Kill();
            boss.gameObject.SetActive(false);
        }
        BossCorpse.SpawnBossCorpse(Boss.main.transform.position);
    }

    private void Update()
    {
        if (!started) return;
        light.range = lightScale;
        //sphere.transform.localScale = Vector3.one * sphereScale;
        sphereScale += 200f * Time.deltaTime;
        lightScale += 90 * Time.deltaTime;
    }
}
