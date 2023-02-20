using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica;

internal static class BossBalance
{
    public const float swimVelocity = 60f;
    public const float chargeVelocity = 120;
    public const float biteDamage = 50f;

    public const float fallbackSwimPriority = 0.01f;
    public const float swimPriority = 0.1f;
    public const float chargePriority = 1f;
    public const float fireEnegyBallPriority = 0.95f;
    public const float vomitPriority = 0.9f;
    public const float fireEnegyBallBallPriority = 0.95f;
}
