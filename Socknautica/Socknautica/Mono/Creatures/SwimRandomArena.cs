using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Socknautica.Mono.Creatures;

public class SwimRandomArena : CreatureAction
{
	private float swimRadius = 150;
	private float swimVelocity = BossBalance.swimVelocity;
	private bool performing;
	private float currentAngle;

	public override void Perform(Creature creature, float deltaTime)
	{
		
	}

	private void Update()
    {
		if (performing)
        {
			Vector3 target = GetBossSwimTargetPosition();
			swimBehaviour.SwimTo(target, swimVelocity);
		}
		currentAngle = GetBossCurrentAngle();
    }

    public override void StartPerform(Creature creature)
    {
		performing = true;
    }

    public override void StopPerform(Creature creature)
    {
		performing = false;
    }

    public override float Evaluate(Creature creature)
    {
		if (ArenaSpawner.main != null) return BossBalance.swimPriority;
		return 0f;
    }

    private Vector3 GetBossSwimTargetPosition()
    {
		var angleRads = (GetBossCurrentAngle() + 10f) * Mathf.Deg2Rad;
		return new Vector3(Mathf.Cos(angleRads) * swimRadius, ArenaSpawner.main.center.position.y, Mathf.Sin(angleRads) * swimRadius);
    }

	private Vector2 GetBossSwimTargetPositionUnscaled()
	{
		var angleRads = (GetBossCurrentAngle() + 15f) * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(angleRads), Mathf.Sin(angleRads));
	}

	private float GetBossCurrentAngle()
    {
		var center = GetArenaCenterPosition();
		Vector2 direction = (new Vector2(transform.position.x, transform.position.z) - new Vector2(center.x, center.z)).normalized;
		var angle = Helpers.Angle(direction);
		//if (direction.y < 0) angle += 180;
		return angle;
    }

	private Vector3 GetArenaCenterPosition()
    {
		if (ArenaSpawner.main != null)
        {
			return ArenaSpawner.main.center.position;
        }
		return default;
    }

	private static float GetTangentToPoint(Vector2 unitCirclePoint)
    {
		if (Mathf.Approximately(unitCirclePoint.y, 0f))
        {
			return float.MaxValue;
        }
		return -unitCirclePoint.x / unitCirclePoint.y;
    }
}