using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

public class ArenaSpawner : MonoBehaviour
{
    public static ArenaSpawner main;

    public Transform center;

    private static FMODAsset bossMusicEvent = Helpers.GetFmodAsset("BossMusic");

    private Vector3 arenaPos = new Vector3(0, -2000, 0);
    private Vector3 arenaTeleportPos = new Vector3(0, -1900, 100);

    public float arenaHeight = 180 * kScaleFactor;

    private const float kScaleFactor = 1.5f;

    private void Awake()
    {
        main = this;
    }
    public static void SpawnArena()
    {
        new GameObject("ArenaSpawner").AddComponent<ArenaSpawner>();
    }

    private IEnumerator Start()
    {
        var arena = Instantiate(Main.assetBundle.LoadAsset<GameObject>("ArenaBasePrefab"));
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
        SpawnObjects();
        center = arena.transform.Find("Center");
        LoopingMusic.Play(bossMusicEvent, 288);
        LargeWorld.main.transform.parent.Find("Chunks").gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
    }

    private void SpawnObjects()
    {
        float radius = 200 * kScaleFactor;
        int num = 8;
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
        SpawnCreature(TechType.Reefback, new Vector3(52, -1873, -86));
        SpawnCreature(TechType.Reefback, new Vector3(-32, -1896, 140));
    }

    private void FixSpawnedObject(GameObject spawned)
    {
        DestroyImmediate(spawned.GetComponent<LargeWorldEntity>());
        spawned.transform.parent = null;
        spawned.SetActive(true);
    }

    private void SpawnCreature(TechType techType, Vector3 globalPos)
    {
        var spawned = CraftData.InstantiateFromPrefab(techType);
        FixSpawnedObject(spawned);
        spawned.transform.localPosition = globalPos;
    }

    private void SpawnEnergyPylon(Vector3 loc, Vector3 eulers = default)
    {
        var spawned = CraftData.InstantiateFromPrefab(Main.energyPylon.TechType);
        FixSpawnedObject(spawned);
        spawned.transform.localScale = Vector3.one * 10;
        spawned.transform.position = arenaPos + loc;
        spawned.transform.eulerAngles = eulers;
    }
}
