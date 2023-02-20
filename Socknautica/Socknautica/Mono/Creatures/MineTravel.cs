namespace Socknautica.Mono.Creatures;

internal class MineTravel : MonoBehaviour
{
    private float mSpeed = 200;

    private float timeSpawned;

    private void Start()
    {
        transform.LookAt(Player.main.transform);
        timeSpawned = Time.time;
        transform.Rotate(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)));
    }

    private void Update()
    {
        if (Time.time < timeSpawned + 0.5f) return;
        transform.position += transform.forward * (mSpeed * Time.deltaTime);
    }
}
