using System.IO;

namespace Socknautica;

internal static class SaveWithoutSaving
{
    public static bool GetBossDefeated()
    {
        return File.Exists(GetBossFile());
    }

    public static void OnDefeatBoss()
    {
        try { File.WriteAllText(GetBossFile(), "nice lol"); }
        catch { }
    }

    private static string GetSaveFolder()
    {
        var userStorage = PlatformUtils.main.userStorage;
        if (userStorage is UserStoragePC pc)
        {
            return Path.Combine(pc.savePath, SaveLoadManager.main.GetCurrentSlot());
        }
        return "";
    }

    private static string GetBossFile()
    {
        return Path.Combine(GetSaveFolder(), "boss-completion.txt");
    }
}
