namespace Socknautica.Mono.Creatures;

internal class MirageFishBehaviour : MonoBehaviour
{
    private Creature creature;
    private Animator animator;
    private Rigidbody rb;

    private bool luring;
    private float hypnoseDistance = 90;
    private float showDistance = 70;

    private float timeLureAgain;

    private MesmerizedScreenFXController mesmerFx;

    private void Start()
    {
        creature = GetComponent<Creature>();
        animator = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        mesmerFx = gameObject.GetComponent<MesmerizedScreenFXController>();
        SetLureState(true);
    }

    private void Update()
    {
        if (creature == null || creature.liveMixin == null || !creature.liveMixin.IsAlive())
        {
            SetLureState(false);
            return;
        }
        if (luring)
        {
            transform.LookAt(Player.main.transform.position);
            var distance = Vector3.Distance(transform.position, Player.main.transform.position);
            if (distance < showDistance)
            {
                JumpOut();
            }
        }
        if (!luring && Time.time > timeLureAgain)
        {
            SetLureState(true);
        }
    }

    private void JumpOut()
    {
        timeLureAgain = Time.time + 30;
        SetLureState(false);
    }

    private void KillPlayer()
    {
        Player.main.liveMixin.Kill();
    }

    private void OnDestroy()
    {
        if (mesmerFx) mesmerFx.StopHypnose();
    }

    private void SetLureState(bool state)
    {
        luring = state;
        animator.SetBool("lure", state);
        rb.isKinematic = state;
        PlayerScreenFXHelper.PlayScreenFX(PlayerScreenFXHelper.ScreenFX.Mesmer, 5f, 2f);
    }
}
