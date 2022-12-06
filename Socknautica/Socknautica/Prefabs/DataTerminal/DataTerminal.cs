using Socknautica.Interfaces;

namespace Socknautica.Prefabs.DataTerminal;

public struct DataTerminal : IDataTerminal
{
    public RSoundSettings SoundSettings { get; set; }
    public RLogSettings LogSettings { get; set; }
    public RFxSettings FxSettings { get; set; }
    public RStoryGoalSettings StoryGoalSettings { get; set; }
    public RUnlockables Unlockables { get; set; }

    public string[] PingClassIds { get; set; }

    public string TerminalClassId { get; set; }

    public bool Interactable { get; set; }
}