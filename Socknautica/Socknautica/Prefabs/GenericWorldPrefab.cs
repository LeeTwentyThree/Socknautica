using ECCLibrary;
using System.Collections;

namespace Socknautica.Prefabs;

/// <summary>
/// Made for Precursor base structures
/// </summary>
public class GenericWorldPrefab : Spawnable
{
    private readonly GameObject _model;
    protected GameObject prefab;
    private readonly UBERMaterialProperties _materialProperties;
    private readonly LargeWorldEntity.CellLevel _cellLevel;
    private readonly bool _applyPrecursorMaterial;

    public GenericWorldPrefab(string classId, string friendlyName, string description, GameObject model, UBERMaterialProperties materialProperties, LargeWorldEntity.CellLevel cellLevel, bool applyPrecursorMaterial = true) : base(classId, friendlyName, description)
    {
        _model = model;
        _materialProperties = materialProperties;
        _cellLevel = cellLevel;
        _applyPrecursorMaterial = applyPrecursorMaterial;
    }

    protected virtual void CustomizePrefab()
    {

    }

    public override GameObject GetGameObject()
    {
        if (prefab == null)
        {
            prefab = GameObject.Instantiate(_model);
            prefab.SetActive(false);
            prefab.EnsureComponent<LargeWorldEntity>().cellLevel = _cellLevel;
            prefab.EnsureComponent<PrefabIdentifier>().classId = ClassID;
            prefab.EnsureComponent<TechTag>().type = TechType;
            prefab.EnsureComponent<SkyApplier>().renderers = prefab.GetComponentsInChildren<Renderer>();
            var rb = prefab.EnsureComponent<Rigidbody>();
            rb.mass = 10000f;
            rb.isKinematic = true;
            prefab.EnsureComponent<ImmuneToPropulsioncannon>();
            MaterialUtils.ApplySNShaders(prefab, _materialProperties.Shininess, _materialProperties.SpecularInt, _materialProperties.EmissionScale);
            if (_applyPrecursorMaterial)
            {
                MaterialUtils.ApplyPrecursorMaterials(prefab, _materialProperties.SpecularInt);
            }
            CustomizePrefab();
            foreach (var col in prefab.GetComponentsInChildren<Collider>())
            {
                col.gameObject.EnsureComponent<ConstructionObstacle>();
            }
        }
        return prefab;
    }

    public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
    {
        gameObject.Set(GetGameObject());
        yield break;
    }
}