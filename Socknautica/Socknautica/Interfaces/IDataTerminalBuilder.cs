namespace Socknautica.Interfaces;

using Prefabs.DataTerminal;
using UnityEngine;
    
public interface IDataTerminalBuilder
{
    /// <summary>
    /// A method that takes enough information to setup the SoundSettings
    /// </summary>
    /// <param name="fmodEvent">The FMOD event.</param>
    /// <param name="subtitlesKey">The key of the subtitle text</param>
    void SetupSound(string fmodEvent, string subtitlesKey, float delay = 0f);

    /// <summary>
    /// A method that takes enough information to setup the LogSettings
    /// </summary>
    /// <param name="key">The key of the PDA log</param>
    void SetupLog(string key);

    /// <summary>
    /// Setup PingType classIDs
    /// </summary>
    /// <param name="pingClassIds"></param>
    void SetupPingClassIds(string[] pingClassIds);
                        
    /// <summary>
    /// Setup Storygoal
    /// </summary>
    /// <param name="encyKey"></param>
    /// <param name="delay"></param>
    void SetupStoryGoal(string encyKey, float delay = 5);
                
    /// <summary>
    /// Setup Visual Effect
    /// </summary>
    /// <param name="fxColor"></param>
    /// <param name="hideSymbol"></param>
    void SetupFX(Color? fxColor, bool hideSymbol);
        
    /// <summary>
    /// Setup TechTypes that get unlocked and/or a TechType to analyze when interacted with the data terminal. 
    /// </summary>
    /// <param name="techTypesToUnlock"></param>
    /// <param name="techTypeToAnalyze"></param>
    void SetupUnlockables(TechType[] techTypesToUnlock, TechType[] techTypesToAnalyze, float delay);
        
    /// <summary>
    /// Template Data Terminal ClassID
    /// </summary>
    /// <param name="templateTerminalClassId"></param>
    void SetupTemplateTerminal(string templateTerminalClassId);

    /// <summary>
    /// Setup whether or not the DataTerminal should be interactable (whether should have <see cref="StoryHandTarget"/> or not.)
    /// </summary>
    /// <param name="interactable"></param>
    void SetupInteractable(bool interactable);

    /// <summary>
    /// Gets the current built terminal
    /// </summary>
    /// <returns></returns>
    DataTerminal GetTerminal();
}