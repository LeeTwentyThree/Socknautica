using UnityEngine;

namespace Socksfor1Subs.Mono
{
    public class SolarCharger : MonoBehaviour
    {
		public DadSubBehaviour sub;
		public PowerSource powerSource;
		public PowerRelay relay;

		public float Efficiency
		{
            get
            {
				var temp = DayNightCycle.main.GetLocalLightScalar(); // normalized
                temp *= DepthBasedMultiplier;
                return Mathf.Clamp01(temp);
            }
		}

        public float Depth
        {
            get
            {
                var y = transform.position.y;
                if (y > Ocean.main.GetOceanLevel())
                {
                    return Ocean.main.GetOceanLevel();
                }
                return Mathf.Abs(y);
            }
        }

        public float DepthBasedMultiplier
        {
            get
            {
                return 1f - Mathf.Clamp01(Depth / Balance.MaxSolarDepth);
            }
        }

        private void Update()
		{
			powerSource.power = Mathf.Clamp(powerSource.power + (Efficiency * Time.deltaTime * Balance.DadSolarChargerIdealRate), 0f, powerSource.maxPower);
		}
	}
}
