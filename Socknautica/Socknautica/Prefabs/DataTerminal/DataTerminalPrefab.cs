using SMLHelper.V2.Assets;
using UWE;
using Socknautica.Mono.Alien;

namespace Socknautica.Prefabs.DataTerminal;

public class DataTerminalPrefab : Spawnable
{
    public const string greenTerminalCID = "625d01c2-40b7-4c87-a1cc-493ad6101c34";
    public const string orangeTerminalCID = "dd3bf908-badb-4c8c-a195-eb50be09df63";
    public const string blueTerminalCID = "b629c806-d3cd-4ee4-ae99-7b1359b60049";

    GameObject _processedPrefab;

    DataTerminal _dataTerminal;

    public DataTerminalPrefab(string classId, DataTerminal dataTerminal)
        : base(classId, "Data terminal", ".")
    {
        _dataTerminal = dataTerminal;
    }

    public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
    {
        classId = ClassID,
        cellLevel = LargeWorldEntity.CellLevel.Medium,
        localScale = Vector3.one,
        slotType = EntitySlot.Type.Large,
        techType = this.TechType
    };

#if SN1
    public override GameObject GetGameObject()
    {
        if (_processedPrefab)
        {
            var go = Object.Instantiate(_processedPrefab);
            go.SetActive(true);

            return go;
        }

        PrefabDatabase.TryGetPrefab(_dataTerminal.TerminalClassId, out GameObject prefab);
        GameObject obj = Object.Instantiate(prefab);
        StoryHandTarget storyHandTarget = obj.GetComponent<StoryHandTarget>();
        if (!string.IsNullOrEmpty(_dataTerminal.StoryGoalSettings?.EncyKey))
        {
            storyHandTarget.goal = new Story.StoryGoal(_dataTerminal.StoryGoalSettings?.EncyKey, Story.GoalType.Encyclopedia, _dataTerminal.StoryGoalSettings.Delay);
        }
        else
        {
            storyHandTarget.goal = new Story.StoryGoal(null, Story.GoalType.Story, 0f);
        }

        if (_dataTerminal.Unlockables != null)
        {
            if (_dataTerminal.Unlockables.TechTypesToUnlock is { Length: > 0 })
            {
                var unlockTech = obj.EnsureComponent<DataTerminalUnlockTech>();
                unlockTech.techsToUnlock = _dataTerminal.Unlockables.TechTypesToUnlock;
                unlockTech.delay = _dataTerminal.Unlockables.Delay;
            }
            if (_dataTerminal.Unlockables.TechTypeToAnalyze is { Length: > 0 })
            {
                var analyzeTech = obj.EnsureComponent<DataTerminalAnalyzeTech>();
                analyzeTech.techsToAnalyze = _dataTerminal.Unlockables.TechTypeToAnalyze;
                analyzeTech.delay = _dataTerminal.Unlockables.Delay;
            }
        }

        if (!_dataTerminal.Interactable)
        {
            Object.DestroyImmediate(storyHandTarget);
        }

        if (_dataTerminal.PingClassIds is { Length: > 0 })
        {
            foreach (string str in _dataTerminal.PingClassIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    DataTerminalUnlockPing unlockPing = obj.AddComponent<DataTerminalUnlockPing>();
                    unlockPing.classId = str;
                    unlockPing.pingTypeName = str;
                }
            }
        }
        if (!string.IsNullOrEmpty(_dataTerminal.SoundSettings?.FmodEvent))
        {
            var playAudio = obj.AddComponent<StoryHandTargetPlayAudio>();
            playAudio.fmodEvent = _dataTerminal.SoundSettings?.FmodEvent;
            playAudio.subtitlesKey = _dataTerminal.SoundSettings?.SubtitlesKey;
        }
        if (!string.IsNullOrEmpty(_dataTerminal.LogSettings?.Key))
        {
            var playAudio = obj.AddComponent<StoryHandTargetPDALog>();
            playAudio.logKey = _dataTerminal.LogSettings?.Key;
        }
        EditFX(obj);
        CustomizePrefab(obj);
        _processedPrefab = Object.Instantiate(obj);
        _processedPrefab.SetActive(false);
        return obj;
    }
#else
        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            if (_processedPrefab)
            {
                var go = Object.Instantiate(_processedPrefab);
                go.SetActive(true);

                gameObject.Set(go);
                yield break;
            }

            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(terminalClassId);
            yield return request;
            request.TryGetPrefab(out GameObject prefab);

            GameObject obj = GameObject.Instantiate(prefab);
            StoryHandTarget storyHandTarget = obj.GetComponent<StoryHandTarget>();
            if (!string.IsNullOrEmpty(encyKey))
            {
                storyHandTarget.goal = new Story.StoryGoal(encyKey, Story.GoalType.Encyclopedia, delay);
            }
            else
            {
                storyHandTarget.goal = null;
            }
            if (disableInteraction)
            {
                Object.DestroyImmediate(storyHandTarget);
            }
            obj.SetActive(false);
            if (pingClassId is { Length: > 0 })
            {
                foreach (string str in pingClassId)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        DataTerminalUnlockPing unlockPing = obj.AddComponent<DataTerminalUnlockPing>();
                        unlockPing.classId = str;
                        unlockPing.pos = pingPosition;
                        unlockPing.pingTypeName = str;
                    }
                }
            }
            if (!string.IsNullOrEmpty(audioClipPrefix))
            {
                var playAudio = obj.AddComponent<StoryHandTargetPlayAudioClip>();
                playAudio.clipPrefix = audioClipPrefix;
                playAudio.subtitlesKey = subtitlesKey;
            }
            if (techToUnlock != null)
            {
                obj.AddComponent<DataTerminalUnlockTech>().techsToUnlock = techToUnlock;
            }
            if (techToAnalyze != TechType.None)
            {
                obj.AddComponent<DataTerminalAnalyzeTech>().techToUnlock = techToAnalyze;
            }
            if (!string.IsNullOrEmpty(achievement))
            {
                obj.EnsureComponent<DataTerminalAchievement>().achievement = achievement;
            }
            EditFX(obj);
            MakeChangesToPrefab(obj);

            _processedPrefab = GameObject.Instantiate(obj);
            _processedPrefab.SetActive(false);
            gameObject.Set(obj);
        }
#endif

    protected virtual void CustomizePrefab(GameObject prefab)
    {

    }

    private void EditFX(GameObject prefab)
    {
        GameObject fx = prefab.transform.GetChild(2).gameObject;
        if (_dataTerminal.FxSettings?.HideSymbol ?? false)
        {
            fx.transform.GetChild(3).gameObject.SetActive(false);
            fx.transform.GetChild(5).gameObject.SetActive(false);
        }

        var fxColor = _dataTerminal.FxSettings?.FxColor;
        if (fxColor != null)
        {
            foreach (Renderer renderer in fx.GetComponentsInChildren<Renderer>())
            {
                renderer.material.SetColor("_Color", fxColor.Value);
            }
        }
    }
}