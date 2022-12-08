namespace Socknautica;

using Prefabs;
using Prefabs.DataTerminal;
using Mono.Alien;
using ECCLibrary;
using System.Collections.Generic;

public partial class Main
{
    internal static AquariumBaseModel aquariumBaseModel;
    internal static CoordBaseModel coordBaseModel;
    internal static List<GenericIsland> genericIslands;
    internal static GenericIsland GetRandomGenericIsland() => genericIslands[Random.Range(0, genericIslands.Count)];
    internal static PressuriumCrystal pressuriumCrystal;
    internal static EnergyPylon energyPylon;
    private static Vector3 aquariumPos = new Vector3(1450, -1000, -1450);
    private static Vector3 coordBasePos = new Vector3(1700, -1900, -200);

    internal static DataTerminalPrefab sockTankTerminal;
    internal static DataTerminalPrefab aquariumBaseCoordsTerminal;
    internal static DataTerminalPrefab dadSubTerminal;
    internal static DataTerminalPrefab coordBaseCoordsTerminal;
    internal static DataTerminalPrefab arenaBaseCoordsTerminal;

    private static void PatchPrefabsEarly()
    {

    }

    private static void PatchPrefabs()
    {
        PatchTerminals();

        aquariumBaseModel = new AquariumBaseModel("AquariumBaseModel", ".", ".", assetBundle.LoadAsset<GameObject>("AquariumBasePrefab"), new UBERMaterialProperties(7f), LargeWorldEntity.CellLevel.VeryFar, true);
        aquariumBaseModel.Patch();

        coordBaseModel = new CoordBaseModel();
        coordBaseModel.Patch();

        var aquariumBaseExteriorSpawner = new AlienBaseInitializer<AquariumBaseExteriorSpawner>("AquariumBaseExteriorSpawner", aquariumPos, LargeWorldEntity.CellLevel.VeryFar);
        aquariumBaseExteriorSpawner.Patch();

        var aquariumBaseSpawner = new AlienBaseInitializer<AquariumBaseSpawner>("AquariumBaseSpawner", aquariumPos, LargeWorldEntity.CellLevel.Medium);
        aquariumBaseSpawner.Patch();

        var coordBaseExteriorSpawner = new AlienBaseInitializer<CoordBaseExteriorSpawner>("CoordBaseExteriorSpawner", coordBasePos, LargeWorldEntity.CellLevel.VeryFar);
        coordBaseExteriorSpawner.Patch();

        var coordBaseSpawner = new AlienBaseInitializer<CoordBaseSpawner>("CoordBaseSpawner", coordBasePos, LargeWorldEntity.CellLevel.Medium);
        coordBaseSpawner.Patch();

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
        var blueprintEvent = "event:/tools/scanner/new_blueprint";

        var aquariumBaseSignal = new GenericSignalPrefab("AquariumBaseSignal", "PingIcon", "Breached Aquarium", "Breached Aquarium", new Vector3(1520, -1000, -1480));
        aquariumBaseSignal.Patch();

        var coordsBaseSignal = new GenericSignalPrefab("CoordBaseSignal", "PingIcon", "Outpost Cache", "Outpost Cache", new Vector3(1700, -1891, -173));
        coordsBaseSignal.Patch();

        var arenaTeleporterSignal = new GenericSignalPrefab("ArenaTeleporterSignal", "PingIcon", "[REDACTED]", "[REDACTED]", new Vector3(999, -9999, -999));
        arenaTeleporterSignal.Patch();

        var terminalBuilder = new DataTerminalBuilder();

        terminalBuilder.SetupStoryGoal("SockTankUnlock");
        terminalBuilder.SetupSound(blueprintEvent, null);
        terminalBuilder.SetupTemplateTerminal(DataTerminalPrefab.orangeTerminalCID);
        if (TechTypeHandler.TryGetModdedTechType("SockTank", out var sockTank))
        {
            terminalBuilder.SetupUnlockables(null, new TechType[] { sockTank }, 3);
        }
        sockTankTerminal = new DataTerminalPrefab("SockTankTerminal", terminalBuilder.GetTerminal());
        sockTankTerminal.Patch();

        terminalBuilder.SetupStoryGoal("DadSubUnlock");
        terminalBuilder.SetupSound(blueprintEvent, null);
        terminalBuilder.SetupTemplateTerminal(DataTerminalPrefab.orangeTerminalCID);
        if (TechTypeHandler.TryGetModdedTechType("DadSub", out var dadSub))
        {
            terminalBuilder.SetupUnlockables(null, new TechType[] { dadSub }, 3);
        }
        dadSubTerminal = new DataTerminalPrefab("DadSubTerminal", terminalBuilder.GetTerminal());
        dadSubTerminal.Patch();

        terminalBuilder.SetupStoryGoal("AquariumBaseCoords");
        terminalBuilder.SetupPingClassIds(new string[] { aquariumBaseSignal.ClassID });
        terminalBuilder.SetupSound("SocksSignalSubtitles", "SocksSignalSubtitles");
        aquariumBaseCoordsTerminal = new DataTerminalPrefab("AquariumBaseCoordsTerminal", terminalBuilder.GetTerminal());
        aquariumBaseCoordsTerminal.Patch();

        terminalBuilder.SetupStoryGoal("CoordBaseCoords");
        terminalBuilder.SetupPingClassIds(new string[] { coordsBaseSignal.ClassID });
        terminalBuilder.SetupSound("SocksSignalSubtitles", "SocksSignalSubtitles");
        coordBaseCoordsTerminal = new DataTerminalPrefab("CoordBaseCoordsTerminal", terminalBuilder.GetTerminal());
        coordBaseCoordsTerminal.Patch();

        terminalBuilder.SetupStoryGoal("ArenaTeleporterCoords");
        terminalBuilder.SetupPingClassIds(new string[] { arenaTeleporterSignal.ClassID });
        terminalBuilder.SetupSound("SocksSignalSubtitles", "SocksSignalSubtitles");
        arenaBaseCoordsTerminal = new DataTerminalPrefab("ArenaBaseCoordsTerminal", terminalBuilder.GetTerminal());
        arenaBaseCoordsTerminal.Patch();
    }
}
