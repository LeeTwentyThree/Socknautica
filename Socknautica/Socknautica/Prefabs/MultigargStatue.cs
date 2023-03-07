using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Prefabs;

internal class MultigargStatue : SimpleBuildable
{
    public MultigargStatue() : base("MultigargStatue", "Boss Statue", "The statue of a slain beast.")
    {
    }

    public override TechGroup GroupForPDA => TechGroup.ExteriorModules;

    public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;

    public override ConstructableSettings ConstructableSettings => new ConstructableSettings(true, true, true, false, true, false, true, true, false, 10f, 5f, 15f);

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Far;

    public override GameObject Model => Main.assetBundle.LoadAsset<GameObject>("MultigargStatue_Prefab");

    protected override OrientedBounds[] GetBounds => new OrientedBounds[] { new OrientedBounds(Vector3.up * 0.6f, Quaternion.identity, Vector3.one * 0.5f) };

    public override bool UnlockedAtStart => false;

    protected override TechData GetBlueprintRecipe()
    {
        return new TechData(new Ingredient(TechType.Titanium, 2), new Ingredient(TechType.Gold, 5));
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return new Atlas.Sprite(Main.assetBundle.LoadAsset<Sprite>("StatueIcon"));
    }

    public override void ApplyChangesToPrefab(GameObject prefab)
    {
        MaterialUtils.ApplySNShaders(prefab);
        var r = prefab.GetComponentInChildren<Renderer>();
        var m = r.materials;
        var m0 = m[0];
        m0.SetColor("_Color", new Color(.95f, .43f, .1f));
        var m1 = m[1];
        m1.color = new Color(.5f, .5f, .5f);
        var m3 = m[3];
        m3.EnableKeyword("MARMO_SPECMAP");
        m3.EnableKeyword("MARMO_EMISSION");
        m3.SetColor("_SpecColor", new Color(2, 1.5f, .05f));
        m3.SetFloat("_SpecInt", 2);
        m3.SetFloat("_Shininess", 8);
        m3.SetFloat("_Fresnel", 0.65f);
        m3.SetFloat("_GlowStrength", 0);
        m3.SetFloat("_GlowStrengthNight", 0);
        m3.SetFloat("_EmissionLMNight", 0.02f);
        m[0] = m0;
        m[1] = m1;
        m[3] = m3;
        r.materials = m;

        var light = new GameObject("Light");
        light.transform.parent = prefab.transform;
        light.transform.localPosition = Vector3.up * 5;
        var l = light.AddComponent<Light>();
    }
}
