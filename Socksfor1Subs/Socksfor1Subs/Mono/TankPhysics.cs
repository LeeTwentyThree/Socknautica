using UnityEngine;

namespace Socksfor1Subs.Mono
{
    public class TankPhysics : MonoBehaviour
    {
        public Tank tank;

        private Rigidbody _rb;
        private VFXConstructing _constructing;

        private void Start()
        {
            _constructing = gameObject.GetComponent<VFXConstructing>();
            _rb = gameObject.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rb.isKinematic = DetermineKinematic();
        }

        private bool DetermineKinematic()
        {
            if (!_constructing.IsConstructed())
            {
                return true;
            }
            if (tank.docked)
            {
                return true;
            }
            if (transform.localPosition.y > Ocean.main.GetOceanLevel())
            {
                return false;
            }
            if (Vector3.Distance(Player.main.transform.position, _rb.position) > 36f)
            {
                return true;
            }
            return false;
        }
    }
}
