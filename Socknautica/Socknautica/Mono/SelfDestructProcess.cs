﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWE;

namespace Socknautica.Mono;

internal class SelfDestructProcess : MonoBehaviour
{
    public Rocket rocket;

    private GameObject cyclopsObj;

    private float timeExplode = -1f;

    private WarningUI warning;

    public static void Begin(Rocket rocket)
    {
        new GameObject().AddComponent<SelfDestructProcess>().rocket = rocket;
    }

    private IEnumerator LoadCyclops()
    {
        yield return new WaitUntil(() => LightmappedPrefabs.main);
        LightmappedPrefabs.main.RequestScenePrefab("Cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
    }

    public void OnSubPrefabLoaded(GameObject prefab)
    {
        cyclopsObj = prefab;
    }

    private IEnumerator Start()
    {
        StartCoroutine(LoadCyclops());
        foreach (var light in rocket.GetComponentsInChildren<Light>())
        {
            // light.intensity *= 3f;
            // light.color = Color.red;
        }
        CustomPDALinesManager.PlayVoiceLine("RocketSelfDestruct1");
        warning = WarningUI.Show("SELF DESTRUCTION IMMINENT", Main.assetBundle.LoadAsset<Sprite>("genericwarning"), 30f);

        var explosionCenter = rocket.gameObject.transform.position + Vector3.up * 21 + Vector3.left * 15f;

        timeExplode = Time.time + 15f;

        yield return new WaitForSeconds(13.5f);

        for (int i = 0; i < 5; i++)
        {
            SpawnExplosion(explosionCenter + Vector3.down * 20 + Vector3.up * 10 * i);
        }

        MainCameraControl.main.ShakeCamera(1.1f, 3.7f, MainCameraControl.ShakeMode.Sqrt, 0.8f);

        yield return new WaitForSeconds(1f);

        FMODAsset sound = Helpers.GetFmodAsset("EnergyPylonExplosion");
        Utils.PlayFMODAsset(sound, Player.main.transform.position);

        yield return new WaitForSeconds(0.5f);
        foreach (var renderer in rocket.GetComponentsInChildren<Renderer>())
        {
            var rb = renderer.gameObject.EnsureComponent<Rigidbody>();
            if (rb == Player.main.rigidBody) continue;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.AddExplosionForce(3000f, explosionCenter, 100f, 0.3f);
            rb.AddTorque(Random.rotation * Vector3.forward * 100f, ForceMode.VelocityChange);
            rb.transform.parent = null;
            Destroy(rb.gameObject, 20f);
        }
        if (Player.main.IsInside() || Player.main.precursorOutOfWater)
        {
            Player.main.liveMixin.Kill(DamageType.Explosive);
        }
        Destroy(rocket.gameObject);
        MainCameraControl.main.ShakeCamera(5f, 1.5f, MainCameraControl.ShakeMode.Quadratic);
        if (warning) warning.Hide();
        CustomPDALinesManager.PlayVoiceLine("RocketSelfDestruct2");
        if (PrefabDatabase.TryGetPrefab("AquariumBaseSignal", out GameObject prefab))
        {
            GameObject pingObj = Instantiate(prefab);
            pingObj.SetActive(true);
            LargeWorld.main.streamer.cellManager.RegisterEntity(pingObj.GetComponent<LargeWorldEntity>());
        }
    }

    private void SpawnExplosion(Vector3 pos)
    {
        var destruction = cyclopsObj.GetComponent<CyclopsDestructionEvent>();
        var spawned = Instantiate(destruction.fxControl.gameObject);
        foreach (var system in spawned.GetComponentsInChildren<ParticleSystem>())
        {
            var main = system.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        spawned.transform.localScale *= 9;
        spawned.transform.position = pos;
        spawned.GetComponent<VFXController>().Play(1);
    }

    private void Update()
    {
        if (warning != null && timeExplode > 0)
        {
            warning.SetText("SELF DESTRUCTION IMMINENT: T-" + Mathf.RoundToInt(timeExplode - Time.time));
        }
    }
}