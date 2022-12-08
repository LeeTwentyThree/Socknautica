using System;
using System.Collections.Generic;
using ECCLibrary;

namespace Socknautica;

internal class Helpers
{
    public static FMODAsset GetFmodAsset(string path)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.path = path;
        asset.id = path;
        return asset;
    }

    public static List<Transform> SearchAllTransforms(GameObject root, string search, ECCStringComparison searchMode)
    {
        var transforms = new List<Transform>();
        foreach (Transform t in root.GetComponentsInChildren<Transform>())
        {
            if (root.transform == t) continue;
            if (ECCHelpers.CompareStrings(t.gameObject.name, search, searchMode)) transforms.Add(t);
        }
        return transforms;
    }

    public static void SwapZAndYComponents(Transform original, Transform reference)
    {
        original.forward = reference.forward;
        original.localEulerAngles += Vector3.right * 90;
    }
}
