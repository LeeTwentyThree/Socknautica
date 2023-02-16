namespace Socknautica.Prefabs;

internal class GiantTooth : Spawnable
{
    public GiantTooth() : base("GiantTooth", "", "")
    {
    }

    public override GameObject GetGameObject()
    {
        var prefab = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.StalkerTooth));
        // Object.DestroyImmediate(prefab.GetComponentInChildren<Collider>());
        Object.DestroyImmediate(prefab.GetComponent<Rigidbody>());
        Object.DestroyImmediate(prefab.GetComponent<Pickupable>());
        Object.DestroyImmediate(prefab.GetComponent<ResourceTracker>());
        Object.DestroyImmediate(prefab.GetComponent<ResourceTracker>());
        prefab.transform.GetChild(0).localScale = Vector3.one * 5f;
        prefab.SetActive(false);
        return prefab;
    }
}
