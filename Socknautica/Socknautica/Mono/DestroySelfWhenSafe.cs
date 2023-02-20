namespace Socknautica.Mono;

internal class DestroySelfWhenSafe : MonoBehaviour
{
    private void Update()
    {
        if (!Player.main.transform.IsChildOf(transform))
        {
            Destroy(gameObject);
        }
    }
}
