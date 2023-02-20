using System.Collections;

namespace Socknautica.Mono.Alien;

internal class EnergyPylonCharge : MonoBehaviour
{
    public Transform[] chargingPoints;
    public float chargeRange = 100f;
    public float energyPerSecond = 6f;

    private Vehicle currentTarget;
    private bool wasCharging;
    private float vfxDuration = 0.15f;
    private FMOD_CustomLoopingEmitter chargeEmitter;
    private LineRenderer lineRenderer;
    private TeleportScreenFXController teleportFx;

    private static int totalSpawned;
    private int uniqueId;

    private static FMODAsset chargeSound = Helpers.GetFmodAsset("event:/sub/base/chargers/charge_loop");
    private static FMODAsset explodeSound = Helpers.GetFmodAsset("EnergyPylonExplosion");

    private void Start()
    {
        GameObject powerTransmitter = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter);
        powerTransmitter.transform.SetParent(transform, false);
        Utils.ZeroTransform(powerTransmitter.transform);

        GameObject laserBeam = GameObject.Instantiate(powerTransmitter.GetComponent<PowerFX>().vfxPrefab, transform.position - new Vector3(0, 2, 0), transform.rotation);
        laserBeam.SetActive(true);

        lineRenderer = laserBeam.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.widthMultiplier = 12f;
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = new Color(.34f, .62f, .36f, 1f);

        DestroyImmediate(powerTransmitter);

        teleportFx = MainCamera.camera.GetComponent<TeleportScreenFXController>();

        uniqueId = totalSpawned++;
    }

    private void Update()
    {
        currentTarget = Player.main.GetVehicle();
        if (currentTarget != null)
        {
            Charge();
        }
        else
        {
            CancelCharge();
        }
    }

    private void Charge()
    {
        Transform closest = null;
        float minDist = chargeRange;
        foreach (var point in chargingPoints)
        {
            var dist = Vector3.Distance(point.position, currentTarget.transform.position);
            if (dist < minDist)
            {
                closest = point;
                minDist = dist;
            }
        }
        if (closest != null && currentTarget != null)
        {
            currentTarget.AddEnergy(energyPerSecond * Time.deltaTime);
            if (!wasCharging)
            {
                wasCharging = true;
                StartCoroutine(StartChargingCoroutine());
                chargeEmitter = currentTarget.gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
                chargeEmitter.followParent = true;
                chargeEmitter.SetAsset(chargeSound);
                chargeEmitter.Play();
            }
            lineRenderer.SetPositions(new Vector3[] { closest.position, currentTarget.transform.position + Vector3.down });
            lineRenderer.enabled = true;
        }
        else
        {
            CancelCharge();
            if (ArenaSpawner.main != null && Vector3.Distance(transform.position, ArenaSpawner.main.center.position) < 700)
            {
                ConnectToReactor();
            }

        }
    }

    private void ConnectToReactor()
    {
        Transform closest = null;
        float minDist = float.MaxValue;
        var center = ArenaSpawner.main.center;
        foreach (var point in chargingPoints)
        {
            var dist = Vector3.Distance(point.position, center.position);
            if (dist < minDist)
            {
                closest = point;
                minDist = dist;
            }
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(new Vector3[] { closest.position, GetReactorConnectPosition(center.position) });
    }

    private Vector3 GetReactorConnectPosition(Vector3 centerPos)
    {
        return centerPos + Vector3.up * (Mathf.PerlinNoise(uniqueId * 100f, Time.time / 6f) * ArenaSpawner.main.arenaHeight - ArenaSpawner.main.arenaHeight / 2f);
    }

    private void CancelCharge()
    {
        wasCharging = false;
        Destroy(chargeEmitter);
        lineRenderer.enabled = false;
    }

    private void Destroy()
    {
        Destroy(lineRenderer);
    }

    private IEnumerator StartChargingCoroutine()
    {
        teleportFx.StartTeleport();
        yield return new WaitForSeconds(vfxDuration);
        teleportFx.StopTeleport();
    }

    public void Explode()
    {
        Utils.PlayFMODAsset(explodeSound, transform.position);
        var arena = ArenaSpawner.main;
        if (arena != null)
        {
            ArenaSpawner.SpawnSeamothExplosion(transform.position, 10);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (lineRenderer) Destroy(lineRenderer.gameObject);
    }
}
