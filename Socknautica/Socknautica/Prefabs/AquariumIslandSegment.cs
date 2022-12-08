using ECCLibrary;
using Socknautica.Mono;
using System.Collections.Generic;

namespace Socknautica.Prefabs;

internal class AquariumIslandSegment : GenericWorldPrefab
{
    public AquariumIslandSegment(string classId, GameObject model) : base(classId, "", "", model, new UBERMaterialProperties(6f), LargeWorldEntity.CellLevel.VeryFar, false)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).transform.GetChild(0).gameObject;
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
        var lootGen = prefab.AddComponent<LootGeneration>();
        lootGen.preset = LootGeneration.Preset.AquariumIsland;
    }
}
