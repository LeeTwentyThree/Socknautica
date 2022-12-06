using ECCLibrary;

namespace Socknautica.Mono.Alien;

// for terminals that have voice lines
public class StoryHandTargetPlayAudio : MonoBehaviour
{
    public string fmodEvent;
    public string subtitlesKey;
    public float delay;

    private bool _playing;

    public void OnStoryHandTarget()
    {
        if (string.IsNullOrEmpty(fmodEvent))
        {
            return;
        }
        if (_playing)
        {
            return;
        }
        _playing = true;
        if (delay > 0f)
        {
            Invoke(nameof(Play), delay);
        }
        else
        {
            Play();
        }

    }

    void Play()
    {
        Utils.PlayFMODAsset(Helpers.GetFmodAsset(fmodEvent), MainCamera.camera.transform.position);
        if (!string.IsNullOrEmpty(subtitlesKey))
        {
            Subtitles.main.Add(subtitlesKey);
        }
        Destroy(this);
    }
}