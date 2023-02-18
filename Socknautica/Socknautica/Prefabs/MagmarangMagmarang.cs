using System;

namespace Socknautica.Prefabs;

internal class MagmarangMagmarang : Equipable
{
    public MagmarangMagmarang() : base("MagmarangMagmarang", "Magmarang Magmarang", "A Magmarang fish that can be used as a... magmarang?")
    {
    }

    public override EquipmentType EquipmentType => EquipmentType.Hand;

    protected override TechData GetBlueprintRecipe()
    {
        return new TechData(new Ingredient(TechType.LavaBoomerang, 4)) { craftAmount = 1 };
    }

    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;

    public override TechCategory CategoryForPDA => TechCategory.Equipment;

    public override TechGroup GroupForPDA => TechGroup.Personal;

    public override string[] StepsToFabricatorTab => new string[] { "Personal", "Tools" };

    public override float CraftingTime => 4f;

    public override GameObject GetGameObject()
    {
        var prefab = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.LavaBoomerang));
        Helpers.RemoveNonEssentialComponents(prefab);
        var b = prefab.AddComponent<ThrowBoomerang>();
        b.lava = true;
        b.mainCollider = prefab.GetComponent<Collider>();
        b.pickupable = prefab.GetComponent<Pickupable>();
        prefab.EnsureComponent<TechTag>();
        return prefab;
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return SpriteManager.Get(TechType.LavaBoomerang);
    }
}
