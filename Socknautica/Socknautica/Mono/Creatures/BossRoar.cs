namespace Socknautica.Mono.Creatures;

internal class BossRoar : MonoBehaviour
{
    public Creature creature;
    public float timeLastRoar { get; private set; }

    private float timeRoarAgain;
    private float minInterval = 30f;
    private float maxInterval = 60f;

    private FMOD_CustomEmitter emitter;

    private void Start()
    {
        emitter = gameObject.AddComponent<FMOD_CustomEmitter>();
        emitter.SetAsset(Helpers.GetFmodAsset(""));
        emitter.followParent = true;
    }

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
        creature.GetAnimator().SetTrigger("roar");
        emitter.Play();
    }
}
