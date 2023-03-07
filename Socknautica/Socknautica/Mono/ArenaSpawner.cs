using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socknautica.Mono.Alien;

namespace Socknautica.Mono;

public class ArenaSpawner : MonoBehaviour
{
    public static ArenaSpawner main;

    public Transform center;

    private static FMODAsset bossMusicEvent = Helpers.GetFmodAsset("BossMusic");

    private Vector3 arenaPos = new Vector3(0, -2000, 0);

    private Vector3 teleportTankRadiusCenter = new Vector3(647, -2207, -1610);
    private float teleportTankRadius = 100f;
    private Vector3 teleportTankDropIntoArenaPosition = new Vector3(0, -1996, 475);

    public float arenaHeight = 180 * kScaleFactor;

    private const float kScaleFactor = 2f;

    private List<EnergyPylonCharge> energyPylons = new List<EnergyPylonCharge>();

    public static GameObject cyclopsObj;
    public static GameObject seamothExplode;

    public GameObject arena;

    private bool exploded;

    private void Awake()
    {
        main = this;
    }
    public static void SpawnArena()
    {
        new GameObject("ArenaSpawner").AddComponent<ArenaSpawner>();
    }

    private IEnumerator LoadVFX()
    {
        seamothExplode = CraftData.GetPrefabForTechType(TechType.Seamoth).GetComponent<SeaMoth>().destructionEffect;
        yield return new WaitUntil(() => LightmappedPrefabs.main);
        LightmappedPrefabs.main.RequestScenePrefab("Cyclops", new LightmappedPrefabs.OnPrefabLoaded(OnSubPrefabLoaded));
    }

    public void OnSubPrefabLoaded(GameObject prefab)
    {
        cyclopsObj = prefab;
    }

    private IEnumerator Start()
    {
        Story.StoryGoalManager.main.OnGoalComplete("BeginBossFight");
        StartCoroutine(LoadVFX());
        arena = Instantiate(Main.assetBundle.LoadAsset<GameObject>("ArenaBasePrefab"));
        arena.transform.position = arenaPos;
        arena.transform.localScale = Vector3.one * kScaleFactor;
        MaterialUtils.ApplySNShaders(arena);
        MaterialUtils.ApplyPrecursorMaterials(arena, 2f);
        arena.EnsureComponent<SkyApplier>().renderers = arena.gameObject.GetComponentsInChildren<Renderer>(true);
        var reactor = arena.transform.Find("ArenaReactorForSocks");
        var ionShader = new Material(MaterialUtils.IonCubeMaterial);
        int ringNum = 0;
        foreach (Transform child in reactor)
        {
            if (child.name.ToLower().Contains("ring"))
            {
                child.gameObject.GetComponent<Renderer>().material = ionShader;
                child.gameObject.AddComponent<ReactorDonutSpin>().reverse = ringNum % 2 == 0;
                ringNum++;
            }
        }
        center = arena.transform.Find("Center");
        SpawnObjects();
        LoopingMusic.Play(bossMusicEvent, 288);
        LargeWorld.main.transform.parent.Find("Chunks").gameObject.SetActive(false);
        foreach (var ping in PingManager.pings.Values)
        {
            if (ping != null && ping.origin != null)
            {
                if (Vector3.SqrMagnitude(ping.origin.position - teleportTankRadiusCenter) < teleportTankRadius * teleportTankRadius)
                {
                    var root = UWE.Utils.GetEntityRoot(ping.origin.gameObject);
                    if (root != null)
                    {
                        root.transform.localPosition = teleportTankDropIntoArenaPosition;
                        root.transform.localEulerAngles = Vector3.up * 90;
                    }
                    break;
                }
                else
                {
                    PingManager.SetVisible(PingManager.GetId(ping), false);
                }
            }
        }
        if (Main.config.NoFogInArena)
        {
            foreach (WaterscapeVolume waterscapeVolume in FindObjectsOfType<WaterscapeVolume>())
            {
                waterscapeVolume.enabled = false;
            }
        }
        yield return new WaitForSeconds(4f);
    }

