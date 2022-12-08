using ECCLibrary;
using Socknautica.Mono;

namespace Socknautica.Prefabs;

internal class CoordBaseModel : GenericWorldPrefab
{
    public CoordBaseModel() : base("CoordBaseModel", "", "", Main.assetBundle.LoadAsset<GameObject>("CoordBasePrefab"), new UBERMaterialProperties(7), LargeWorldEntity.CellLevel.VeryFar, true)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).gameObject;
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
        var lootGen = prefab.AddComponent<LootGeneration>();
        lootGen.preset = LootGeneration.Preset.CoordBaseIsland;
    }
}
