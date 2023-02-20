using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Prefabs;

internal class PrecursorProjector : SimpleBuildable
{
    public PrecursorProjector() : base("PrecursorProjector", "Leviathan Holographic Projector", "A pedestal that displays great creatures that you have defeated.")
    {
    }

    public override TechGroup GroupForPDA => TechGroup.InteriorModules;

    public override TechCategory CategoryForPDA => TechCategory.InteriorModule;

    public override ConstructableSettings ConstructableSettings => new ConstructableSettings(true, true, true, false, true, false, true, true, false, 4, 2.3f, 7f);

    public override LargeWorldEntity.CellLevel CellLevel => LargeWorldEntity.CellLevel.Medium;

    public override GameObject Model => Main.assetBundle.LoadAsset<GameObject>("PrecursorPedestal_Prefab");

    protected override OrientedBounds[] GetBounds => new OrientedBounds[] { new OrientedBounds(Vector3.up * 0.6f, Quaternion.identity, Vector3.one * 0.5f) };

    public override bool UnlockedAtStart => true;

    protected override TechData GetBlueprintRecipe()
    {
        return new TechData(new Ingredient(TechType.TitaniumIngot, 1), new Ingredient(TechType.ComputerChip, 1));
    }

    protected override Atlas.Sprite GetItemSprite()
    {
        return new Atlas.Sprite(Main.assetBundle.LoadAsset<Sprite>("PrecursorPedestal_Icon"));
    }

    public override void ApplyChangesToPrefab(GameObject prefab)
    {
        prefab.EnsureComponent<ProjectorController>();
    }
}
