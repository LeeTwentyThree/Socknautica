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

    public override float TurnSpeedHorizontal => 0.2f;
    public override float TurnSpeedVertical => 3f;
    public override float MaxVelocityForSpeedParameter => 40f;
    public override RespawnData RespawnSettings => new RespawnData(false);

    public override StayAtLeashData StayAtLeashSettings => new StayAtLeashData(0.4f, 400);
    public override BehaviourLODLevelsStruct BehaviourLODSettings => new BehaviourLODLevelsStruct(400, 1000, 2000);

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        components.swimRandom.swimForward = 0.7f;
        components.locomotion.freezeHorizontalRotation = true;
        components.locomotion.driftFactor = 0.5f;
        components.locomotion.maxVelocity = 40f;
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }
}
