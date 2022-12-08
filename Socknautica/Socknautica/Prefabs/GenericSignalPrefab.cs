using System.Collections.Generic;
using UWE;
using Socknautica.Mono;

namespace Socknautica.Prefabs;

public class GenericSignalPrefab : Spawnable
{
    public static List<PingType> registeredPingTypes = new List<PingType>();
    PingType pingType;
    Vector3 position;
    int defaultColorIndex;
    string pingTypeName;
    string labelKey;

    /// <summary>
    /// Constructor for a generic signal.
    /// </summary>
    /// <param name="classId">Must be unique.</param>
    /// <param name="textureName"></param>
    /// <param name="displayName">Shows up in the beacon manager only.</param>
    /// <param name="label">Shows up in the HUD.</param>
    /// <param name="position"></param>
    /// <param name="defaultColorIndex"></param>
    /// <param name="voiceLineSettings">Settings related to the voice line that plays when approaching the signal. By default does no voice line.</param>
    public GenericSignalPrefab(string classId, string textureName, string displayName, string label, Vector3 position, int defaultColorIndex = 0)
        : base(classId, displayName, ".")
    {
        this.pingTypeName = classId;
        this.defaultColorIndex = defaultColorIndex;
        this.position = position;
        OnFinishedPatching = () =>
        {
            Atlas.Sprite pingSprite = ImageUtils.LoadSpriteFromTexture(Main.assetBundle.LoadAsset<Texture2D>(textureName));
            pingType = PingHandler.RegisterNewPingType(pingTypeName, pingSprite);
            registeredPingTypes.Add(pingType);
            LanguageHandler.SetLanguageLine(pingTypeName, displayName);

            labelKey = $"Label_{pingTypeName}";
            LanguageHandler.SetLanguageLine(labelKey, label);
        };
    }

    public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
    {
        classId = ClassID,
        cellLevel = LargeWorldEntity.CellLevel.Global,
        localScale = Vector3.one,
        slotType = EntitySlot.Type.Small,
        techType = TechType
    };

    public override GameObject GetGameObject()
    {
        GameObject obj = new GameObject(ClassID);
        obj.SetActive(false);

        obj.EnsureComponent<PrefabIdentifier>().classId = ClassID;
        obj.EnsureComponent<TechTag>().type = TechType;

        PingInstance ping = obj.EnsureComponent<PingInstance>();
        ping.pingType = pingType;
        ping.origin = obj.transform;

        SphereCollider trigger = obj.EnsureComponent<SphereCollider>(); //if you enter this trigger the ping gets disabled
        trigger.isTrigger = true;
        trigger.radius = 20f;

        var rb = obj.EnsureComponent<Rigidbody>();
        rb.isKinematic = true;

        SignalPing signalPing = obj.EnsureComponent<SignalPing>(); //basically to enable the disable on approach
        signalPing.pingInstance = ping;
        signalPing.disableOnEnter = true;

        var delayedInit = obj.EnsureComponent<SignalPingDelayedInitialize>(); //to override the serializer
        delayedInit.position = position;
        delayedInit.label = labelKey;
        delayedInit.pingTypeName = pingTypeName;
        delayedInit.colorIndex = defaultColorIndex;

        obj.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
        obj.SetActive(true);
        return obj;
    }
}