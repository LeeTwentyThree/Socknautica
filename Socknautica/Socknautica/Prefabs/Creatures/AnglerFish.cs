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

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.EnsureComponent<MirageFishBehaviour>();
        prefab.EnsureComponent<HuntDownPlayer>();

        GameObject mouth = prefab.SearchChild("MouthTrigger");
        mouth.AddComponent<OnTouch>();

        AnglerMeleeAttack meleeAttack = prefab.AddComponent<AnglerMeleeAttack>();
        meleeAttack.mouth = mouth;
        meleeAttack.canBeFed = false;
        meleeAttack.biteInterval = 5f;
        meleeAttack.eatHungerDecrement = 0.05f;
        meleeAttack.eatHappyIncrement = 0.1f;
        meleeAttack.biteAggressionDecrement = 0.02f;
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.lastTarget = components.lastTarget;
        meleeAttack.creature = components.creature;
        meleeAttack.liveMixin = components.liveMixin;
        meleeAttack.animator = components.creature.GetAnimator();
    }

    public override AnimateByVelocityData AnimateByVelocitySettings => new AnimateByVelocityData(false, 30, 45, 0.5f);
    public override AttackLastTargetSettings AttackSettings => new AttackLastTargetSettings(0.9f, 30f, 20f, 20f, 25f, 23f);

    public override float MaxVelocityForSpeedParameter => 10;

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = 5000f;
    }

    public override float Mass => 3000f;
}
