namespace Socknautica;

using Prefabs;
using Prefabs.DataTerminal;
using Mono.Alien;
using ECCLibrary;
using System.Collections.Generic;

public partial class Main
{
    internal static AquariumBaseModel aquariumBaseModel;
    internal static List<GenericIsland> genericIslands;
    internal static GenericIsland GetRandomGenericIsland() => genericIslands[Random.Range(0, genericIslands.Count)];

    private static void PatchPrefabsEarly()
    {

    }

    private static void PatchPrefabs()
    {
        PatchTerminals();
        aquariumBaseModel = new AquariumBaseModel("AquariumBaseModel", ".", ".", assetBundle.LoadAsset<GameObject>("AquariumBasePrefab"), new UBERMaterialProperties(7f), LargeWorldEntity.CellLevel.VeryFar, true);
        aquariumBaseModel.Patch();

        var aquariumBaseExteriorSpawner = new AlienBaseInitializer<AquariumBaseExteriorSpawner>("AquariumBaseExteriorSpawner", new Vector3(1450, -1000, -1450), LargeWorldEntity.CellLevel.VeryFar);
        aquariumBaseExteriorSpawner.Patch();

        var aquariumBaseSpawner = new AlienBaseInitializer<AquariumBaseSpawner>("AquariumBaseSpawner", new Vector3(1450, -1000, -1450), LargeWorldEntity.CellLevel.Medium);
        aquariumBaseSpawner.Patch();

        genericIslands = new List<GenericIsland>();
        for (int i = 1; i <= 7; i++)
        {
            var island = new GenericIsland("GenericIsland" + i, assetBundle.LoadAsset<GameObject>("IslandPrefab" + i));
            island.Patch();
            genericIslands.Add(island);
        }
    }

    private static void PatchTerminals()
    {
        var terminalBuilder = new DataTerminalBuilder();
    }
}
