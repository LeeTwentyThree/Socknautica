using ECCLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Prefabs;

internal class ArenaTeleporterBaseModel : GenericWorldPrefab
{
    public ArenaTeleporterBaseModel() : base("ArenaTeleporterBaseModel", "Arena Teleporter", "", Main.assetBundle.LoadAsset<GameObject>("ArenaTeleporterBasePrefab"), new UBERMaterialProperties(8), LargeWorldEntity.CellLevel.Far, true)
    {
    }

    protected override void CustomizePrefab()
    {
    }
}
