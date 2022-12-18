using ECCLibrary;
using System.Collections.Generic;
using Socknautica.Mono.Creatures;

namespace Socknautica.Prefabs;

internal class Multigarg : CreatureAsset
{
    public Multigarg() : base("Multigarg", "Sock Eater", "[REDACTED]", Main.assetBundle.LoadAsset<GameObject>("MultigargPrefab"), null)
    {
    }

    public override BehaviourType BehaviourType => BehaviourType.Leviathan;

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Global;

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(300f, 100f, 300f), 40f, 5f, BossBalance.fallbackSwimPriority);

    public override EcoTargetType EcoTargetType => EcoTargetType.Whale;

    public override float TurnSpeed => 0.1f;
    public override float TurnSpeedHorizontal => 0.1f;
    public override float TurnSpeedVertical => 3f;
    public override float MaxVelocityForSpeedParameter => BossBalance.swimVelocity;
    public override RespawnData RespawnSettings => new RespawnData(false);

    public override BehaviourLODLevelsStruct BehaviourLODSettings => new BehaviourLODLevelsStruct(400, 1000, 2000);
    public override StayAtLeashData StayAtLeashSettings => new StayAtLeashData(0, 1000);
    public override float Mass => 20000;

    public override bool CanBeInfected => false;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        var boss = prefab.AddComponent<Boss>();
        boss.creature = components.creature;

        var roar = prefab.AddComponent<BossRoar>();
        roar.creature = components.creature;
        boss.roar = roar;

        prefab.AddComponent<ChargeAtPlayer>();
        prefab.AddComponent<SwimRandomArena>();
        prefab.AddComponent<BossCollisions>();
        prefab.AddComponent<FireEnergyBall>().boss = boss;
        prefab.AddComponent<FireEnergyMines>().boss = boss;

        components.swimRandom.swimForward = 0.7f;
        components.locomotion.freezeHorizontalRotation = true;
        components.locomotion.driftFactor = 0.5f;
        components.locomotion.maxVelocity = 40f;
        components.locomotion.maxAcceleration = 60f;

        var spine2 = Search(prefab.transform, "Spine2");
        List<Transform> trailSpines = new List<Transform>();
        int spineIndex = 3;
        Transform current = spine2;
        do {
            current = current.Find("Spine" + spineIndex);
            if (current != null) trailSpines.Add(current);
            spineIndex++;
        }
        while (current != null);
        CreateTrail(spine2.gameObject, trailSpines.ToArray(), components, 2f);

        var spine1 = Search(prefab.transform, "Spine1");
        AddNeckTrailManager(components, spine1.Find("Neck1LD"));
        AddNeckTrailManager(components, spine1.Find("Neck1LU"));
        AddNeckTrailManager(components, spine1.Find("Neck1RD"));
        AddNeckTrailManager(components, spine1.Find("Neck1RU"));
        AddNeckTrailManager(components, spine1.Find("Neck1T"));

        boss.heads = new BossHead[] { AddHead("HeadLD"), AddHead("HeadLU"), AddHead("HeadRD"), AddHead("HeadRU"), AddHead("HeadT") };
    }

    private Transform Search(Transform inside, string lookingFor)
    {
        return inside.gameObject.SearchChild(lookingFor, ECCStringComparison.StartsWith).transform;
    }

    private BossHead AddHead(string headParentName)
    {
        var headParent = prefab.transform.Search(headParentName);
        var head = headParent.gameObject.AddComponent<BossHead>();
        head.jawTransform = headParent.GetChild(0);

        return head;
    }

    private void AddNeckTrailManager(CreatureComponents components, Transform root)
    {
        List<Transform> trailSpines = new List<Transform>();
        int index = 2;
        Transform current = root;
        do
        {
            if (current.GetChild(0) != null && current.GetChild(0).name.StartsWith("Neck" + index))
            {
                current = current.GetChild(0);
                if (current != null) trailSpines.Add(current);
            }
            else
            {
                current = null;
            }
            index++;
        }
        while (current != null);
        CreateTrail(root.gameObject, trailSpines.ToArray(), components, 20);
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }
}
