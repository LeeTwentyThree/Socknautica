using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Creatures;

internal class ChargeAtPlayer : CreatureAction
{
    private float minInterval = 45f;
    private float duration = 4f;
    private float velocity = BossBalance.chargeVelocity;
    private float priority = BossBalance.chargePriority;

    private float timeLastCharge;
    private bool performing;

    public override float Evaluate(Creature creature)
    {
        if (performing && Time.time <= timeLastCharge + duration)
        {
            return 1f;
        }
        if (Time.time > timeLastCharge + minInterval)
        {
            return priority;
        }
        return 0f;
    }

    public override void StartPerform(Creature creature)
    {
        performing = true;
        timeLastCharge = Time.time;
    }

    public override void Perform(Creature creature, float deltaTime)
    {
        swimBehaviour.SwimTo(Player.main.transform.position, velocity);
    }

    public override void StopPerform(Creature creature)
    {
        performing = false;
    }
}
