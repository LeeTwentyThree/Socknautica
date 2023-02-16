using ECCLibrary;

namespace Socknautica.Mono;

internal class GenericRoar : MonoBehaviour
{
    public float soundFalloffStart;
    public float soundFalloffEnd;
    public float minInterval;
    public float maxInterval;
    public string clipPrefix;
    public string animationParameterName;

    public void SetEssentials(float soundFalloffStart, float soundFalloffEnd, float minInterval, float maxInterval, string clipPrefix, string animationParameterName)
    {
        this.soundFalloffStart = soundFalloffStart;
        this.soundFalloffEnd = soundFalloffEnd;
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.clipPrefix = clipPrefix;
        this.animationParameterName = animationParameterName;
    }

    private Creature creature;
    private ECCAudio.AudioClipPool clipPool;
    private AudioSource source;
    private float timeNextRoar;
    private Animator animator;

    private void Start()
    {
        creature = GetComponent<Creature>();
        animator = creature.GetAnimator();
        clipPool = ECCAudio.CreateClipPool(clipPrefix);
        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1;
        source.minDistance = soundFalloffStart;
        source.maxDistance = soundFalloffEnd;
        source.volume = ECCHelpers.GetECCVolume();
    }

    private void Update()
    {
        if (Time.time > timeNextRoar && creature != null && creature.liveMixin != null && creature.liveMixin.IsAlive())
        {
            RoarOnce();
        }
    }

    public void RoarOnce()
    {
        if (animator && !string.IsNullOrEmpty(animationParameterName)) animator.SetTrigger(animationParameterName);
        source.clip = clipPool.GetRandomClip();
        source.Play();
        TimeNextRoar();
    }

    private void TimeNextRoar()
    {
        timeNextRoar = Time.time + Random.Range(minInterval, maxInterval);
    }
}
