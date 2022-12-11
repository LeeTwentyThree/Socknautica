namespace Socknautica.Prefabs;

internal class ArenaLightPillar : Spawnable
{
    private GameObject prefab;

    public ArenaLightPillar() : base("ArenaLightPillar", "", "")
    {
    }

    private float scaleFactor = 3f;

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            UWE.PrefabDatabase.TryGetPrefab("04ad6244-1766-4622-bb8a-7fa29845bc68", out var pref);
            prefab = Object.Instantiate(pref);
            foreach (Transform child in prefab.transform)
            {
                if (child.name == "Pillar")
                {
                    child.localPosition = Vector3.zero;
                    child.localEulerAngles = Vector3.zero;
                    child.localScale = Vector3.one * scaleFactor;
                    foreach (var light in child.gameObject.GetComponentsInChildren<Light>()) light.range *= scaleFactor;
                }
                else
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }
            pref.SetActive(false); 
        }
        return prefab;
    }
}
