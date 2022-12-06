using ECCLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Prefabs;

internal class AquariumBaseModel : GenericWorldPrefab
{
    public AquariumBaseModel(string classId, string friendlyName, string description, GameObject model, UBERMaterialProperties materialProperties, LargeWorldEntity.CellLevel cellLevel, bool applyPrecursorMaterial = true) : base(classId, friendlyName, description, model, materialProperties, cellLevel, applyPrecursorMaterial)
    {
    }

    protected override void CustomizePrefab()
    {
        var islandMesh = prefab.transform.GetChild(0).GetChild(1).Find("IslandMesh");
        islandMesh.GetComponent<Renderer>().material = MaterialUtils.AuroraRockMaterial;
    }
}
