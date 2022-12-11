using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Socknautica.Mono.Creatures;

public class SwimRandomArena : CreatureAction
{
	public override void Perform(Creature creature, float deltaTime)
	{
		if (Time.time > timeNextSwim)
		{
			timeNextSwim = Time.time + swimInterval;
			Vector3 vector;
			if (onSphere)
			{
				vector = UnityEngine.Random.onUnitSphere;
			}
			else
			{
				vector = UnityEngine.Random.insideUnitSphere;
			}
			vector += transform.forward * swimForward;
			vector = Vector3.Scale(vector, swimRadius);
			float velocity = Mathf.Lerp(swimVelocity, 0f, creature.Tired.Value);
			Vector3 vector2 = transform.position + vector;
			LastScarePosition component = gameObject.GetComponent<LastScarePosition>();
			if (component != null && Time.time < component.lastScareTime + 3f)
			{
				Vector3 normalized = (component.lastScarePosition - transform.position).normalized;
				Debug.DrawLine(transform.position, component.lastScarePosition, Color.red);
				Debug.DrawLine(vector2, vector2 - normalized, Color.green);
				vector2 -= normalized;
			}
			swimBehaviour.SwimTo(vector2, velocity);
		}
	}

	private Vector3 swimRadius = new Vector3(400f, 135, 400);
	private float swimForward = 0.5f;
	private float swimVelocity = BossBalance.swimVelocity;
	private float swimInterval = 6f;
	private bool onSphere = true;

	private float timeNextSwim;
}