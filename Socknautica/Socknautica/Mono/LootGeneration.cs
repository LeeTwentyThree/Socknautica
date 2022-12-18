using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;
using UWE;

namespace Socknautica.Mono;

public class LootGeneration : MonoBehaviour
{
    public float leviathanProbability = 0.25f;
    public Vector3 leviathanOffset = new(0, 17, 0);
    public Preset preset;
    private List<LootGroup> groups;

    public float maxPrecursorTechProbability;

    private PrefabIdentifier identifier;

    private static List<string> leviathanClassIDs;
    private static List<string> precursorTechClassIDs;

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

    private static void DetermineClassIDs()
    {
        leviathanClassIDs = new List<string>();
        leviathanClassIDs.Add("5ea36b37-300f-4f01-96fa-003ae47c61e5"); // ghost
        leviathanClassIDs.Add("8d3d3c8b-9290-444a-9fea-8e5493ecd6fe"); // reefback
        if (OtherMods.BloopAndBlazaModExists)
        {
            leviathanClassIDs.Add("BlazaLeviathan");
            leviathanClassIDs.Add("Bloop");
        }

        precursorTechClassIDs = new List<string>();
        precursorTechClassIDs.Add(Main.arenaLightPillar.ClassID);
        precursorTechClassIDs.Add(Main.energyPylon.ClassID);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_box);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_box_double);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_box_quadruple);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_destroyedTile);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_largeChunk);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.damageprop_smallPanel);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.structure_outpost_1); // sonic deterrent
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.structure_outpost_2);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.structure_column);
        precursorTechClassIDs.Add(Alien.AlienBaseSpawner.structure_skeletonScanner1);
    }

    private void DetermineGroups()
    {
        groups = new List<LootGroup>();
        if (preset == Preset.AquariumIsland)
        {
            groups.Add(new LootGroup("GiantFloaterLocation", 1f, 0.1f, 0.1f, ClassIds.ancientFloater)); // ancient floater
            groups.Add(new LootGroup("RandomPlantSpawn", 0.9f, 1f, 1f, ClassIds.rogueCradle, ClassIds.bloodVine1, ClassIds.bloodVine2, ClassIds.bloodVine3, ClassIds.bloodVine4, ClassIds.fansSmall, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3));
            groups.Add(new LootGroup("SpawnPointFish", 0.1f, 1f, 1f, ClassIds.bladderfish, ClassIds.oculus, ClassIds.hoverfish, ClassIds.spinefish, ClassIds.peeper, ClassIds.ghostray, ClassIds.crabsquid, ClassIds.boneshark, ClassIds.ampeel));
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
            groups.Add(new PlantLootGroup("RandomPlantSpawn", 0.2f, 1f, 1f, ClassIds.rogueCradle, ClassIds.bloodVine1, ClassIds.bloodVine2, ClassIds.bloodVine3, ClassIds.bloodVine4, ClassIds.fansSmall, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3));
            groups.Add(new LootGroup("SpawnPointFish", 0.05f, 1f, 1f, ClassIds.bladderfish, ClassIds.oculus, ClassIds.hoverfish, ClassIds.spinefish, ClassIds.peeper, ClassIds.ghostray, ClassIds.crabsquid, ClassIds.boneshark, ClassIds.ampeel));
            var genericIslandResources = new LootGroup("ResourceSpawnPoint", 0.2f, 1f, 1f, ClassIds.sandstoneChunk, ClassIds.limestoneChunk, ClassIds.shaleChunk, ClassIds.pressurium, ClassIds.barnacle, ClassIds.magnetite, ClassIds.lithium);
            genericIslandResources.canSpawnAtmospherium = true;
            groups.Add(genericIslandResources);
        }
        if (preset == Preset.CoordBaseIsland)
        {
            groups.Add(new LootGroup("GiantFloaterLocation", 1f, 0.1f, 0.1f, ClassIds.ancientFloater)); // ancient floater
            groups.Add(new LootGroup("SpawnPointFish", 0.12f, 1f, 1f, ClassIds.warperSpawner, ClassIds.oculus));
            groups.Add(new LootGroup("LightFlower", 0.7f, 1f, 1f, ClassIds.circleGrass));
            groups.Add(new LootGroup("ReefbackCoral01", 0.5f, 1f, 1f, ClassIds.reefbackCoral1));
            groups.Add(new LootGroup("ReefbackCoral02", 0.5f, 1f, 1f, ClassIds.reefbackCoral2));
            groups.Add(new LootGroup("ReefbackCoral03", 0.5f, 1f, 1f, ClassIds.reefbackCoral3));
            groups.Add(new LootGroup("ResourceSpawnPoint", 1f, 1f, 1f, ClassIds.barnacle, ClassIds.magnetite));
            groups.Add(new LootGroup("SmallFan1", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallFan2", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallFan3", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallFan4", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallFan5", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallFanCluster", 1f, 1f, 1f, ClassIds.fans1, ClassIds.fans2, ClassIds.fans3, ClassIds.fans4, ClassIds.fans5));
            groups.Add(new LootGroup("SmallPlantBloodKelp", 1f, 1f, 1f, "2ab96dc4-5201-4a41-aa5c-908f0a9a0da8", "2bfcbaf4-1ae6-4628-9816-28a6a26ff340"));
            groups.Add(new LootGroup("RougeCradle", 1f, 1f, 1f, ClassIds.rogueCradle));

        }
    }

    public void Generate()
    {
        saveData.completedUniqueIdentifiers.Add(identifier.Id);
        SpawnSpecialObjects();
        DetermineGroups();
        foreach (var lootGroup in groups) GenerateLootGroup(lootGroup);
        //Destroy(this);
    }

    private void GenerateLootGroup(LootGroup group)
    {
        var slots = Helpers.SearchAllTransforms(gameObject, group.namePrefix, ECCLibrary.ECCStringComparison.StartsWith);
        foreach (var slot in slots)
        {
            if (group.Evaluate(slot))
            {
                var spawned = SpawnPrefab(group.GetRandomClassID(slot), slot.position, Vector3.forward, Vector3.one * Random.Range(group.scaleMin, group.scaleMax));
                Helpers.SwapZAndYComponents(spawned.transform, slot);
            }
        }
    }

    private void SpawnSpecialObjects()
    {
        if (leviathanClassIDs == null || precursorTechClassIDs == null) DetermineClassIDs();
        if (Random.value < leviathanProbability)
        {
            SpawnPrefab(leviathanClassIDs[Random.Range(0, leviathanClassIDs.Count)], transform.position + leviathanOffset, Quaternion.identity);
        }
        var precursorSlots = Helpers.SearchAllTransforms(gameObject, "PrecursorTechSpawnPoint", ECCLibrary.ECCStringComparison.StartsWith);
        foreach (var precursorSlot in precursorSlots)
        {
            if (Random.value < PrecursorZone.GetGlobalClosenessPercent(precursorSlot.position) * maxPrecursorTechProbability)
            {
                var classId = precursorTechClassIDs[Random.Range(0, precursorTechClassIDs.Count)];
                var spawned = SpawnPrefab(classId, precursorSlot.position, precursorSlot.rotation);
                if (classId.Equals(Main.energyPylon.ClassID)) spawned.transform.localScale = Vector3.one * 6;
            }
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
        AquariumIsland,
        CoordBaseIsland
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
    public bool canSpawnAtmospherium;

    public virtual bool Evaluate(Transform slot)
    {
        if (Random.value > probability)
        {
            return false;
        }
        return true;
    }

    public LootGroup(string namePrefix, float probability, float scaleMin, float scaleMax, params string[] spawnClassIDs)
    {
        this.namePrefix = namePrefix;
        this.spawnClassIDs = spawnClassIDs;
        this.probability = probability;
        this.scaleMin = scaleMin;
        this.scaleMax = scaleMax;
    }

    public string GetRandomClassID(Transform atSlot)
    {
        bool atmoshperiumSpawner = canSpawnAtmospherium && atSlot.position.y < -2000;

        int maxIndex = spawnClassIDs.Length;
        if (atmoshperiumSpawner) maxIndex++;
        int randomIndex = Random.Range(0, maxIndex);

        string chosenClassId;
        if (atmoshperiumSpawner && randomIndex >= maxIndex - 1)
            chosenClassId = Main.atmospheriumCrystal.ClassID;
        else
            chosenClassId = spawnClassIDs[randomIndex];
        return chosenClassId;
    }
}
[System.Serializable]
public class PlantLootGroup : LootGroup
{
    public PlantLootGroup(string namePrefix, float probability, float scaleMin, float scaleMax, params string[] spawnClassIDs) : base(namePrefix, probability, scaleMin, scaleMax, spawnClassIDs)
    {
    }

    public override bool Evaluate(Transform slot)
    {
        if (!base.Evaluate(slot)) return false;
        return slot.position.y > -2000;
    }
}