using UnityStandardAssets.ImageEffects;

namespace Socknautica.Mono;

internal class PlayerScreenFXHelper : MonoBehaviour
{
    private ScreenFX _screenFX;

    private float _duration;

    private float _startTime;

    private float _intensity;

    public static void PlayScreenFX(ScreenFX fx, float duration, float intensity = 1f)
    {
        var component = MainCamera.camera.gameObject.AddComponent<PlayerScreenFXHelper>();
        component._screenFX = fx;
        component._duration = duration;
        component._intensity = intensity;
    }

    public enum ScreenFX
    {
        Blur,
        Mesmer,
        Grayscale,
        Telepathy,
        WarperWarp,
        Teleport,
        GargTelepathy
    }

    private void Start()
    {
        _startTime = Time.time;
        SetFXState(true);
    }

    private void Update()
    {
        if (Time.time > _startTime + _duration)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        SetFXState(false);
    }

    private void SetFXState(bool enabled)
    {
        switch (_screenFX)
        {
            default:
                return;
            case ScreenFX.Blur:
                gameObject.GetComponent<RadialBlurScreenFXController>().SetAmount(enabled ? _intensity : 0f);
                return;
            case ScreenFX.Mesmer:
                var mesmerFx = gameObject.GetComponent<MesmerizedScreenFXController>();
                if (enabled) mesmerFx.StartHypnose();
                else mesmerFx.StopHypnose();
                return;
            case ScreenFX.Grayscale:
                var grayscale = gameObject.GetComponent<Grayscale>();
                grayscale.enabled = enabled;
                grayscale.effectAmount = enabled ? _intensity : 0f;
                return;
            case ScreenFX.Telepathy:
                var telepathy = gameObject.GetComponent<TelepathyScreenFXController>();
                if (enabled) telepathy.StartTelepathy(false);
                else telepathy.StopTelepathy();
                return;
            case ScreenFX.WarperWarp:
                var warper = gameObject.GetComponent<WarpScreenFXController>();
                if (enabled) warper.StartWarp();
                else warper.StopWarp();
                return;
            case ScreenFX.Teleport:
                var teleport = gameObject.GetComponent<TeleportScreenFXController>();
                if (enabled) teleport.StartTeleport();
                else teleport.StopTeleport();
                return;
        }
    }
}
