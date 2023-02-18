using UnityEngine;
using ECCLibrary;

namespace Socknautica.Mono.Creatures
{
    public class AnglerMeleeAttack : MeleeAttack
    {
        private AudioSource attackSource;
        private ECCAudio.AudioClipPool biteClipPool;

        private float playerDamage = 110;
        private float vehicleDamage = 1000;
        private float subDamage = 500;

        void Start()
        {
            attackSource = gameObject.AddComponent<AudioSource>();
            attackSource.minDistance = 10f;
            attackSource.maxDistance = 40f;
            attackSource.spatialBlend = 1f;
            attackSource.volume = ECCHelpers.GetECCVolume();
            biteClipPool = ECCAudio.CreateClipPool("AbyssalBlazaBite");
            gameObject.SearchChild("MouthTrigger").GetComponent<OnTouch>().onTouch = new OnTouch.OnTouchEvent();
            gameObject.SearchChild("MouthTrigger").GetComponent<OnTouch>().onTouch.AddListener(OnTouch);
        }

        public override void OnTouch(Collider collider)
        {
            if (frozen)
            {
                return;
            }
            if (liveMixin.IsAlive() && Time.time > timeLastBite + biteInterval)
            {
                GameObject target = GetTarget(collider);
                Player player = target.GetComponent<Player>();
                if (player != null)
                {
                    if (!player.CanBeAttacked() || !player.liveMixin.IsAlive() || player.cinematicModeActive)
                    {
                        return;
                    }
                }
                LiveMixin liveMixin = target.GetComponent<LiveMixin>();
                if (liveMixin == null) return;
                if (!liveMixin.IsAlive())
                {
                    return;
                }
                liveMixin.TakeDamage(GetBiteDamage(target));
                timeLastBite = Time.time;
                attackSource.clip = biteClipPool.GetRandomClip();
                attackSource.Play();
                creature.GetAnimator().SetTrigger("bite");
                GetComponent<MirageFishBehaviour>().SetLureState(true);
            }
        }

        private float GetBiteDamage(GameObject target)
        {
            if (target.GetComponent<SubRoot>() != null)
            {
                return subDamage;
            }
            if (target.GetComponent<Player>() != null)
            {
                return playerDamage;
            }
            if (target.GetComponent<Vehicle> != null)
            {
                return vehicleDamage;
            }
            return 100;
        }

        public void OnVehicleReleased()
        {
            timeLastBite = Time.time;
        }
    }
}
