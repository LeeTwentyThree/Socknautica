using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class LoopingMusic : MonoBehaviour
{
    private FMODAsset asset;
    private FMOD_CustomEmitter emitter;
    private float duration;
    private float runtimeLeft;

    public static LoopingMusic Play(FMODAsset asset, float duration)
    {
        var music = new GameObject("LoopingMusicPlayer").AddComponent<LoopingMusic>();
        music.asset = asset;
        music.duration = duration;
        return music;
    }

    private void Start()
    {
        emitter = gameObject.EnsureComponent<FMOD_CustomEmitter>();
        emitter.SetAsset(asset);
        emitter.Play();
        runtimeLeft = duration;
    }

    private void Update()
    {
        if (Helpers.MusicIsPaused)
            return;
        if (runtimeLeft <= 0f)
        {
            emitter.Play();
            runtimeLeft = duration;
        }
        runtimeLeft -= Time.unscaledDeltaTime;
    }

    public void Stop()
    {
        Destroy(gameObject);
    }
}
