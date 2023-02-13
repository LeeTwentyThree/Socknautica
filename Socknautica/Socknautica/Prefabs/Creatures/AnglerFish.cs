using ECCLibrary;

namespace Socknautica.Prefabs.Creatures;

internal class AnglerFish : CreatureAsset
{
    public AnglerFish() : base("AnglerFish", "???", "Fear this creature.", Main.assetBundle.LoadAsset<GameObject>("Anglerfish_Prefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.VeryFar;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(50, 20, 50), 6, 3, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.Leviathan;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.EnsureComponent<MirageFishBehaviour>();
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }

    public override float Mass => 1000f;
}
