namespace Socknautica.Mono.Creatures;

internal class Boss : MonoBehaviour
{
    public Creature creature;

    public GameObject projectilePrefab;

    public BossHead[] heads;

    public BossRoar roar;

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
        projectilePrefab = seamoth.torpedoTypes[0].prefab.GetComponent<SeamothTorpedo>().explosionPrefab.GetComponent<PrefabSpawn>().prefab;
        Destroy(projectilePrefab.GetComponent<VFXDestroyAfterSeconds>());
        var electricity = Helpers.SpawnPrecursorElectricSparks();
        electricity.transform.parent = projectilePrefab.transform;
        electricity.transform.localPosition = Vector3.zero;
        electricity.transform.localScale = Vector3.one;
        projectilePrefab.SetActive(false);
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
}
