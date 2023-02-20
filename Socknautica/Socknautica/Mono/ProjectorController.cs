using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;

namespace Socknautica.Mono;

internal class ProjectorController : HandTarget, IHandTarget
{
    private static ProjectorSaveData saveData { get; } = SaveDataHandler.Main.RegisterSaveDataCache<ProjectorSaveData>();

    private Transform projectionParent;

    private int selected;

    private string UniqueId
    {
        get
        {
            var identifier = gameObject.GetComponent<PrefabIdentifier>();
            if (identifier != null) return identifier.Id;
            return string.Empty;
        }
    }

    private void Start()
    {
        if (saveData == null || saveData.data == null) saveData.Load();
        if (saveData.data == null) saveData.data = new List<ProjectorSave>();

        projectionParent = transform.GetChild(0).GetChild(1);

        foreach (var renderer in projectionParent.gameObject.GetComponentsInChildren<Renderer>())
        {
            var materials = new Material[renderer.materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = MaterialUtils.StasisFieldMaterial;
                materials[i].color = new Color(0.54f, 1f, 0.54f);
            }
            renderer.materials = materials;
        }
        if (saveData.TryGet(UniqueId, out var found))
        {
            SetActive(found.selected);
        }
        else
        {
            SetActive(6);
        }
    }

    public void SetActive(int index)
    {
        for (int i = 0; i < projectionParent.childCount; i++)
        {
            projectionParent.GetChild(i).gameObject.SetActive(index == i);
        }
        if (saveData.TryGet(UniqueId, out var data))
        {
            data.selected = index;
        }
        else
        {
            saveData.data.Add(new ProjectorSave() { uniqueId = UniqueId, selected = index });
        }
        selected = index;
    }

    private void Update()
    {
        projectionParent.transform.localEulerAngles += Vector3.up * (36 * Time.deltaTime);
    }

    public void OnHandHover(GUIHand hand)
    {
        HandReticle.main.SetInteractText("Change display", false, HandReticle.Hand.Left);
    }

    public void OnHandClick(GUIHand hand)
    {
        var valid = KilledLeviathanTracker.GetUnlockedIndices();
        var currentIndex = valid.IndexOf(selected);
        if (currentIndex == -1)
        {
            SetActive(6);
            ErrorMessage.AddMessage("Error!");
            return;
        }
        var newIndex = currentIndex + 1;
        if (newIndex >= valid.Count) newIndex = 0;
        SetActive(valid[newIndex]);
    }
}

[FileName("projectorsavedata")]
internal class ProjectorSaveData : SaveDataCache
{
    public List<ProjectorSave> data;

    public bool TryGet(string classId, out ProjectorSave found)
    {
        found = null;
        if (data == null || string.IsNullOrEmpty(classId)) return false;
        foreach (var d in data)
        {
            if (d != null && d.uniqueId == classId)
            {
                found = d;
                return true;
            }
        }
        return false;
    }
}
[System.Serializable]
internal class ProjectorSave
{
    public string uniqueId;
    public int selected;
}