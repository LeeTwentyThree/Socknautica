using ECCLibrary;
using Socknautica.Mono.Creatures;

namespace Socknautica.Prefabs.Creatures;

internal class AnglerFish : CreatureAsset
{
    public AnglerFish() : base("AnglerFish", "???", "Fear this creature.", Main.assetBundle.LoadAsset<GameObject>("Anglerfish_Prefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.VeryFar;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(50, 20, 50), 10, 3, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.Leviathan;

    public override StayAtLeashData StayAtLeashSettings => new StayAtLeashData(0.9f, 100f);

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.EnsureComponent<MirageFishBehaviour>();
    }

    public override AnimateByVelocityData AnimateByVelocitySettings => new AnimateByVelocityData(false, 30, 45, 0.5f);
    public override SmallVehicleAggressivenessSettings AggressivenessToSmallVehicles => new SmallVehicleAggressivenessSettings(0.4f, 100f);
    public override AttackLastTargetSettings AttackSettings => new AttackLastTargetSettings(0.9f, 30f, 20f, 20f, 25f, 23f);

    public override float MaxVelocityForSpeedParameter => 10;

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = 5000f;
    }

    public override float Mass => 3000f;
}
