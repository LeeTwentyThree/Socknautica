using UWE;

namespace Socknautica.Prefabs.Teleporter;

public class TeleporterPrimaryPrefab : Spawnable
{
    const string referenceClassId = "63e69987-7d34-41f0-aab9-1187ea06e740";
    private TeleporterFramePrefab frame;

    public TeleporterPrimaryPrefab(string classId, string teleporterId, float teleportAngle) : base(classId, "", "")
    {
        frame = new TeleporterFramePrefab(string.Format("{0}Frame", classId), teleporterId, teleportAngle, null);
        OnFinishedPatching += () =>
        {
            frame.Patch();
        };
    }

    public void SetTeleportPosition(Vector3 teleportPosition)
    {
        frame.SetTeleportPosition(teleportPosition);
    }

    public override GameObject GetGameObject()
    {
        PrefabDatabase.TryGetPrefab(referenceClassId, out GameObject prefab);
        GameObject obj = GameObject.Instantiate(prefab);

        obj.SetActive(false);
        var prefabPlaceholder = obj.GetComponent<PrefabPlaceholdersGroup>();
        prefabPlaceholder.prefabPlaceholders[0].prefabClassId = frame.ClassID;

        return obj;
    }

    public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
    {
        classId = ClassID,
        cellLevel = LargeWorldEntity.CellLevel.Medium,
        localScale = Vector3.one,
        slotType = EntitySlot.Type.Large,
        techType = this.TechType
    };
}