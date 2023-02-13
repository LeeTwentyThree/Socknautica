using Socknautica.Patches;

namespace Socknautica.Mono;

internal class UnloadLoadingScreen : MonoBehaviour
{
    private float timeStarted;

    private bool destroyed;

    private void Start()
    {
        timeStarted = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (destroyed || Time.realtimeSinceStartup < timeStarted + 5f)
        {
            return;
        }
        if (!uGUI.isLoading)
        {
            Destroy(gameObject, 1.5f);
            destroyed = true;
        }
    }

    private void OnDestroy()
    {
        var bundle = LoadingPatches.loadingScreensBundle;
        if (bundle != null) bundle.Unload(true);
    }
}
