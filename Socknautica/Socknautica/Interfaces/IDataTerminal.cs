namespace Socknautica.Interfaces;

using Prefabs.DataTerminal;
    
public interface IDataTerminal
{
    RSoundSettings SoundSettings { get; set; }

    RLogSettings LogSettings { get; set; }
        
    RFxSettings FxSettings { get; set; }
        
    RStoryGoalSettings StoryGoalSettings { get; set; }
        
    RUnlockables Unlockables { get; set; }

    string[] PingClassIds { get; set; }
         
    string TerminalClassId { get; set; }
        
    bool Interactable { get; set; }
}