using Story;
using System.Collections.Generic;

namespace Socknautica;

internal static class KilledLeviathanTracker
{
    public static KillableLeviathanData[] killableLeviathans = new KillableLeviathanData[]
    {
        new KillableLeviathanData("Mirage", "AnglerFish", 0),
        new KillableLeviathanData("Abyssal Blaza", "AbyssalBlaza", 1),
        new KillableLeviathanData("Ancient Bloop", "AncientBloop", 2),
        new KillableLeviathanData("Sea Dragon", "ff43eacd-1a9e-4182-ab7b-aa43c16d1e53", 3),
        new KillableLeviathanData("Sea Emperor", new string[] { "ff43eacd-1a9e-4182-ab7b-aa43c16d1e53", "c9a28181-a0eb-4ae5-9fb8-ce57772980f1" }, 4),
        new KillableLeviathanData("Ghost Leviathan", new string[] { "54701bfc-bb1a-4a84-8f79-ba4f76691bef", "5ea36b37-300f-4f01-96fa-003ae47c61e5" }, 5),
        new KillableLeviathanData("Hydragargantuan Leviathan", "Multigarg", 6),
        new KillableLeviathanData("Reaper Leviathan", "f78942c3-87e7-4015-865a-5ae4d8bd9dcb", 7),
        new KillableLeviathanData("Reefback", "8d3d3c8b-9290-444a-9fea-8e5493ecd6fe", 8),
        new KillableLeviathanData("Sea Treader", "35ee775a-d54c-4e63-a058-95306346d582", 9),
    };

    public static void OnKill(GameObject anyCreature)
    {
        if (anyCreature == null) return;
        var identifier = anyCreature.GetComponent<PrefabIdentifier>();
        if (identifier == null || string.IsNullOrEmpty(identifier.ClassId)) return;
        var entry = GetLeviathanInfoByClassId(identifier.ClassId);
        if (entry != null)
        {
            ProcessDeath(entry);
        }
    }

    public static void ProcessDeath(KillableLeviathanData leviathanData)
    {
        var s = StoryGoalManager.main;
        if (s.OnGoalComplete(leviathanData.StoryGoal))
        {
            WarningUI.Show($"{leviathanData.name} defeated! New hologram unlocked.", Main.assetBundle.LoadAsset<Sprite>("killleviathan"), 6f);
        }
    }

    public static List<int> GetUnlockedIndices()
    {
        List<int> unlocked = new List<int>();
        unlocked.Add(6);
        foreach (var lev in killableLeviathans)
        {
            if (lev.indexInProjector != 6 && StoryGoalManager.main.IsGoalComplete(lev.StoryGoal))
            {
                unlocked.Add(lev.indexInProjector);
            }
        }
        return unlocked;
    }

    private static KillableLeviathanData GetLeviathanInfoByClassId(string classId)
    {
        foreach (var entry in killableLeviathans)
        {
            foreach (var cid in entry.classIds)
            {
                if (cid.Equals(classId))
                {
                    return entry;
                }
            }
        }
        return null;
    }
}
internal class KillableLeviathanData
{
    public string name;
    public string[] classIds;
    public int indexInProjector;

    public KillableLeviathanData(string name, string classId, int indexInProjector)
    {
        this.name = name;
        this.classIds = new string[] { classId };
        this.indexInProjector = indexInProjector;
    }

    public KillableLeviathanData(string name, string[] classIds, int indexInProjector)
    {
        this.name = name;
        this.classIds = classIds;
        this.indexInProjector = indexInProjector;
    }

    public string StoryGoal => "KillLeviathan_" + name;
}