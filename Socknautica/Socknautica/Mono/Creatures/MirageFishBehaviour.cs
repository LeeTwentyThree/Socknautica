namespace Socknautica.Mono.Creatures;

internal class MirageFishBehaviour : MonoBehaviour
{
    private Creature creature;
    private Animator animator;
    private Rigidbody rb;
    private LastTarget lastTarget;
    private LiveMixin live;

    private bool luring;
    private float showDistance = 56;

    private float timeLureAgain;

    private MesmerizedScreenFXController mesmerFx;

    private void Start()
    {
        creature = GetComponent<Creature>();
        animator = gameObject.GetComponentInChildren<Animator>();
        lastTarget = gameObject.GetComponent<LastTarget>();
        live = gameObject.GetComponent<LiveMixin>();
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
        if (luring && Time.time > timeLureAgain)
        {
            transform.LookAt(Player.main.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            var distance = Vector3.Distance(transform.position, Player.main.transform.position);
            if (distance < showDistance)
            {
                JumpOut();
            }
        }
        else
        {
            lastTarget.target = Player.main.gameObject;
        }
        if (!luring && Time.time > timeLureAgain)
        {
            SetLureState(true);
        }
    }

    private void JumpOut()
    {
        timeLureAgain = Time.time + 14;
        SetLureState(false);
        Utils.PlayFMODAsset(Helpers.GetFmodAsset("AnglerJumpscare"), Player.main.transform.position);
    }

    private void KillPlayer()
    {
        Player.main.liveMixin.Kill();
    }

    private void OnDestroy()
    {
        if (mesmerFx) mesmerFx.StopHypnose();
    }

    public void SetLureState(bool state)
    {
        if (!luring && state)
        {
            PlayerScreenFXHelper.PlayScreenFX(PlayerScreenFXHelper.ScreenFX.Mesmer, 5f, 2f);
        }
        luring = state;
        animator.SetBool("lure", state);
        rb.isKinematic = state;
        live.invincible = state;
    }
}
