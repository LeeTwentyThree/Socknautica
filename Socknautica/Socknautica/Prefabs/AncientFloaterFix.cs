namespace Socknautica.Prefabs;

internal class AncientFloaterFix : Spawnable
{
    private GameObject prefab;

    public AncientFloaterFix() : base("AncientFloaterFix", "", "")
    {
    }

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            UWE.PrefabDatabase.TryGetPrefab(ClassIds.ancientFloaterUpsideDown, out var pref);
            prefab = Object.Instantiate(pref);
            prefab.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
            pref.SetActive(false);
        }
        return prefab;
    }
}
