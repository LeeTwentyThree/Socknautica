namespace Socknautica.Mono;

internal class WarningUI : MonoBehaviour
{
    public static GameObject prefabObject;

    public static WarningUI currentInstance;

    private float _endTime;

    private void Update()
    {
        if (Time.time > _endTime)
        {
            Hide();
        }
    }

    public void ExtendForSeconds(float secondsStartingNow = 1f)
    {
        _endTime = Time.time + secondsStartingNow;
    }

    public void Hide()
    {
        Destroy(gameObject);
    }

    public static WarningUI Show(string title, Sprite background, float duration)
    {
        if (currentInstance != null) currentInstance.Hide();
        currentInstance = Instantiate(prefabObject).EnsureComponent<WarningUI>();
        DestroyImmediate(currentInstance.GetComponent<uGUI_RadiationWarning>());
        currentInstance.GetComponentInChildren<UnityEngine.UI.Text>().text = title;
        currentInstance.GetComponentInChildren<UnityEngine.UI.Image>().sprite = background;
        currentInstance._endTime = duration;
        return currentInstance;
    }
}