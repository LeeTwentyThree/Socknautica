using ECCLibrary;
using Socknautica.Mono.Alien;

namespace Socknautica.Prefabs;

internal class EnergyPylon : GenericWorldPrefab
{
    public EnergyPylon() : base("BossEnergyPylon", "Energy Pylon", "", Main.assetBundle.LoadAsset<GameObject>("EnergyPylonPrefab"), new UBERMaterialProperties(8f), LargeWorldEntity.CellLevel.Far, true)
    {
    }

    protected override void CustomizePrefab()
    {
        var c = prefab.AddComponent<EnergyPylonCharge>();
        c.chargingPoints = Helpers.SearchAllTransforms(prefab, "ChargingPoint", ECCStringComparison.StartsWith).ToArray();
    }
}
