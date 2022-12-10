using ECCLibrary;

namespace Socknautica.Prefabs;

internal class Multigarg : CreatureAsset
{
    public Multigarg() : base("Multigarg", "Sock Eater", "[REDACTED]", Main.assetBundle.LoadAsset<GameObject>("MultigargPrefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Global;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(300f, 100f, 300f), 40f, 5f, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.Whale;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }
}
