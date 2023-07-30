using ECCLibrary;
using ECCLibrary.Internal;
using System.Collections.Generic;

namespace Socknautica.Mono.Creatures;

internal class BossCorpse
{
    public static void SpawnBossCorpse(Vector3 position)
    {
        var corpse = Object.Instantiate(Main.assetBundle.LoadAsset<GameObject>("MultiGargDeath_Prefab"));
        corpse.transform.position = position;
        var behaviourLOD = corpse.AddComponent<BehaviourLOD>();
        behaviourLOD.veryCloseThreshold = 3000;
        behaviourLOD.veryCloseThresholdSq = 3000 * 3000;
        behaviourLOD.closeThreshold = 10000;
        behaviourLOD.closeThresholdSq = 10000 * 10000;
        behaviourLOD.farThreshold = 15000;
        behaviourLOD.farThresholdSq = 15000 * 15000;
        corpse.transform.GetChild(0).localScale = Vector3.one * 0.6f;
        corpse.transform.eulerAngles = Vector3.up * (Boss.main.transform.localEulerAngles.y);

        var rb = corpse.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 20000;
        var wf = corpse.AddComponent<WorldForces>();
        wf.underwaterGravity = 3;

        corpse.AddComponent<BossCorpseHitFloor>();

        corpse.EnsureComponent<SkyApplier>().renderers = corpse.GetComponentsInChildren<Renderer>(true);
        MaterialUtils.ApplySNShaders(corpse);
    
        var spine2 = Search(corpse.transform, "Spine2");
        List<Transform> trailSpines = new List<Transform>();
        int spineIndex = 3;
        Transform current = spine2;
        do
        {
            current = current.Find("Spine" + spineIndex);
            if (current != null) trailSpines.Add(current);
            spineIndex++;
        }
        while (current != null);
        CreateTrail(corpse.transform, spine2.gameObject, trailSpines.ToArray(), 0.5f);

        var spine1 = Search(corpse.transform, "Spine1");
        AddNeckTrailManager(corpse.transform, spine1.Find("Neck1LD"));
        AddNeckTrailManager(corpse.transform, spine1.Find("Neck1LU"));
        AddNeckTrailManager(corpse.transform, spine1.Find("Neck1RD"));
        AddNeckTrailManager(corpse.transform, spine1.Find("Neck1RU"));
        AddNeckTrailManager(corpse.transform, spine1.Find("Neck1T"));

    }

    private static Transform Search(Transform inside, string lookingFor)
    {
        return inside.gameObject.SearchChild(lookingFor, ECCStringComparison.StartsWith).transform;
    }

    private static void AddNeckTrailManager(Transform prefabRoot, Transform root)
    {
        var trailSpines = new List<Transform>();
        int index = 2;
        Transform current = root;
        do
        {
            if (current.GetChild(0) != null && current.GetChild(0).name.StartsWith("Neck" + index))
            {
                current = current.GetChild(0);
                if (current != null)
                {
                    trailSpines.Add(current);
                }
            }
            else
            {
                current = null;
            }
            index++;
        }
        while (current != null);
        CreateTrail(prefabRoot, root.gameObject, trailSpines.ToArray(), 2);
    }

    private static TrailManager CreateTrail(Transform prefabRoot, GameObject trailRoot, Transform[] trails, float segmentSnapSpeed, float maxSegmentOffset = -1f)
    {
        trailRoot.gameObject.SetActive(value: false);
        TrailManagerECC trailManagerECC = trailRoot.AddComponent<TrailManagerECC>();
        trailManagerECC.trails = trails;
        trailManagerECC.rootTransform = prefabRoot;
        trailManagerECC.rootSegment = trailManagerECC.transform;
        trailManagerECC.levelOfDetail = prefabRoot.GetComponent<BehaviourLOD>();
        trailManagerECC.segmentSnapSpeed = segmentSnapSpeed;
        trailManagerECC.maxSegmentOffset = maxSegmentOffset;
        trailManagerECC.allowDisableOnScreen = false;
        trailManagerECC.yawMultiplier = (trailManagerECC.rollMultiplier = (trailManagerECC.pitchMultiplier = new AnimationCurve(new Keyframe(0f, 0.25f), new Keyframe(1f, 0.75f))));
        trailRoot.gameObject.SetActive(value: true);
        return trailManagerECC;
    }
}
