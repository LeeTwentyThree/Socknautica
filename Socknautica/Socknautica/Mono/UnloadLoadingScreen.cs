using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class UnloadLoadingScreen : MonoBehaviour
{
    private float timeStarted;

    private bool destroyed;

    private void Start()
    {
        timeStarted = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (destroyed || Time.realtimeSinceStartup < timeStarted + 5f)
        {
            return;
        }
        if (!uGUI.isLoading)
        {
            Destroy(gameObject, 5);
            destroyed = true;
        }
    }
}
