using System.Linq;
using UnityEngine.UI;

namespace Socknautica.Mono;
internal class Timer : MonoBehaviour
{
    private Text text;

    public static void Show()
    {
        var timer = new GameObject("Timer").AddComponent<Timer>();

        timer.text = timer.gameObject.EnsureComponent<Text>();
        timer.text.font = FindObjectsOfType<Text>().First((t) => t.font != null).font;

        var fitter = timer.gameObject.EnsureComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        timer.transform.SetParent(uGUI.main.screenCanvas.transform, false);
        timer.text.canvas.overrideSorting = true;
        timer.gameObject.layer = 31;
        timer.transform.localPosition = new Vector3(0, 200, 0);
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private float startTime;

    private void Update()
    {
        text.text = (Time.time - startTime).ToString();
    }
}
