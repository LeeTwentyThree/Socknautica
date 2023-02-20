using System.Collections;
using System.Collections.Generic;

namespace Socknautica.Mono.Creatures;

internal class AbyssalMouthBehaviour : MonoBehaviour
{
    private GameObject currentlySpawned;
    private Vector3 attackCenter = Main.arenaTeleporterPos;
    private float minAttackY = -3500;
    private float maxAttackY = -1800;
    private float min2DDistance = 80;
    private float max2DDistance = 500;
    private bool attackedThisSession;

    private float timeLastAttack = -9999;
    private float attackInterval = 20;

    private float scaleFactor = 3f;

    private void Update()
    {
        if (currentlySpawned != null)
        {
            ClearArea();
            return;
        }
        if (Time.time < timeLastAttack + attackInterval)
        {
            return;
        }
        if (uGUI.isLoading) return;
        if (InRange())
        {
            if (Player.main.GetCurrentSub() != null && Player.main.IsPiloting())
                StartAttack();
        }
        else if (attackedThisSession)
        {
            attackedThisSession = false;
        }
    }

    private void StartAttack()
    {
        attackedThisSession = true;
        timeLastAttack = Time.time;
        StartCoroutine(DoAttack());
    }

    private void ClearArea()
    {
        var sphereCast = UWE.Utils.OverlapSphereIntoSharedBuffer(transform.position, 90f);
        for (int i = 0; i < sphereCast; i++)
        {
            var col = UWE.Utils.sharedColliderBuffer[i];
            var c = col.GetComponentInParent<Creature>();
            if (c != null && c.liveMixin != null)
            {
                c.liveMixin.Kill();
            }
            var island = col.GetComponentInParent<Island>();
            if (island != null)
            {
                Destroy(island.gameObject);
            }
        }
    }

    private bool InRange()
    {
        var playerPos = Player.main.transform.position;
        if (playerPos.y > maxAttackY || playerPos.y < minAttackY) return false;
        var playerPos2D = new Vector2(playerPos.x, playerPos.z);
        var attackArea2D = new Vector2(attackCenter.x, attackCenter.z);
        var distance = Vector2.Distance(attackArea2D, playerPos2D);
        if (distance < min2DDistance || distance > max2DDistance) return false;
        return true;
    }

    private IEnumerator DoAttack()
    {
        currentlySpawned = Instantiate(Main.assetBundle.LoadAsset<GameObject>("AbyssalMouth_Prefab"));
        MaterialUtils.ApplySNShaders(currentlySpawned, 8, 6, 2);
        currentlySpawned.EnsureComponent<SkyApplier>();
        var cam = MainCamera.camera.transform;
        currentlySpawned.transform.position = cam.transform.position + Helpers.Flatten(cam.transform.forward) * (100 * scaleFactor);
        currentlySpawned.transform.LookAt(Player.main.transform.position);
        currentlySpawned.transform.localScale = Vector3.one * scaleFactor;

        var target = GetTarget();
        yield return new WaitForSeconds(3f);
        var emitter = currentlySpawned.AddComponent<FMOD_CustomEmitter>();
        emitter.SetAsset(Helpers.GetFmodAsset("MassiveLeviathanIdle"));
        emitter.followParent = true;
        emitter.Play();
        yield return new WaitForSeconds(30f);
        currentlySpawned.GetComponentInChildren<Animator>().SetTrigger("death");
        yield return new WaitForSeconds(2f);
        if (target != null)
        {
            var rb = target.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
            target.transform.parent = currentlySpawned.Search("CineAttach").transform;
            var lm = target.GetComponent<LiveMixin>();
            lm.TakeDamage(9999f);
            var sub1 = target.GetComponent<SubRoot>();
            if (sub1 != Player.main.GetCurrentSub())
            {
                sub1.gameObject.AddComponent<DestroySelfWhenSafe>();
            }
            if (sub1 == Player.main.GetCurrentSub() && !Player.main.isPiloting)
            {
                Player.main.liveMixin.Kill();
            }
            var powerRelay = target.GetComponent<PowerRelay>();
            if (powerRelay)
            {
                powerRelay.ConsumeEnergy(9999, out float _);
            }
        }
        var sub = target.GetComponent<SubRoot>();
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(1);
            if (sub != null && Player.main.GetCurrentSub() == sub) Player.main.liveMixin.TakeDamage(4, transform.position, DamageType.Acid);
        }
        if (sub != null && Player.main.GetCurrentSub() == sub)
        {
            Player.main.liveMixin.Kill();
        }

        currentlySpawned.GetComponentInChildren<Animator>().SetTrigger("lose_interest");
        yield return new WaitForSeconds(5f);
        currentlySpawned.AddComponent<DestroySelfWhenSafe>();
    }

    private GameObject GetTarget()
    {
        if (Player.main.GetCurrentSub() != null)
        {
            return Player.main.GetCurrentSub().gameObject;
        }
        return null;
    }
}
