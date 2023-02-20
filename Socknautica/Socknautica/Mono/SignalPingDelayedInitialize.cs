using System.Collections.Generic;

namespace Socknautica.Mono;

public class SignalPingDelayedInitialize : MonoBehaviour
{
    public Vector3 position;
    public string pingTypeName;
    public string label;
    public int colorIndex;

    public static List<SignalPingDelayedInitialize> spawnedPings = new List<SignalPingDelayedInitialize>();

    public void SetData(string label, int colorIndex)
    {
        this.label = label;
        this.colorIndex = colorIndex;
    }

    void Start()
    {
        Refresh();
        spawnedPings.Add(this);
    }

    void Refresh()
    {
        var signal = GetComponent<SignalPing>();
        signal.pos = position;
        transform.position = position;
        signal.descriptionKey = label;
        signal.UpdateLabel();
        PingInstance ping = GetComponent<PingInstance>();
        if (ping)
        {
            ping.SetColor(colorIndex);
        }
    }

    void OnDestroy()
    {
        spawnedPings.Remove(this);
    }
}