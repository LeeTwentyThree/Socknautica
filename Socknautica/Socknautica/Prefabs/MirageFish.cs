using Socknautica.Mono;

namespace Socknautica.Prefabs;

internal class MirageFish : Spawnable
{
    private GameObject prefab;

    public MirageFish() : base("MirageFish", "???", "")
    {
    }

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            prefab = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("MirageFish_Prefab"));
            prefab.SetActive(false);
            prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Far;
            prefab.EnsureComponent<PrefabIdentifier>().classId = ClassID;
            prefab.EnsureComponent<TechTag>().type = TechType;
            prefab.EnsureComponent<SkyApplier>().renderers = prefab.GetComponentsInChildren<Renderer>();
            var rb = prefab.EnsureComponent<Rigidbody>();
            rb.mass = 1000f;
            rb.isKinematic = true;
            prefab.EnsureComponent<WorldForces>();
            MaterialUtils.ApplySNShaders(prefab, 8f, 2f, 1f);
            prefab.EnsureComponent<MirageFishBehaviour>();
        }
        return prefab;
    }
}
