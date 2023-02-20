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

    public void SetText(string val)
    {
        gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = val;
    }

    public static WarningUI Show(string title, Sprite background, float duration)
    {
        if (currentInstance != null)
        {
            currentInstance.Hide();
        }
        currentInstance = Instantiate(prefabObject).EnsureComponent<WarningUI>();
        currentInstance.GetComponent<RectTransform>().SetParent(prefabObject.transform.parent);
        currentInstance.transform.localScale = Vector3.one;
        currentInstance.transform.localPosition = Vector3.up * 360;
        Destroy(currentInstance.GetComponent<uGUI_RadiationWarning>());
        currentInstance.transform.GetChild(0).gameObject.SetActive(true);
        currentInstance.GetComponentInChildren<UnityEngine.UI.Text>().text = title;
        currentInstance.GetComponentInChildren<UnityEngine.UI.Image>().sprite = background;
        currentInstance._endTime = Time.time + duration;
        return currentInstance;
    }
}