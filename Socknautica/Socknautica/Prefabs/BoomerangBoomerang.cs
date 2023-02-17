using System;

namespace Socknautica.Prefabs;

internal class BoomerangBoomerang : Equipable
{
    public BoomerangBoomerang() : base("BoomerangBoomerang", "Boomerang Boomerang", "A boomerang fish that can be used as a boomerang.")
    {
    }

    public override EquipmentType EquipmentType => EquipmentType.Hand;

    protected override TechData GetBlueprintRecipe()
    {
        return new TechData(new Ingredient(TechType.Boomerang, 16));
    }

    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;

    public override TechCategory CategoryForPDA => TechCategory.Equipment;

    public override TechGroup GroupForPDA => TechGroup.Personal;

    public override string[] StepsToFabricatorTab => new string[] { "Personal", "Equipment" };

    public override float CraftingTime => 4f;

    public override GameObject GetGameObject()
    {
        var prefab = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Boomerang));
        Helpers.RemoveNonEssentialComponents(prefab);
        prefab.AddComponent<ThrowBoomerang>();
        return prefab;
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return SpriteManager.Get(TechType.Boomerang);
    }
}
