using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class MirageFishBehaviour : MonoBehaviour
{
    private Animator animator; 

    private bool luring;
    private float showDistance = 60;

    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (luring) transform.LookAt(Player.main.transform.position);

        /*if (!luring && Vector3.Distance(transform.position, Player.main.transform.position) < showDistance)
        {
            luring = true;
            animator.SetTrigger("bite");
            Invoke(nameof(KillPlayer), 2f);
        }
        */
    }

    private void KillPlayer()
    {
        Player.main.liveMixin.Kill();
    }
}
