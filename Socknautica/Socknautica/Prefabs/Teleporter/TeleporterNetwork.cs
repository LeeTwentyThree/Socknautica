namespace Socknautica.Prefabs.Teleporter;

public class TeleporterNetwork
{
    public string classIdRoot;
    public TeleporterPrimaryPrefab primaryTeleporter;
    public TeleporterFramePrefab auxiliaryTeleporter;
    public Vector3 masterCoords, auxiliaryCoords;
    public float masterAngle, auxiliaryAngle;
    public bool oneWay;

    public Vector3? masterOverrideEulers, auxiliaryOverrideEulers;
    public Vector3? masterOverrideSpawnPosition, auxiliaryOverrideSpawnPosition;
    public float? masterOverrideSpawnAngle, auxiliaryOverrideSpawnAngle;

    public const float kLargeTeleporterScale = 3f;

    public const LargeWorldEntity.CellLevel kLargeTeleporterCellLevel = LargeWorldEntity.CellLevel.Far;

    public TeleporterNetwork(string classIdRoot, Vector3 masterCoords, float masterAngle, Vector3 auxiliaryCoords, float auxiliaryAngle, string auxiliaryOverrideId = null)
    {
        this.classIdRoot = classIdRoot;
        this.masterCoords = masterCoords;
        this.auxiliaryCoords = auxiliaryCoords;
        this.masterAngle = masterAngle;
        this.auxiliaryAngle = auxiliaryAngle;
        primaryTeleporter = new TeleporterPrimaryPrefab(string.Format("{0}Primary", classIdRoot), classIdRoot, auxiliaryAngle);
        auxiliaryTeleporter = new TeleporterFramePrefab(string.Format("{0}Auxiliary", classIdRoot), classIdRoot, masterAngle, auxiliaryOverrideId);
    }

    public void Patch()
    {
        primaryTeleporter.Patch();
        auxiliaryTeleporter.Patch();
        primaryTeleporter.SetTeleportPosition(GetPlayerSpawnPosition(auxiliaryCoords, auxiliaryAngle, false));
        auxiliaryTeleporter.SetTeleportPosition(GetPlayerSpawnPosition(masterCoords, masterAngle, true));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new(primaryTeleporter.ClassID, masterCoords, Quaternion.Euler(masterOverrideEulers.HasValue ? masterOverrideEulers.Value : new Vector3(0f, masterAngle, 0f))));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new(auxiliaryTeleporter.ClassID, auxiliaryCoords, Quaternion.Euler(auxiliaryOverrideEulers.HasValue ? auxiliaryOverrideEulers.Value : new Vector3(0f, auxiliaryAngle, 0f))));
    }


    Vector3 GetPlayerSpawnPosition(Vector3 coords, float angle, bool master)
    {
        // check if the angle has been overriden anywhere
        if (master && masterOverrideSpawnAngle.HasValue) angle = masterOverrideSpawnAngle.Value;
        else if (!master && auxiliaryOverrideSpawnAngle.HasValue) angle = auxiliaryOverrideSpawnAngle.Value;

        // determine the direction the teleporter will face
        var forward = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;

        // check if the direction has been overriden anywhere
        bool amMasterWithNewRotation = master && masterOverrideEulers.HasValue;
        bool amAuxiliaryWithNewRotation = !master && auxiliaryOverrideEulers.HasValue;

        if (amMasterWithNewRotation) forward = EulersToForwardVector(masterOverrideEulers.Value);
        else if (amAuxiliaryWithNewRotation) forward = EulersToForwardVector(auxiliaryOverrideEulers.Value);

        // offset the player, move him closer to the center
        var up = Vector3.up;

        // make sure up is correct direction
        if (amMasterWithNewRotation)
        {
            up = EulersToDirection(masterOverrideEulers.Value, Vector3.up);
        }
        else if (amAuxiliaryWithNewRotation)
        {
            up = EulersToDirection(auxiliaryOverrideEulers.Value, Vector3.up);
        }

        return coords + (forward * 3f) + up;
    }


    private static Vector3 EulersToForwardVector(Vector3 eulers)
    {
        return Quaternion.Euler(eulers) * Vector3.forward;
    }

    private static Vector3 EulersToDirection(Vector3 eulers, Vector3 direction)
    {
        return Quaternion.Euler(eulers) * direction;
    }
}