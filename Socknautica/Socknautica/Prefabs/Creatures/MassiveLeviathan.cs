using ECCLibrary;

namespace Socknautica.Prefabs.Creatures;

internal class MassiveLeviathan : CreatureAsset
{
    public MassiveLeviathan() : base("MassiveLeviathan", "???", "Fear this creature.", Main.assetBundle.LoadAsset<GameObject>("MassiveLeviathan_Prefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Global;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(false, default, 0, 0, 0);

    public override EcoTargetType EcoTargetType => EcoTargetType.Leviathan;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }
}