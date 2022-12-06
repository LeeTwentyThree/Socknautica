using ECCLibrary;

namespace Socknautica.Mono.Alien;

// for terminals that have voice lines
public class StoryHandTargetPDALog : MonoBehaviour
{
    public string logKey;

    public void OnStoryHandTarget()
    {
        if (string.IsNullOrEmpty(logKey))
        {
            return;
        }
        CustomPDALinesManager.PlayGoalVoiceLine(logKey);
        Destroy(this);
    }
}