using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class PressureDamage : MonoBehaviour
{
    private float timeLastDamaged;
    private float dmg = 1;
    private float dmgInterval = 1;

    private void Update()
    {
        if (Time.time < timeLastDamaged + dmgInterval) return;
        if (transform.position.y < -2000 && VoidIslandBiome.bounds.Contains(transform.position))
        {
            Player.main.liveMixin.TakeDamage(dmg, default, DamageType.Normal);
            timeLastDamaged = Time.time;
        }
    }
}
