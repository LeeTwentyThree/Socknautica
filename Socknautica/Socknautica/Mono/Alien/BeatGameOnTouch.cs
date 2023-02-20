using UnityEngine.SceneManagement;

namespace Socknautica.Mono.Alien;

internal class BeatGameOnTouch : MonoBehaviour
{
    private bool touched;
    private void Update()
    {
        if (touched) return;
        if (Vector3.Distance(Player.main.transform.position, transform.position) < 70f)
        {
            Touch();
        }
    }
    private void Touch()
    {
        SceneManager.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
        touched = true;
    }
}
