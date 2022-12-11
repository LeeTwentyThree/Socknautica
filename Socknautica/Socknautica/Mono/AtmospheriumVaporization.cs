using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class AtmospheriumVaporization : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y > -1500 && gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
}
