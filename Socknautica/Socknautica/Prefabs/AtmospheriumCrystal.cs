using Socknautica.Mono;

namespace Socknautica.Prefabs;

internal class AtmospheriumCrystal : Spawnable
{
    private GameObject prefab;

    public AtmospheriumCrystal() : base("AtmospheriumCrystal", "Atmospherium Crystal", "Low-density crystal that maintains its form under high pressures. Outside of the deep sea, the lack of intermolecular forces will cause it to vaporize.")
    {
    }

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            prefab = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("AtmospheriumPrefab"));
            prefab.SetActive(false);
            prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
            prefab.EnsureComponent<PrefabIdentifier>().classId = ClassID;
            prefab.EnsureComponent<TechTag>().type = TechType;
            prefab.EnsureComponent<SkyApplier>().renderers = prefab.GetComponentsInChildren<Renderer>();
            prefab.EnsureComponent<Pickupable>();
            prefab.EnsureComponent<AtmospheriumVaporization>();
            var rb = prefab.EnsureComponent<Rigidbody>();
            rb.mass = 0.5f;
            rb.useGravity = false;
            rb.isKinematic = true;
            var wf = prefab.EnsureComponent<WorldForces>();
            wf.underwaterGravity = 0f;
            MaterialUtils.ApplySNShaders(prefab, 8f, 2f, 1f);
        }
        return prefab;
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return Main.LoadSprite("PressuriumIcon");
    }
}
