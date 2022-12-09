using ECCLibrary;
using UWE;

namespace Socknautica.Prefabs.Teleporter;

public class TeleporterFramePrefab : Spawnable
{
    const string referenceClassId = "f0429c44-a387-42e6-b621-74ba4dd8c2da";
    private string teleporterId;
    private Vector3 teleportPosition;
    private float teleportAngle;
    private bool disablePillarModels;
    private string overrideId;

    public TeleporterFramePrefab(string classId, string teleporterId, float teleportAngle, string overrideId) : base(classId, "", "")
    {
        this.teleporterId = teleporterId;
        this.teleportAngle = teleportAngle;
        this.overrideId = overrideId;
    }

    public void DisablePillarModels(bool disabled = true)
    {
        disablePillarModels = disabled;
    }

    public void SetTeleportPosition(Vector3 teleportPosition)
    {
        this.teleportPosition = teleportPosition;
    }

    public override GameObject GetGameObject()
    {
        PrefabDatabase.TryGetPrefab(referenceClassId, out GameObject prefab);
        GameObject obj = GameObject.Instantiate(prefab);

        obj.SetActive(false);
        obj.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
        var teleporter = obj.GetComponent<PrecursorTeleporter>();
        teleporter.teleporterIdentifier = string.IsNullOrEmpty(overrideId) ? teleporterId : overrideId;
        teleporter.warpToPos = teleportPosition;
        teleporter.warpToAngle = teleportAngle;
        var meshParent = GameObjectExtensions.SearchChild(obj, "Meshes").transform;
        if (disablePillarModels)
        {
            meshParent.GetChild(0).gameObject.SetActive(false);
            meshParent.GetChild(1).gameObject.SetActive(false);
            meshParent.GetChild(2).gameObject.SetActive(false);
            meshParent.GetChild(3).gameObject.SetActive(false);
        }

        var skyApplier = teleporter.GetComponent<SkyApplier>();
        skyApplier.anchorSky = Skies.Custom;
        skyApplier.customSkyPrefab = MaterialUtils.PrecursorAntechamberSky;
        return obj;
    }

    public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
    {
        classId = ClassID,
        cellLevel = LargeWorldEntity.CellLevel.Medium,
        localScale = Vector3.one,
        slotType = EntitySlot.Type.Large,
        techType = TechType.PrecursorTeleporter
    };

    protected override void ProcessPrefab(GameObject go)
    {
        go.name = ClassID;
        //dont override techtype
    }
}