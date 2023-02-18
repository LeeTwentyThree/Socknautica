namespace Socknautica.Mono.Creatures;

internal class AbyssalMouthBehaviour : MonoBehaviour
{
    private GameObject currentlySpawned;
    private Bounds attackArea;
    private bool attackedThisSession;

    private void Update()
    {
        if (attackedThisSession)
        {
            return;
        }
    }

    private IEnumerator DoAttack()
    {

    }
}
