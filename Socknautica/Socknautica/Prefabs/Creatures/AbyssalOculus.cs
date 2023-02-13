namespace Socknautica.Prefabs.Creatures;

internal class AbyssalOculus : ReskinSpawnable
{
    private Texture2D mainTex;
    private Texture2D specTex;
    private Texture2D emissiveTex;
    private Texture2D normalTex;

    public AbyssalOculus() : base("AbyssalOculus", "Abyssal Oculus", "A creepy creature.")
    {
        mainTex = Main.assetBundle.LoadAsset<Texture2D>("oculus_01");
        emissiveTex = Main.assetBundle.LoadAsset<Texture2D>("oculus_01_illum");
        normalTex = Main.assetBundle.LoadAsset<Texture2D>("oculus_01_normal");
        specTex = Main.assetBundle.LoadAsset<Texture2D>("oculus_01_spec");
    }

    protected override string ReferenceClassId => "aefe2153-9e68-41cf-9615-253aa6f965aa";

    protected override void ApplyChangesToPrefab(GameObject prefab)
    {
        Object.DestroyImmediate(prefab.GetComponent<Pickupable>());
        prefab.EnsureComponent<EcoTarget>().type = EcoTargetType.MediumFish;
        prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Far;
        var rendererObj = prefab.transform.Find("model/Oculus");
        prefab.transform.Find("model/Oculus_LOD1").gameObject.SetActive(false);
        prefab.transform.Find("model/Oculus_LOD2").gameObject.SetActive(false);
        prefab.transform.Find("model/Oculus_LOD3").gameObject.SetActive(false);
        var renderer = rendererObj.GetComponent<Renderer>();
        var material = renderer.material;
        material.SetTexture("_MainTex", mainTex);
        material.SetTexture("_SpecTex", specTex);
        material.SetTexture("_BumpMap", normalTex);
        material.SetTexture("_Illum", emissiveTex);
        renderer.material = material;
        prefab.transform.GetChild(0).localScale *= 6;
        var blod = prefab.GetComponent<BehaviourLOD>();
        blod.veryCloseThreshold = 50;
        blod.closeThreshold = 150;
        blod.farThreshold = 500;
        prefab.GetComponent<Creature>().Tired = new CreatureTrait(0, 1);
        prefab.GetComponent<SphereCollider>().radius = 2.8f;
        prefab.GetComponent<Rigidbody>().mass = 50;
    }
}
