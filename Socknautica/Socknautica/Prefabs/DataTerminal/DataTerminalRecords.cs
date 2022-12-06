namespace Socknautica.Prefabs.DataTerminal;

public record RSoundSettings(string FmodEvent, string SubtitlesKey, float Delay);

public record RLogSettings(string Key);

public record RFxSettings(Color? FxColor, bool HideSymbol);

public record RStoryGoalSettings(string EncyKey, float Delay);

public record RUnlockables(TechType[] TechTypesToUnlock, TechType[] TechTypeToAnalyze, float Delay);