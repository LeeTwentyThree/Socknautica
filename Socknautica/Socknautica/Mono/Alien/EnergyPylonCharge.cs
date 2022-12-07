using System.Collections;

namespace Socknautica.Mono.Alien;

internal class EnergyPylonCharge : MonoBehaviour
{
    public Transform[] chargingPoints;
    public float chargeRange = 35f;
    public float energyPerSecond = 3f;

    private Vehicle currentTarget;
    private bool wasCharging;
    private float vfxDuration = 0.15f;
    private float vfxIntervalDuration = 3f;
    private FMODAsset chargeSound = Helpers.GetFmodAsset("event:/sub/base/chargers/charge_loop");
    private FMOD_CustomLoopingEmitter chargeEmitter;
    private LineRenderer lineRenderer;
    private TeleportScreenFXController teleportFx;

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
        lineRenderer.widthMultiplier = 4f;
        lineRenderer.positionCount = 2;
        lineRenderer.material.color = new Color(.34f, .62f, .36f, 1f);

        DestroyImmediate(powerTransmitter);

        teleportFx = MainCamera.camera.GetComponent<TeleportScreenFXController>();
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
        if (closest != null)
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
        }
    }

    private void CancelCharge()
    {
        wasCharging = false;
        StopAllCoroutines();
        Destroy(chargeEmitter);
        lineRenderer.enabled = false;
        teleportFx.StopTeleport();
    }

    private void Destroy()
    {
        Destroy(lineRenderer);
    }

    private IEnumerator StartChargingCoroutine()
    {
        while (wasCharging)
        {
            teleportFx.StartTeleport();
            yield return new WaitForSeconds(vfxDuration);
            teleportFx.StopTeleport();
            yield return new WaitForSeconds(vfxIntervalDuration);
        }
    }
}
