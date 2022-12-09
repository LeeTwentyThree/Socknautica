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
    private Vector3 arenaTeleportPos = new Vector3(0, -2000, 0);
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
        var teleportFx = MainCamera.camera.GetComponent<TeleportScreenFXController>();
        teleportFx.StartTeleport();
        yield return new WaitForSeconds(2f);

        var arena = Instantiate(Main.assetBundle.LoadAsset<GameObject>("ArenaBasePrefab"));
        arena.transform.position = arenaPos;
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
        LargeWorld.main.transform.parent.Find("Chunks").gameObject.SetActive(false);
        Player.main.transform.position = arenaTeleportPos;
        SpawnObjects();
        center = arena.transform.Find("Center");

        yield return new WaitForSeconds(2f);
        teleportFx.StopTeleport();

        var emitter = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
        emitter.SetAsset(bossMusicEvent);
        emitter.Play();
    }

    private void SpawnObjects()
    {
        float radius = 200;
        int num = 12;
        float max = Mathf.PI * 2f;
        for (int i = 0; i < num; i++)
        {
            var percent = i * num / max;
            SpawnEnergyPylon(new Vector3(Mathf.Cos(percent) * radius, 0f, Mathf.Sin(percent) * radius));
        }
    }

    private void SpawnEnergyPylon(Vector3 loc)
    {
        var spawned = CraftData.InstantiateFromPrefab(Main.energyPylon.TechType);
        DestroyImmediate(spawned.GetComponent<LargeWorldEntity>());
        spawned.SetActive(true);
        spawned.transform.localScale = Vector3.one * 6f;
        spawned.transform.position = arenaPos + loc;
    }
}
