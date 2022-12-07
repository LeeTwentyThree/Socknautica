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
    internal static PressuriumCrystal pressuriumCrystal;
    internal static EnergyPylon energyPylon;
    private static Vector3 aquariumPos = new Vector3(1450, -1000, -1450);

    private static void PatchPrefabsEarly()
    {

    }

    private static void PatchPrefabs()
    {
        PatchTerminals();
        aquariumBaseModel = new AquariumBaseModel("AquariumBaseModel", ".", ".", assetBundle.LoadAsset<GameObject>("AquariumBasePrefab"), new UBERMaterialProperties(7f), LargeWorldEntity.CellLevel.VeryFar, true);
        aquariumBaseModel.Patch();

        var aquariumBaseExteriorSpawner = new AlienBaseInitializer<AquariumBaseExteriorSpawner>("AquariumBaseExteriorSpawner", aquariumPos, LargeWorldEntity.CellLevel.VeryFar);
        aquariumBaseExteriorSpawner.Patch();

        var aquariumBaseSpawner = new AlienBaseInitializer<AquariumBaseSpawner>("AquariumBaseSpawner", aquariumPos, LargeWorldEntity.CellLevel.Medium);
        aquariumBaseSpawner.Patch();

        PatchAquariumIslandSegment("AquariumIslandBottom", "AquariumIslandBottom", Vector3.down * 170);
        PatchAquariumIslandSegment("AquariumIslandMiddle", "AquariumIslandMiddle", Vector3.zero);
        PatchAquariumIslandSegment("AquariumIslandTop", "AquariumIslandTop", Vector3.up * 170);

        genericIslands = new List<GenericIsland>();
        for (int i = 1; i <= 7; i++)
        {
            var island = new GenericIsland("GenericIsland" + i, assetBundle.LoadAsset<GameObject>("IslandPrefab" + i));
            island.Patch();
            genericIslands.Add(island);
        }

        pressuriumCrystal = new PressuriumCrystal();
        pressuriumCrystal.Patch();

        energyPylon = new EnergyPylon();
        energyPylon.Patch();

        var ancientFloaterFix = new AncientFloaterFix();
        ancientFloaterFix.Patch();
    }

    private static void PatchAquariumIslandSegment(string classId, string prefabName, Vector3 offset)
    {
        var aquariumIslandBottom = new AquariumIslandSegment(classId, assetBundle.LoadAsset<GameObject>(prefabName));
        aquariumIslandBottom.Patch();
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(aquariumIslandBottom.TechType, aquariumPos + offset));
    }

    private static void PatchTerminals()
    {
        var terminalBuilder = new DataTerminalBuilder();
    }
}
