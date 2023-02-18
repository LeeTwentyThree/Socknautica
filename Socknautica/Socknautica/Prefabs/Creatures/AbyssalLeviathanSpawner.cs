using System.Collections.Generic;
using Socknautica.Mono.Creatures;

namespace Socknautica.Prefabs.Creatures;

internal class AbyssalLeviathanSpawner : Spawnable
{
    public AbyssalLeviathanSpawner() : base("AbyssalLeviathanSpawner", "???", "???")
    {
    }

    public override List<SpawnLocation> CoordinatedSpawns => new List<SpawnLocation>() { new SpawnLocation(Vector3.zero) };

    public override GameObject GetGameObject()
    {
        var prefab = new GameObject("AbyssalLeviathan");
        prefab.AddComponent<PrefabIdentifier>();
        prefab.AddComponent<TechTag>();
        prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
        prefab.AddComponent<AbyssalMouthBehaviour>();

        return prefab;
    }
}
