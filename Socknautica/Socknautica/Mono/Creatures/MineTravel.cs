namespace Socknautica.Mono.Creatures;

internal class MineTravel : MonoBehaviour
{
    private float mSpeed = 100;

    private void Start()
    {
        transform.LookAt(Player.main.transform);
    }

    private void Update()
    {
        transform.position += transform.forward * (mSpeed * Time.deltaTime);
    }
}
