namespace Socknautica.Mono.Creatures;

internal class FireEnergyMines : CreatureAction
{
    public Boss boss;
    private float minInterval = 20f;
    private float maxInterval = 33f;
    private float priority = BossBalance.fireEnegyBallBallPriority;

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
        foreach (var head in boss.heads)
        {
            head.FireBasicEnergyBall();
        }
        boss.creature.GetAnimator().SetTrigger("vomit");
        boss.PlayAttackSound();

    }
}
