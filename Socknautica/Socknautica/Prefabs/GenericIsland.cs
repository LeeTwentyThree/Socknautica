using ECCLibrary;
using Socknautica.Mono;
using System.Collections.Generic;

namespace Socknautica.Prefabs;

internal class GenericIsland : GenericWorldPrefab
{
    public GenericIsland(string classId, GameObject model) : base(classId, "", "", model, new UBERMaterialProperties(4f), LargeWorldEntity.CellLevel.Far, false)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).gameObject;
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
        var lootGen = prefab.AddComponent<LootGeneration>();
        lootGen.preset = LootGeneration.Preset.GenericIsland;
        lootGen.precursorProbability = 0.34f;
    }
}
