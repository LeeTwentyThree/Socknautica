using UnityEngine;

namespace Socksfor1Subs.Mono
{
    public class ExteriorLightsController : MonoBehaviour
    {
        public DadSubBehaviour sub;
        public Light[] lights;

        public float onIntensity = 3f;

        public Color defaultColor = Color.white;
        public Color stealthColor = new Color(1f, 0f, 0f);

        private void LateUpdate()
        {
            foreach (var l in lights)
            {
                l.intensity = onIntensity;
                if (sub.stealthManager.StealthEnabled)
                {
                    l.color = stealthColor;
                }
                else
                {
                    l.color = defaultColor;
                }
            }
        }
    }
}
