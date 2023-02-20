using System.Collections.Generic;
using UWE;
using Socknautica.Mono.Alien;

namespace Socknautica.Mono.Creatures;

internal class EnergyBallDamageInRange : MonoBehaviour
{
    public Boss boss;

    private List<LiveMixin> damagedTargets;

    public float damageRadius = 10;
    public float damage = 100;

    public float lifetime = 20f;

    private float timeSpawned;

    public bool destroyPylons = true;

    private void Start()
    {
        damagedTargets = new List<LiveMixin>();
        timeSpawned = Time.time;
    }

    private void Update()
    {
        if (Time.time > timeSpawned + lifetime)
        {
            Destroy(gameObject);
            return;
        }
        int num = UWE.Utils.OverlapSphereIntoSharedBuffer(transform.position, damageRadius);
        bool destroySelf = false;
        for (int i = 0; i < num; i++)
        {
            var go = UWE.Utils.sharedColliderBuffer[i].gameObject;
            var lm = go.GetComponentInParent<LiveMixin>();
            if (lm == boss.creature.liveMixin)
            {
                continue;
            }
            if (lm != null && !damagedTargets.Contains(lm))
            {
                lm.TakeDamage(damage);
                damagedTargets.Add(lm);
                destroySelf = true;
            }
            var pylon = go.GetComponent<EnergyPylonCharge>();
            if (destroyPylons && pylon != null)
            {
                pylon.Explode();
                destroySelf = true;
            }
        }
        if (destroySelf)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ArenaSpawner.SpawnSeamothExplosion(transform.position, 10f);
    }
}
