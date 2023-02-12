using ECCLibrary;
using Socknautica.Mono;

namespace Socknautica.Prefabs.Creatures;

internal class AbyssalOculusSchool : GenericWorldPrefab
{
    public AbyssalOculusSchool() : base("AbyssalOculusSchool", "AbyssalOculusSchool", "", Main.assetBundle.LoadAsset<GameObject>("AbyssalOculusPrefab"), new UBERMaterialProperties(), LargeWorldEntity.CellLevel.Far, false)
    {
    }

    protected override void CustomizePrefab()
    {
    }
}
