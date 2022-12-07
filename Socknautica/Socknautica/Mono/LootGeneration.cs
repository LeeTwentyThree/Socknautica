using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;
using UWE;

namespace Socknautica.Mono;

internal class LootGeneration : MonoBehaviour
{
    public float leviathanProbability = 0.05f;
    public Vector3 leviathanOffset = new(0, 17, 0);
    public List<LootGroup> groups = new List<LootGroup>();

    private PrefabIdentifier identifier;

    private static List<string> leviathanClassIDs;

    private void Start()
    {
        identifier = gameObject.GetComponent<PrefabIdentifier>();
        if (saveData == null) saveData.Load();
        if (saveData.completedUniqueIdentifiers == null) saveData.completedUniqueIdentifiers = new List<string>();

        if (saveData.completedUniqueIdentifiers.Contains(identifier.Id))
        {
            Destroy(this);
        }
        else
        {
            Generate();
        }
    }

    private static LootGenerationData saveData { get; } = SaveDataHandler.Main.RegisterSaveDataCache<LootGenerationData>();

    private static void DetermineLeviathanClassIDs()
    {
        leviathanClassIDs = new List<string>();
        leviathanClassIDs.Add("f78942c3-87e7-4015-865a-5ae4d8bd9dcb"); // reaper
        leviathanClassIDs.Add("5ea36b37-300f-4f01-96fa-003ae47c61e5"); // ghost
        leviathanClassIDs.Add("8d3d3c8b-9290-444a-9fea-8e5493ecd6fe"); // reefback
        if (OtherMods.BloopAndBlazaModExists)
        {
            leviathanClassIDs.Add("blazaleviathan");
            leviathanClassIDs.Add("bloop");
        }
    }

    public void Generate()
    {
        saveData.completedUniqueIdentifiers.Add(identifier.Id);
        SpawnLeviathanIfPossible();
        foreach (var lootGroup in groups) GenerateLootGroup(lootGroup);
        Destroy(this);
    }

    private void GenerateLootGroup(LootGroup group)
    {
        var slots = Helpers.SearchAllTransforms(gameObject, group.namePrefix, ECCLibrary.ECCStringComparison.StartsWith);
    }

    private void SpawnLeviathanIfPossible()
    {
        if (leviathanClassIDs == null) DetermineLeviathanClassIDs();
        if (Random.value < leviathanProbability)
        {
            SpawnPrefab(leviathanClassIDs[Random.Range(0, leviathanClassIDs.Count)], transform.position + leviathanOffset, Quaternion.identity);
        }
    }

    private GameObject SpawnPrefab(string classId, Vector3 loc, Quaternion rot, Vector3 scale = default)
    {
        if (scale == default) scale = Vector3.one;
        if (PrefabDatabase.TryGetPrefab(classId, out var prefab))
        {
            var s = Instantiate(prefab, loc, rot);
            s.transform.localScale = scale;
            s.SetActive(true);
            return s;
        }
        return null;
    }
}
[FileName("lootgenerationdata")]
internal class LootGenerationData : SaveDataCache
{
    public List<string> completedUniqueIdentifiers;
}
internal struct LootGroup
{
    public string namePrefix;
    public string[] spawnClassIDs;
    public float probability, scaleMin, scaleMax;
    
    public LootGroup(string namePrefix, float probability, float scaleMin, float scaleMax, params string[] spawnClassIDs)
    {
        this.namePrefix = namePrefix;
        this.spawnClassIDs = spawnClassIDs;
        this.probability = probability;
        this.scaleMin = scaleMin;
        this.scaleMax = scaleMax;
    }
}