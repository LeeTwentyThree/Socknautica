using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class MirageFishBehaviour : MonoBehaviour
{
    public GameObject fishModel;
    public GameObject baseModel;

    private Animator animator; 

    private bool shown;
    private float showDistance = 60;

    private void Start()
    {
        fishModel = transform.GetChild(0).gameObject;
        baseModel = transform.GetChild(1).gameObject;
        animator = fishModel.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        transform.LookAt(Player.main.transform.position);
        fishModel.SetActive(shown);
        baseModel.SetActive(!shown);

        if (!shown && Vector3.Distance(transform.position, Player.main.transform.position) < showDistance)
        {
            shown = true;
            animator.SetTrigger("bite");
            Invoke(nameof(KillPlayer), 2f);
        }
    }

    private void KillPlayer()
    {
        Player.main.liveMixin.Kill();
    }
}
