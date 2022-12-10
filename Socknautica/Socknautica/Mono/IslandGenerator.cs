using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using System.Collections.Generic;

namespace Socknautica.Mono;

internal class IslandGenerator : MonoBehaviour
{
    private float spawnRangeMin = 120;
    private float spawnRangeMax = 180;
    private float minDistanceBetween = 45;

    private float generateInterval = 0.5f;
    private float timeGenerateAgain;

    private static IslandsSaveData saveData { get; } = SaveDataHandler.Main.RegisterSaveDataCache<IslandsSaveData>();

    private static ForbiddenZone[] forbiddenZones = new ForbiddenZone[] {
        new ForbiddenZone(new (1450, -1098, -1448), 200), // aquarium
        new ForbiddenZone(new (1450, -998, -1448), 200), // aquarium island
        new ForbiddenZone(new (650, -2200, -1587), 70), // arena teleporter
        new ForbiddenZone(new (1700, -1900, -200), 70) // coords base
};
    private record ForbiddenZone(Vector3 pos, float dist);

    private void Start()
    {
        if (saveData.spawnedIslands == null) saveData.spawnedIslands = new List<Vector3>();
    }

    private void Update()
    {
        if (Time.time > timeGenerateAgain)
        {
            TryGenerate();
            timeGenerateAgain = Time.time + generateInterval;
        }
    }

    private void TryGenerate()
    {
        var randomPoint = MainCamera.camera.transform.position + Random.onUnitSphere * Random.Range(spawnRangeMin, spawnRangeMax);
        if (VoidIslandBiome.bounds.Contains(randomPoint))
        {
            if ((LargeWorld.main.GetBiome(randomPoint).Equals("void") || randomPoint.y < -1400) && IsWithinNoOtherIslands(randomPoint))
            {
                SpawnIsland(Main.GetRandomGenericIsland().ClassID, randomPoint);
            }
        }
    }

    private bool IsWithinNoOtherIslands(Vector3 point)
    {
        foreach (var forbidden in forbiddenZones)
        {
            if (Vector3.Distance(point, forbidden.pos) < forbidden.dist) return false;
        }
        foreach (var island in saveData.spawnedIslands)
        {
            if (Vector3.Distance(island, point) < minDistanceBetween) return false;
        }
        return true;
    }

    private void SpawnIsland(string classId, Vector3 position)
    {
        UWE.PrefabDatabase.TryGetPrefab(classId, out var prefab);
        var spawned = Instantiate(prefab, position, Quaternion.Euler(0, Random.value * 360, 0));
        spawned.SetActive(true);
        saveData.spawnedIslands.Add(position);
    }
}
[FileName("islands")]
internal class IslandsSaveData : SaveDataCache
{
    public List<Vector3> spawnedIslands;
}