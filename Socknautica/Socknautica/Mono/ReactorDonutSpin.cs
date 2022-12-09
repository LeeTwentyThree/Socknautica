using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class ReactorDonutSpin : MonoBehaviour
{
    private float speed = 5f;
    public bool reverse;

    private void Start()
    {
        if (reverse) speed *= -1;
    }

    private void Update()
    {
        var distToCenter = Vector3.Distance(transform.position, ArenaSpawner.main.center.position);
        transform.localEulerAngles += Vector3.up * speed * distToCenter * Time.deltaTime;
    }
}
