using ECCLibrary;
using Socknautica.Mono;

namespace Socknautica.Prefabs;

internal class AquariumIslandSegment : GenericWorldPrefab
{
    public AquariumIslandSegment(string classId, GameObject model) : base(classId, "", "", model, new UBERMaterialProperties(8f), LargeWorldEntity.CellLevel.Far, false)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).transform.GetChild(0).gameObject;
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
        var lootGen = prefab.AddComponent<LootGeneration>();
        lootGen.groups.Add(new LootGroup("GiantFloaterLocation", 1f, 1f, 1f, "37ea521a-6be4-437c-8ed7-6b453d9218a8")); // ancient floater
        lootGen.groups.Add(new LootGroup("RandomPlantSpawn", 1f, 1f, 1f, "37ea521a-6be4-437c-8ed7-6b453d9218a8")); // 
    }
}
