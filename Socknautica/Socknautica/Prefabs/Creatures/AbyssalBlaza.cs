using ECCLibrary;
using System.Collections.Generic;
using Socknautica.Mono.Creatures;

namespace Socknautica.Prefabs.Creatures;

internal class AbyssalBlaza : CreatureAsset
{
    public AbyssalBlaza() : base("AbyssalBlaza", "Abyssal Blaza", "", Main.assetBundle.LoadAsset<GameObject>("BlazaV2_Prefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.VeryFar;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, Vector3.one * 100, 10, 3f, 0.1f);

    public override EcoTargetType EcoTargetType => EcoTargetType.Leviathan;

    public override BehaviourLODLevelsStruct BehaviourLODSettings => new BehaviourLODLevelsStruct(300, 600, 4000);

    public override SmallVehicleAggressivenessSettings AggressivenessToSmallVehicles => new SmallVehicleAggressivenessSettings(0.4f, 100f);

    public override float Mass => 2300;
    public override float TurnSpeedHorizontal => 0.6f;
    public override float MaxVelocityForSpeedParameter => 20f;

    public override bool CanBeInfected => false;
    public override bool AcidImmune => true;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.AddComponent<GenericRoar>().SetEssentials(15f, 150f, 8.5f, 16f, "AbyssalBlazaIdle", null);

        var spine1 = Search(prefab.transform, "Spine_1");
        List<Transform> trailSpines = new List<Transform>();
        int spineIndex = 2;
        Transform current = spine1;
        do
        {
            current = current.Find("Spine_" + spineIndex);
            if (current != null) trailSpines.Add(current);
            spineIndex++;
        }
        while (current != null);
        CreateTrail(spine1.gameObject, trailSpines.ToArray(), components, 2f);

        components.locomotion.maxAcceleration = 7;
        components.locomotion.driftFactor = 0.5f;

        MakeAggressiveTo(150, 4, EcoTargetType.Shark, 0, 2);
        MakeAggressiveTo(60, 3, EcoTargetType.Leviathan, 0f, 0.5f);

        BlazaBehaviour blazaBehaviour = prefab.AddComponent<BlazaBehaviour>();
        blazaBehaviour.creature = components.creature;

        GameObject mouth = prefab.SearchChild("MouthTrigger");
        BlazaMeleeAttack meleeAttack = prefab.AddComponent<BlazaMeleeAttack>();
        meleeAttack.mouth = mouth;
        meleeAttack.canBeFed = false;
        meleeAttack.biteInterval = 2f;
        meleeAttack.biteDamage = 75f;
        meleeAttack.eatHungerDecrement = 0.05f;
        meleeAttack.eatHappyIncrement = 0.1f;
        meleeAttack.biteAggressionDecrement = 0.02f;
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.lastTarget = components.lastTarget;
        meleeAttack.creature = components.creature;
        meleeAttack.liveMixin = components.liveMixin;
        meleeAttack.animator = components.creature.GetAnimator();

        AttackCyclops actionAtkCyclops = prefab.AddComponent<AttackCyclops>();
        actionAtkCyclops.swimVelocity = 30f;
        actionAtkCyclops.aggressiveToNoise = new CreatureTrait(0f, 0.01f);
        actionAtkCyclops.evaluatePriority = 0.6f;
        actionAtkCyclops.priorityMultiplier = ECCHelpers.Curve_Flat();
        actionAtkCyclops.maxDistToLeash = 200f;
        actionAtkCyclops.attackAggressionThreshold = 0.25f;

        mouth.AddComponent<OnTouch>();
    }

    private Transform Search(Transform inside, string lookingFor)
    {
        return inside.gameObject.SearchChild(lookingFor, ECCStringComparison.StartsWith).transform;
    }

    public override AttackLastTargetSettings AttackSettings => new AttackLastTargetSettings(0.6f, 45f, 8f, 15f, 24f, 8);

    public override StayAtLeashData StayAtLeashSettings => new StayAtLeashData(0.5f, 200f);

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = 8000f;
    }
}
