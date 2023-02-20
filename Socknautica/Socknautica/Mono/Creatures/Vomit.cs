namespace Socknautica.Mono.Creatures;

internal class Vomit : CreatureAction
{
    public Boss boss;
    private float minInterval = 16f;
    private float maxInterval = 35f;
    private float priority = BossBalance.vomitPriority;

    private float timeLastFire;
    private float timeFireAgain;

    private void Start()
    {
        timeFireAgain = Time.time + maxInterval;
    }

    public override float Evaluate(Creature creature)
    {
        return 0f;
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
            head.FireVomitGas();
        }
        boss.creature.GetAnimator().SetTrigger("vomit");
    }
}
