using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;
using UWE;

namespace Socknautica.Mono;

public class LootGeneration : MonoBehaviour
{
    public float leviathanProbability = 0.05f;
    public Vector3 leviathanOffset = new(0, 17, 0);
    public Preset preset;
    private List<LootGroup> groups;

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

    private void DetermineGroups()
    {
        groups = new List<LootGroup>();
        if (preset == Preset.AquariumIsland)
        {
            groups.Add(new LootGroup("GiantFloaterLocation", 1f, 0.1f, 0.1f, ClassIds.ancientFloater)); // ancient floater
            groups.Add(new LootGroup("RandomPlantSpawn", 0.9f, 1f, 1f, ClassIds.rogueCradle, ClassIds.bloodVine1, ClassIds.bloodVine2, ClassIds.bloodVine3, ClassIds.bloodVine4, ClassIds.fansSmall, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3));
            groups.Add(new LootGroup("SpawnPointFish", 0.1f, 1f, 1f, ClassIds.bladderfish, ClassIds.oculus, ClassIds.hoverfish, ClassIds.spinefish, ClassIds.peeper, ClassIds.blighter, ClassIds.crabsquid, ClassIds.boneshark, ClassIds.ampeel));
            groups.Add(new LootGroup("ResourceSpawnPoint", 1f, 1f, 1f, ClassIds.sandstoneChunk, ClassIds.limestoneChunk, ClassIds.shaleChunk, ClassIds.pressurium, ClassIds.barnacle, ClassIds.magnetite));
            groups.Add(new LootGroup("ReefbackCoral01", 0.5f, 1f, 1f, ClassIds.reefbackCoral1));
            groups.Add(new LootGroup("ReefbackCoral02", 0.5f, 1f, 1f, ClassIds.reefbackCoral2));
            groups.Add(new LootGroup("ReefbackCoral03", 0.5f, 1f, 1f, ClassIds.reefbackCoral3));
            groups.Add(new LootGroup("BloodKelpSmallPlant", 1f, 1f, 1f, ClassIds.bloodVine1, ClassIds.bloodVine2));
            groups.Add(new LootGroup("Gabe'sFeather", 1f, 1f, 1f, ClassIds.gabesFeather));
        }
        if (preset == Preset.GenericIsland)
        {
            groups.Add(new LootGroup("GiantFloaterLocation", 1f, 0.1f, 0.1f, ClassIds.ancientFloater)); // ancient floater
            groups.Add(new LootGroup("RandomPlantSpawn", 0.2f, 1f, 1f, ClassIds.rogueCradle, ClassIds.bloodVine1, ClassIds.bloodVine2, ClassIds.bloodVine3, ClassIds.bloodVine4, ClassIds.fansSmall, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3));
            groups.Add(new LootGroup("SpawnPointFish", 0.12f, 1f, 1f, ClassIds.bladderfish, ClassIds.oculus, ClassIds.hoverfish, ClassIds.spinefish, ClassIds.peeper, ClassIds.blighter, ClassIds.crabsquid, ClassIds.boneshark, ClassIds.ampeel));
            groups.Add(new LootGroup("ResourceSpawnPoint", 1f, 1f, 1f, ClassIds.sandstoneChunk, ClassIds.limestoneChunk, ClassIds.shaleChunk, ClassIds.pressurium, ClassIds.barnacle, ClassIds.magnetite));
        }
    }

    public void Generate()
    {
        saveData.completedUniqueIdentifiers.Add(identifier.Id);
        SpawnLeviathanIfPossible();
        DetermineGroups();
        foreach (var lootGroup in groups) GenerateLootGroup(lootGroup);
        //Destroy(this);
    }

    private void GenerateLootGroup(LootGroup group)
    {
        var slots = Helpers.SearchAllTransforms(gameObject, group.namePrefix, ECCLibrary.ECCStringComparison.StartsWith);
        foreach (var slot in slots)
        {
            if (Random.value <= group.probability)
            {
                SpawnPrefab(group.spawnClassIDs[Random.Range(0, group.spawnClassIDs.Length)], slot.position, slot.up, Vector3.one * Random.Range(group.scaleMin, group.scaleMax));
            }
        }
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

    private GameObject SpawnPrefab(string classId, Vector3 loc, Vector3 forward, Vector3 scale = default)
    {
        if (scale == default) scale = Vector3.one;
        if (PrefabDatabase.TryGetPrefab(classId, out var prefab))
        {
            var s = Instantiate(prefab, loc, Quaternion.identity);
            s.transform.localScale = scale;
            s.transform.forward = forward;
            s.SetActive(true);
            return s;
        }
        return null;
    }

    public enum Preset
    {
        GenericIsland,
        AquariumIsland
    }
}
[FileName("lootgenerationdata")]
internal class LootGenerationData : SaveDataCache
{
    public List<string> completedUniqueIdentifiers;
}
[System.Serializable]
public class LootGroup
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