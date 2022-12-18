using System;
using System.Collections.Generic;
using ECCLibrary;
using UWE;

namespace Socknautica;

internal static class Helpers
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

    public static bool MusicIsPaused
    {
        get
        {
            return UWE.FreezeTime.freezers.Count != 0;
        }
    }

    public static float Angle(Vector2 vector2)
    {
        return Mathf.Atan2(vector2.y, vector2.x);
    }

    public static GameObject SpawnPrecursorElectricSparks()
    {
        PrefabDatabase.TryGetPrefab("ff8e782e-e6f3-40a6-9837-d5b6dcce92bc", out var prefab);
        var spawned = UnityEngine.Object.Instantiate(prefab);
        foreach (var l in spawned.GetComponentsInChildren<Light>())
        {
            l.color = new Color(0.54f, 1f, 0.54f);
        }
        foreach (var r in spawned.GetComponentsInChildren<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m.SetColor(ShaderPropertyID._Color, new Color(0.5f, 1f, 0.5f));
            }
        }
        return spawned;
    }

    public static GameObject Search(this GameObject gameObject, string byName)
    {
        return SearchChildRecursive(gameObject, byName);
    }

    public static Transform Search(this Transform transform, string byName)
    {
        return SearchChildRecursive(transform.gameObject, byName).transform;
    }

    private static GameObject SearchChildRecursive(GameObject gameObject, string byName)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (string.Equals(child.gameObject.name, byName))
            {
                return child.gameObject;
            }
            GameObject recursive = SearchChildRecursive(child.gameObject, byName);
            if (recursive)
            {
                return recursive;
            }
        }
        return null;
    }
}
