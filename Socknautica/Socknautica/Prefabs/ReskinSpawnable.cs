using System.Collections;
using System.Collections.Generic;
using SMLHelper.V2.Assets;
using UnityEngine;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using UWE;

namespace Socknautica.Prefabs;

/// <summary>
/// An item based of off a base-game item, that can spawn in the world if you want.
/// </summary>
public abstract class ReskinSpawnable : Spawnable
{
    private GameObject cachedPrefab;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="classId">Class ID.</param>
    /// <param name="friendlyName">Name.</param>
    /// <param name="description">Tooltip.</param>
    protected ReskinSpawnable(string classId, string friendlyName, string description) : base(classId, friendlyName, description)
    {
        OnFinishedPatching += () =>
        {
            if (GetBlueprintRecipe() is not null)
                CraftDataHandler.SetTechData(TechType, GetBlueprintRecipe());
                
            if (CraftingTime > 0f)
                CraftDataHandler.SetCraftingTime(TechType, CraftingTime);

            var categories = new List<TechCategory>();
            CraftData.GetBuilderCategories(GroupForPDA, categories);
                
            if (categories.Contains(CategoryForPDA))
                CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);

            if (FabricatorType != CraftTree.Type.None)
            {
                if (StepsToFabricatorTab is null || StepsToFabricatorTab.Length is 0)
                    CraftTreeHandler.AddCraftingNode(FabricatorType, TechType);
                else
                    CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, StepsToFabricatorTab);
            }

            if (RequiredForUnlock != TechType.None)
                KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new[] { TechType });
        };
    }

    /// <summary>
    /// The original class id of this Item's Prefab.
    /// </summary>
    protected abstract string ReferenceClassId { get; }

    /// <summary>
    /// Allows you to customize this object.
    /// </summary>
    /// <param name="prefab"></param>
    protected abstract void ApplyChangesToPrefab(GameObject prefab);

    /// <summary>
    /// This provides the <see cref="TechData"/> instance used to designate how this item is crafted or constructed.
    /// </summary>
    protected virtual TechData GetBlueprintRecipe() => null;

    /// <summary>
    /// <para>Override with a custom crafting time for this item. Normal default crafting time is <c>1f</c>.</para>
    /// Any value zero or less will be ignored.
    /// </summary>
    public virtual float CraftingTime => 0f;

    /// <summary>
    /// Override with the main group in the PDa blueprints where this item appears.
    /// </summary>
    public virtual TechGroup GroupForPDA => TechGroup.Uncategorized;
        
    /// <summary>
    /// Override with the category within the group in the PDA blueprints where this item appears.
    /// </summary>
    public virtual TechCategory CategoryForPDA => TechCategory.Misc;

    /// <summary>
    /// <para>Override with the vanilla fabricator that crafts this.</para>
    /// Leave this as <see cref="CraftTree.Type.None"/> if you are manually adding this item to a custom fabricator.
    /// </summary>
    public virtual CraftTree.Type FabricatorType => CraftTree.Type.None;

    /// <summary>
    /// <para>Override with the tab node steps to take to get to the tab you want the item's blueprint to appear in.</para>
    /// If not overriden, the item will appear at the craft tree's root.
    /// </summary>
    public virtual string[] StepsToFabricatorTab => null;

    /// <summary>
    /// <para>Override to set the <see cref="TechType"/> that must first be scanned or picked up to unlock the blueprint for this item.</para>
    /// If not overriden, this item will not be unlocked at all.
    /// </summary>
    public virtual TechType RequiredForUnlock => TechType.None;

    public sealed override GameObject GetGameObject()
    {
        if (cachedPrefab == null)
        {
            PrefabDatabase.TryGetPrefab(ReferenceClassId, out GameObject prefab);
            cachedPrefab = GameObject.Instantiate(prefab);
            cachedPrefab.SetActive(false);
            cachedPrefab.EnsureComponent<TechTag>().type = TechType;
            cachedPrefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
            ApplyChangesToPrefab(cachedPrefab);
        }
        return cachedPrefab;
    }
}