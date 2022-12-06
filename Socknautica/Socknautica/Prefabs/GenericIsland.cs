using ECCLibrary;
using Socknautica.Mono;

namespace Socknautica.Prefabs;

internal class GenericIsland : GenericWorldPrefab
{
    public GenericIsland(string classId, GameObject model) : base(classId, "", "", model, new UBERMaterialProperties(8f), LargeWorldEntity.CellLevel.Far, false)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).gameObject;
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
        prefab.AddComponent<LootGeneration>();
    }
}
