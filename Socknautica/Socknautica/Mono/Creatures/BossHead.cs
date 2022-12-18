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
        var ball = gameObject.GetComponent<Boss>().SpawnEnergyBall(jawTransform.position);
        ball.gameObject.AddComponent<HomingEnergyBall>();
    }

    public void FireStationaryEnergyBall()
    {
        var ball = gameObject.GetComponent<Boss>().SpawnEnergyBall(jawTransform.position);
        ball.lifetime = 120;
    }
}
