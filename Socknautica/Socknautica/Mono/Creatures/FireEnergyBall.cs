namespace Socknautica.Mono.Creatures;

internal class FireEnergyBall : CreatureAction
{
    public Boss boss;
    private float minInterval = 10f;
    private float maxInterval = 15f;
    private float priority = BossBalance.fireEnegyBallPriority;

    private float timeLastFire;
    private float timeFireAgain;

    private void Start()
    {
        timeFireAgain = Time.time + maxInterval;
    }

    public override float Evaluate(Creature creature)
    {
        if (Time.time > timeFireAgain)
        {
            return priority;
        }
        return 0f;
    }

    public override void StartPerform(Creature creature)
    {
        timeLastFire = Time.time;
        timeFireAgain = Time.time + Random.Range(minInterval, maxInterval);
        boss.GetRandomHead().FireHomingEnergyBall();
        boss.creature.GetAnimator().SetTrigger("vomit");
        boss.PlayAttackSound();

    }
}
