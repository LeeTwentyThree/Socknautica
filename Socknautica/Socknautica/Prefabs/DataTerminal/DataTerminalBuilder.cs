using Socknautica.Interfaces;

namespace Socknautica.Prefabs.DataTerminal;

public class DataTerminalBuilder : IDataTerminalBuilder
{
    DataTerminal _dataTerminal;

    public DataTerminalBuilder()
    {
        Reset();
    }

    public void SetupSound(string fmodEvent, string subtitlesKey, float delay = 0f)
    {
        _dataTerminal.SoundSettings = new RSoundSettings(fmodEvent, subtitlesKey, delay);
    }

    public void SetupLog(string key)
    {
        _dataTerminal.LogSettings = new RLogSettings(key);
    }

    public void SetupPingClassIds(string[] pingClassIds)
    {
        _dataTerminal.PingClassIds = pingClassIds;
    }

    public void SetupFX(Color? fxColor, bool hideSymbol)
    {
        _dataTerminal.FxSettings = new RFxSettings(fxColor, hideSymbol);
    }

    public void SetupStoryGoal(string encyKey, float delay = 5)
    {
        _dataTerminal.StoryGoalSettings = new RStoryGoalSettings(encyKey, delay);
    }

    public void SetupUnlockables(TechType[] techTypesToUnlock = null, TechType[] techTypesToAnalyze = null, float delay = 0)
    {
        _dataTerminal.Unlockables = new RUnlockables(techTypesToUnlock, techTypesToAnalyze, delay);
    }

    public void SetupTemplateTerminal(string terminalClassId)
    {
        _dataTerminal.TerminalClassId = terminalClassId;
    }

    public void SetupInteractable(bool interactable)
    {
        _dataTerminal.Interactable = interactable;
    }

    public DataTerminal GetTerminal()
    {
        var dataTerminal = _dataTerminal;

        Reset();

        return dataTerminal;
    }

    private void Reset()
    {
        _dataTerminal = new DataTerminal();
        SetupTemplateTerminal(DataTerminalPrefab.blueTerminalCID);
        SetupSound(null, null);
        SetupLog(null);
        SetupPingClassIds(null);
        SetupStoryGoal(null);
        SetupInteractable(true);
    }
}