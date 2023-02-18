namespace Socknautica.Mono.Creatures;

internal class HuntDownPlayer : MonoBehaviour
{
    private SwimBehaviour sb;
    public float velocity = 20f;

    private void Start()
    {
        sb = GetComponent<SwimBehaviour>();
    }

    private void Update()
    {
        sb.SwimTo(Player.main.transform.position, velocity);
    }
}
