using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class ReactorDonutSpin : MonoBehaviour
{
    private float speed = 10f;
    public bool reverse;
    private float defaultY;
    private float hoverSpeed = 1f;
    private float hoverDist = 3f;

    private void Start()
    {
        defaultY = transform.localPosition.y;
        if (reverse) speed *= -1;
    }

    private void Update()
    {
        var distToCenter = Vector3.Distance(transform.position, ArenaSpawner.main.center.position);
        transform.localEulerAngles += Vector3.up * speed * distToCenter * Time.deltaTime;
        transform.localPosition = new Vector3(0, defaultY + Mathf.Cos(Time.time * hoverSpeed) * hoverDist, 0);
    }
}
