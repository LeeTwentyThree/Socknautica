using ECCLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Prefabs;

internal class PressuriumCrystal : Spawnable
{
    private GameObject prefab;

    public PressuriumCrystal() : base("PressuriumCrystal", "Pressurium Crystal", "Navy blue crystal with pressure-resistant properties. Exclusive to 4546B, it likely has biological origins.")
    {
    }

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            prefab = GameObject.Instantiate(Main.assetBundle.LoadAsset<GameObject>("PressuriumPrefab"));
            prefab.SetActive(false);
            prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
            prefab.EnsureComponent<PrefabIdentifier>().classId = ClassID;
            prefab.EnsureComponent<TechTag>().type = TechType;
            prefab.EnsureComponent<SkyApplier>().renderers = prefab.GetComponentsInChildren<Renderer>();
            prefab.EnsureComponent<Pickupable>();
            var rb = prefab.EnsureComponent<Rigidbody>();
            rb.mass = 20f;
            rb.useGravity = false;
            rb.isKinematic = true;
            prefab.EnsureComponent<WorldForces>();
            MaterialUtils.ApplySNShaders(prefab, 8f, 2f, 1f);
        }
        return prefab;
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return Main.LoadSprite("PressuriumIcon");
    }
}
