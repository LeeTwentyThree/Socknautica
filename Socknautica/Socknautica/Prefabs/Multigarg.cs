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

    public override SwimRandomData SwimRandomSettings => new SwimRandomData(true, new Vector3(300f, 100f, 300f), 40f, 5f, 0f);

    public override EcoTargetType EcoTargetType => EcoTargetType.Whale;

    public override float TurnSpeedHorizontal => 0.05f;
    public override float TurnSpeedVertical => 3f;
    public override float MaxVelocityForSpeedParameter => BossBalance.swimVelocity;
    public override RespawnData RespawnSettings => new RespawnData(false);

    public override BehaviourLODLevelsStruct BehaviourLODSettings => new BehaviourLODLevelsStruct(400, 1000, 2000);
    public override float Mass => 20000;

    public override void AddCustomBehaviour(CreatureComponents components)
    {
        prefab.AddComponent<BossRoar>();
        prefab.AddComponent<ChargeAtPlayer>();
        prefab.AddComponent<SwimRandomArena>().evaluatePriority = BossBalance.swimPriority;

        components.swimRandom.swimForward = 0.7f;
        components.locomotion.freezeHorizontalRotation = true;
        components.locomotion.driftFactor = 0.5f;
        components.locomotion.maxVelocity = 40f;

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

        /*var spine1 = Search(prefab.transform, "Spine1");
        AddNeckTrailManager(components, spine1.Find("Neck1LD"));
        AddNeckTrailManager(components, spine1.Find("Neck1LU"));
        AddNeckTrailManager(components, spine1.Find("Neck1RD"));
        AddNeckTrailManager(components, spine1.Find("Neck1RU"));
        AddNeckTrailManager(components, spine1.Find("Neck1T"));*/
    }

    private Transform Search(Transform inside, string lookingFor)
    {
        return inside.gameObject.SearchChild(lookingFor, ECCStringComparison.StartsWith).transform;
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
        CreateTrail(root.gameObject, trailSpines.ToArray(), components, 2f);
    }

    public override void SetLiveMixinData(ref LiveMixinData liveMixinData)
    {
        liveMixinData.maxHealth = float.MaxValue;
    }
}