    private void SpawnObjects()
    {
        float radius = 200 * kScaleFactor;
        int num = 6;
        float max = Mathf.PI * 2f;
        for (int i = 0; i < num; i++)
        {
            var percent = (float)i / num * max;
            SpawnEnergyPylon(new Vector3(Mathf.Cos(percent) * radius, 0f, Mathf.Sin(percent) * radius));
            SpawnEnergyPylon(new Vector3(Mathf.Cos(percent) * radius, arenaHeight, Mathf.Sin(percent) * radius), Vector3.right * 180);
        }
        SpawnCreature(TechType.GhostLeviathan, new Vector3(-182, -1900, 136));
        SpawnCreature(TechType.GhostLeviathan, new Vector3(175, -1918, 104));
        SpawnCreature(TechType.GhostLeviathan, new Vector3(116, -1930, -205));
        SpawnCreature(TechType.GhostLeviathan, new Vector3(-90, -1881, -146));
        SpawnCreature(TechType.SeaDragon, new Vector3(52, -1873, -86));
        SpawnCreature(TechType.SeaDragon, new Vector3(-32, -1896, 140));
        SpawnCreature(Main.abyssalBlaza.TechType, new Vector3(-304.66f, -1872.92f, -2.17f));
        SpawnCreature(Main.ancientBloop.TechType, new Vector3(-7.32f, -1890.77f, -313.86f));
        SpawnCreature(Main.abyssalBlaza.TechType, new Vector3(398.72f, -1845.19f, 56.60f));
        SpawnCreature(Main.ancientBloop.TechType, new Vector3(4.51f, -1822.17f, 404.14f));
        SpawnCreature(Main.multigarg.TechType, new Vector3(400, -1900, 0));
        for (int i = 0; i < 50; i++)
        {
            var pos2d = Random.insideUnitCircle * 500f;
            var pos3d = new Vector3(pos2d.x, arenaPos.y, pos2d.y);
            if (Vector3.Distance(Helpers.Flatten(pos3d), Helpers.Flatten(center.position)) < 129) continue;
            if (Vector3.Distance(Helpers.Flatten(pos3d), Helpers.Flatten(teleportTankDropIntoArenaPosition)) < 100) continue;
            bool invalid = false;
            foreach (var pylon in energyPylons)
            {
                if (pylon && Vector3.Distance(pylon.transform.position, pos3d) < 60) invalid = true;
            }
            if (invalid) continue;
            SpawnLightPillar(pos3d);
        }
    }

    private void FixSpawnedObject(GameObject spawned)
    {
        DestroyImmediate(spawned.GetComponent<LargeWorldEntity>());
        spawned.transform.parent = null;
        spawned.SetActive(true);
    }

    private void SpawnLightPillar(Vector3 locPos)
    {
        var spawned = CraftData.InstantiateFromPrefab(Main.arenaLightPillar.TechType);
        spawned.transform.localScale = Vector3.one * Random.Range(2f, 3f);
        FixSpawnedObject(spawned);
        spawned.transform.localPosition = locPos;
        spawned.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        spawned.SetActive(true);
    }

    private void SpawnCreature(TechType techType, Vector3 globalPos, Vector3 scale = default)
    {
        var spawned = CraftData.InstantiateFromPrefab(techType);
        FixSpawnedObject(spawned);
        spawned.transform.localPosition = globalPos;
        if (scale != default) spawned.transform.localScale = scale;
        var leash = spawned.GetComponent<StayAtLeashPosition>();
        if (leash != null) leash.leashDistance = 9999;
        if (techType != Main.multigarg.TechType)
        {
            var swimRandom = spawned.GetComponent<SwimRandom>();
            if (swimRandom)
            {
                spawned.AddComponent<Creatures.HuntDownPlayer>().velocity = swimRandom.swimVelocity;
            }
        }
    }

    private void SpawnEnergyPylon(Vector3 loc, Vector3 eulers = default)
    {
        var spawned = CraftData.InstantiateFromPrefab(Main.energyPylon.TechType);
        FixSpawnedObject(spawned);
        spawned.transform.localScale = Vector3.one * 10;
        spawned.transform.position = arenaPos + loc;
        spawned.transform.eulerAngles = eulers;
        energyPylons.Add(spawned.GetComponent<EnergyPylonCharge>());
    }
    
    public static GameObject SpawnSeamothExplosion(Vector3 pos, float scale)
    {
        var go = Instantiate(seamothExplode, pos, Quaternion.identity);
        Helpers.MakeParticleSystemScaleable(go);
        go.transform.GetChild(1).gameObject.SetActive(false);
        go.transform.localScale = Vector3.one * scale;
        return go;
    }

    private void Update()
    {
        if (exploded) return;
        if (energyPylons == null || energyPylons.Count == 0) return;
        int alive = 0;
        foreach (var pylon in energyPylons)
        {
            if (pylon != null) alive++;
        }
        if (alive == 0)
        {
            exploded = true;
            ArenaExplosion.Explode();
        }
    }
}
