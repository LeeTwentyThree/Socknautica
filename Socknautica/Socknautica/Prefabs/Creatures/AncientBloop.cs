﻿using ECCLibrary;
using System.Collections.Generic;
using Socknautica.Mono.Creatures;

namespace Socknautica.Prefabs.Creatures;

internal class AncientBloop : CreatureAsset
{
    public static GameObject vortexVfx;

    public AncientBloop() : base("AncientBloop", "Ancient Bloop", "", Main.assetBundle.LoadAsset<GameObject>("BloopV2_Prefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.VeryFar;

    public override EcoTargetType EcoTargetType => EcoTargetType.Leviathan;

    public override BehaviourLODLevelsStruct BehaviourLODSettings => new BehaviourLODLevelsStruct(100, 300, 500);

    public override float Mass => 3000;
    public override float TurnSpeedHorizontal => 0.4f;
    public override float MaxVelocityForSpeedParameter => 16;

    public override UBERMaterialProperties MaterialSettings => new UBERMaterialProperties(8, 8, 1);

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.AddComponent<GenericRoar>().SetEssentials(20f, 200f, 30f, 40f, "AncientBloopRoar", null);

        var spine1 = Search(prefab.transform, "Spine1");
        List<Transform> trailSpines = new List<Transform>();
        int spineIndex = 2;
        Transform current = spine1;
        do
        {
            current = current.Find("Spine" + spineIndex);
            if (current != null) trailSpines.Add(current);
            spineIndex++;
        }
        while (current != null);
        CreateTrail(spine1.gameObject, trailSpines.ToArray(), components, 2f);

        components.locomotion.maxVelocity = 20;
        components.locomotion.maxAcceleration = 9;

        MakeAggressiveTo(140, 3, EcoTargetType.Shark, 0.1f, 2);
        MakeAggressiveTo(60, 3, EcoTargetType.Leviathan, 0f, 0.5f);

        components.creature.Hunger = new CreatureTrait(0f, -0.05f);
        components.locomotion.driftFactor = 0.9f;
        prefab.AddComponent<Mono.Creatures.SwimAmbience>();

        ValidateVortexVFX();

        BloopBehavior behavior = prefab.AddComponent<BloopBehavior>();
        prefab.AddComponent<BloopVortexAttack>();

        GameObject mouth = prefab.SearchChild("MouthTrigger");
        BloopMeleeAttack meleeAttack = prefab.AddComponent<BloopMeleeAttack>();
        meleeAttack.mouth = mouth;
        meleeAttack.canBeFed = false;
        meleeAttack.biteInterval = 3f;
        meleeAttack.biteDamage = 75f;
        meleeAttack.eatHungerDecrement = 0.05f;
        meleeAttack.eatHappyIncrement = 0.1f;
        meleeAttack.biteAggressionDecrement = 0.02f;
        meleeAttack.biteAggressionThreshold = 0.1f;
        meleeAttack.lastTarget = components.lastTarget;
        meleeAttack.creature = components.creature;
        meleeAttack.liveMixin = components.liveMixin;
        meleeAttack.animator = components.creature.GetAnimator();

        mouth.AddComponent<OnTouch>();

        AttackCyclops actionAtkCyclops = prefab.AddComponent<AttackCyclops>();
        actionAtkCyclops.swimVelocity = 15f;
        actionAtkCyclops.aggressiveToNoise = new CreatureTrait(0f, 0.01f);
        actionAtkCyclops.evaluatePriority = 0.6f;
        actionAtkCyclops.priorityMultiplier = ECCHelpers.Curve_Flat();
        actionAtkCyclops.maxDistToLeash = 70f;
        actionAtkCyclops.attackAggressionThreshold = 0.4f;
    }

    private void ValidateVortexVFX()
    {
        if (vortexVfx == null)
        {
            SeaMoth seamoth = CraftData.GetPrefabForTechType(TechType.Seamoth).GetComponent<SeaMoth>();
            vortexVfx = seamoth.torpedoTypes[0].prefab.GetComponent<SeamothTorpedo>().explosionPrefab.GetComponent<PrefabSpawn>().prefab;
            GameObject.Destroy(vortexVfx.GetComponent<VFXDestroyAfterSeconds>());
        }
    }

    private Transform Search(Transform inside, string lookingFor)
    {
        return inside.gameObject.SearchChild(lookingFor, ECCStringComparison.StartsWith).transform;
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = 8000f;
    }

    public override AnimateByVelocityData AnimateByVelocitySettings => new AnimateByVelocityData(false);

    public override bool CanBeInfected => false;

    // actions
    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(60, 10, 60), 10, 3f, 0.1f);
    public override SmallVehicleAggressivenessSettings AggressivenessToSmallVehicles => new SmallVehicleAggressivenessSettings(0.4f, 17f);
    public override AttackLastTargetSettings AttackSettings => new AttackLastTargetSettings(0.6f, 20, 8f, 15f, 24f, 10);
    public override AvoidObstaclesData AvoidObstaclesSettings => new AvoidObstaclesData(0.9f, true, 30f);
    public override StayAtLeashData StayAtLeashSettings => new StayAtLeashData(0.9f, 200);

}
