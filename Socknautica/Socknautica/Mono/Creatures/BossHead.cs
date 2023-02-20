using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Creatures;

internal class BossHead : MonoBehaviour
{
    public Boss boss;
    public Transform jawTransform;

    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    public void FireHomingEnergyBall()
    {
        var ball = boss.SpawnEnergyBall(jawTransform.position, 3);
        ball.gameObject.AddComponent<HomingEnergyBall>();
        foreach (var r in ball.GetComponentsInChildren<Renderer>())
        {
            foreach (var m in r.materials)
            {
                m.SetColor(ShaderPropertyID._Color, new Color(1f, 0.5f, 0f));
            }
        }
        ball.gameObject.GetComponent<EnergyBallDamageInRange>().lifetime = 30;
    }

    public void FireBasicEnergyBall()
    {
        var ball = boss.SpawnEnergyBall(jawTransform.position, 7);
        ball.lifetime = 30;
        ball.gameObject.AddComponent<MineTravel>();
        var damage = ball.gameObject.GetComponent<EnergyBallDamageInRange>();
        damage.damageRadius = 25;
        damage.damage = 50;
        damage.destroyPylons = false;
    }

    public void FireVomitGas()
    {
        boss.SpawnVomit(jawTransform.position);
    }
}
