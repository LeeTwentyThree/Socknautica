namespace Socknautica.Mono.Creatures;

internal class BossRoar : MonoBehaviour
{
    public float timeLastRoar { get; private set; }

    private float timeRoarAgain;
    private float minInterval = 30f;
    private float maxInterval = 60f;

    private void Update()
    {
        if (Time.time > timeRoarAgain)
        {
            Roar();
        }
    }

    public void Roar()
    {
        timeLastRoar = Time.time;
        timeRoarAgain = Time.time + Random.Range(minInterval, maxInterval);
        ErrorMessage.AddMessage("rawr");
    }
}
