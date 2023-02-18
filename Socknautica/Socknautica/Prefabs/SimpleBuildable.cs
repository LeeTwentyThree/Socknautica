namespace Socknautica.Prefabs;

/// <summary>
/// This class helps make buildables require less code to implement
/// </summary>
public abstract class SimpleBuildable : Buildable
{
    private GameObject _prefab;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="classId"></param>
    /// <param name="friendlyName"></param>
    /// <param name="description"></param>
    protected SimpleBuildable(string classId, string friendlyName, string description) : base(classId, friendlyName, description)
    {
    }

    /// <summary>
    /// No need to override this
    /// </summary>
    /// <returns></returns>
    public sealed override GameObject GetGameObject()
    {
        if (_prefab != null)
        {
            return _prefab;
        }
        _prefab = new GameObject(ClassID);
        _prefab.SetActive(false);
        var model = GameObject.Instantiate(Model);
        model.transform.SetParent(_prefab.transform, false);
        model.transform.localPosition = Vector3.zero;
        _prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
        _prefab.EnsureComponent<TechTag>().type = TechType;
        _prefab.EnsureComponent<LargeWorldEntity>().cellLevel = CellLevel;
        var sky = _prefab.EnsureComponent<SkyApplier>();
        var con = _prefab.AddComponent<Constructable>();
        con.techType = TechType;
        con.model = model;
        ConstructableSettings.ApplyToConstructable(con, ConstructableSettings);
        foreach (var bounds in GetBounds)
        {
            _prefab.AddComponent<ConstructableBounds>().bounds = bounds;
        }
        ApplyChangesToPrefab(_prefab);
        sky.renderers = _prefab.GetComponentsInChildren<Renderer>();
        _prefab.SetActive(true);
        MaterialUtils.ApplySNShaders(_prefab, 8f, 1f, 1f);
        MaterialUtils.ApplyPrecursorMaterials(_prefab, 2f);
        return _prefab;
    }

    /// <summary>
    /// Apply any changes to this prefab, if necessary
    /// </summary>
    /// <param name="prefab"></param>
    public virtual void ApplyChangesToPrefab(GameObject prefab)
    {

    }

    /// <summary>
    /// Settins for this buildable's placement
    /// </summary>
    public abstract ConstructableSettings ConstructableSettings { get; }

    /// <summary>
    /// Loading distance for this object
    /// </summary>
    public abstract LargeWorldEntity.CellLevel CellLevel { get; }

    /// <summary>
    /// The model used for this buildable
    /// </summary>
    public abstract GameObject Model { get; }

    /// <summary>
    /// The bounding box(es) of this constructable.
    /// </summary>
    protected abstract OrientedBounds[] GetBounds { get; }
}
/// <summary>
/// Stores many constructable settings for buildables.
/// </summary>
public struct ConstructableSettings
{
    internal readonly bool AllowedInBase;
    internal readonly bool AllowedOutside;
    internal readonly bool AllowedInSub;
    internal readonly bool AllowedOnWall;
    internal readonly bool AllowedOnGround;
    internal readonly bool AllowedOnCeiling;
    internal readonly bool AllowedOnConstructables;
    internal readonly bool RotationEnabled;
    internal readonly bool ForceUpright;
    internal readonly float PlaceDefaultDistance;
    internal readonly float PlaceMinDistance;
    internal readonly float PlaceMaxDistance;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="allowedInBase"></param>
    /// <param name="allowedInSub"></param>
    /// <param name="allowedOutside"></param>
    /// <param name="allowedOnWall"></param>
    /// <param name="allowedOnGround"></param>
    /// <param name="allowedOnCeiling"></param>
    /// <param name="allowedOnConstructables"></param>
    /// <param name="rotationEnabled"></param>
    /// <param name="forceUpright"></param>
    /// <param name="placeDefaultDistance"></param>
    /// <param name="placeMinDistance"></param>
    /// <param name="placeMaxDistance"></param>
    public ConstructableSettings(bool allowedInBase, bool allowedInSub, bool allowedOutside, bool allowedOnWall, bool allowedOnGround, bool allowedOnCeiling, bool allowedOnConstructables, bool rotationEnabled = true, bool forceUpright = false, float placeDefaultDistance = 2f, float placeMinDistance = 1.2f, float placeMaxDistance = 5f)
    {
        AllowedInBase = allowedInBase;
        AllowedInSub = allowedInSub;
        AllowedOutside = allowedOutside;
        AllowedOnWall = allowedOnWall;
        AllowedOnGround = allowedOnGround;
        AllowedOnCeiling = allowedOnCeiling;
        AllowedOnConstructables = allowedOnConstructables;
        RotationEnabled = rotationEnabled;
        ForceUpright = forceUpright;
        PlaceDefaultDistance = placeDefaultDistance;
        PlaceMinDistance = placeMinDistance;
        PlaceMaxDistance = placeMaxDistance;
    }

    /// <summary>
    /// Applies all the properties in the <paramref name="settings"/> struct to the <paramref name="con"/> component.
    /// </summary>
    /// <param name="con"></param>
    /// <param name="settings"></param>
    public static void ApplyToConstructable(Constructable con, ConstructableSettings settings)
    {
        con.allowedInBase = settings.AllowedInBase;
        con.allowedOutside = settings.AllowedOutside;
        con.allowedInSub = settings.AllowedInSub;
        con.allowedOnWall = settings.AllowedOnWall;
        con.allowedOnGround = settings.AllowedOnGround;
        con.allowedOnCeiling = settings.AllowedOnCeiling;
        con.allowedOnConstructables = settings.AllowedOnConstructables;
        con.rotationEnabled = settings.RotationEnabled;
        con.forceUpright = settings.ForceUpright;
        con.placeMinDistance = settings.PlaceMinDistance;
        con.placeDefaultDistance = settings.PlaceDefaultDistance;
        con.placeMaxDistance = settings.PlaceMaxDistance;
    }
}