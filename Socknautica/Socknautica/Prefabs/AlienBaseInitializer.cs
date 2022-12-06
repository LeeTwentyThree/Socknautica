using System.Collections.Generic;
using Socknautica.Mono.Alien;
using UWE;

namespace Socknautica.Prefabs;

class AlienBaseInitializer<T> : Spawnable where T : AlienBaseSpawner
{

    private LargeWorldEntity.CellLevel cellLevel;
    Vector3 position;

    public AlienBaseInitializer(string classId, Vector3 coords, LargeWorldEntity.CellLevel cellLevel = LargeWorldEntity.CellLevel.Medium)
        : base(classId, ".", ".")
    {
        this.cellLevel = cellLevel;
        position = coords;
    }

    public override List<SpawnLocation> CoordinatedSpawns => new()
    {
        new SpawnLocation(position)
    };

    public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
    {
        classId = ClassID,
        cellLevel = cellLevel,
        localScale = Vector3.one,
        slotType = EntitySlot.Type.Creature,
        techType = this.TechType
    };

    public override GameObject GetGameObject()
    {
        GameObject obj = new GameObject(ClassID);
        obj.EnsureComponent<LargeWorldEntity>().cellLevel = cellLevel;
        obj.EnsureComponent<T>();
        obj.SetActive(true);
        obj.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
        return obj;
    }

}