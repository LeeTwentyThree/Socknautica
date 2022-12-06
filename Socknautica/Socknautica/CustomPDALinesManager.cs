using ECCLibrary;
using Story;

namespace Socknautica;

public static class CustomPDALinesManager
{
    // cannot be played multiple times. calls the story goal by id.
    public static void PlayGoalVoiceLine(string id, float delay = 0f)
    {
        new StoryGoal(id, Story.GoalType.PDA, delay).Trigger();
    }

    // can be played multiple times
    public static void PlayVoiceLine(string id)
    {
        PDASounds.queue.PlayQueued(id, id);
    }
}