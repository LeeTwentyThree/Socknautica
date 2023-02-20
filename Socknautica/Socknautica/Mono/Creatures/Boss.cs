namespace Socknautica.Mono.Creatures;

internal class Boss : MonoBehaviour
{
    public Creature creature;

    public GameObject projectilePrefab;

    public GameObject vomitPrefab;

    public BossHead[] heads;

    public BossRoar roar;

    private float scaleFactor = 2f;

    public float Anger
    {
        get
        {
            return _anger;
        }
        set
        {
            _anger = Mathf.Clamp01(_anger);
        }
    }

    public BossHead GetRandomHead()
    {
        return heads[Random.Range(0, heads.Length)];
    }

    private float _anger;

    private void Start()
    {
        SeaMoth seamoth = CraftData.GetPrefabForTechType(TechType.Seamoth).GetComponent<SeaMoth>();
        projectilePrefab = Instantiate(seamoth.torpedoTypes[0].prefab.GetComponent<SeamothTorpedo>().explosionPrefab.GetComponent<PrefabSpawn>().prefab);
        Destroy(projectilePrefab.GetComponent<VFXDestroyAfterSeconds>());
        var electricity = Helpers.SpawnPrecursorElectricSparks();
        electricity.transform.parent = projectilePrefab.transform;
        electricity.transform.localPosition = Vector3.zero;
        electricity.transform.localScale = Vector3.one * 3.5f;
        Helpers.MakeParticleSystemScaleable(projectilePrefab);
        Helpers.MakeParticleSystemLoopable(projectilePrefab);
        projectilePrefab.SetActive(false);

        var gas = CraftData.GetPrefabForTechType(TechType.Gasopod).GetComponent<GasoPod>().podPrefab.GetComponent<GasPod>().gasEffectPrefab;
        vomitPrefab = Instantiate(gas, Player.main.transform.position, Quaternion.identity);
        Helpers.MakeParticleSystemScaleable(vomitPrefab);
        foreach (var ps in vomitPrefab.GetComponentsInChildren<ParticleSystem>())
        {
            var main = ps.main;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }
        vomitPrefab.transform.localScale *= 10;
        vomitPrefab.SetActive(false);
    }

    public EnergyBallDamageInRange SpawnEnergyBall(Vector3 position, float scale = 30)
    {
        var spawned = Instantiate(projectilePrefab, position, Quaternion.identity);
        spawned.transform.localScale = Vector3.one * scale;
        spawned.SetActive(true);
        var damageComponent = spawned.AddComponent<EnergyBallDamageInRange>();
        damageComponent.boss = this;
        return damageComponent;
    }

    public GameObject SpawnVomit(Vector3 position)
    {
        var spawned = Instantiate(vomitPrefab, position, Quaternion.identity);
        spawned.SetActive(true);
        return spawned;
    }

    private FMODAsset attackSound = Helpers.GetFmodAsset("BossAttack");

    public void PlayAttackSound()
    {
        Utils.PlayFMODAsset(attackSound, transform.position);
    }

    private void Update()
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
