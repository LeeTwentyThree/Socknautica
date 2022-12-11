namespace Socknautica;

using Prefabs;
using Prefabs.DataTerminal;
using Mono.Alien;
using ECCLibrary;
using System.Collections.Generic;
using Prefabs.Teleporter;

public partial class Main
{
    internal static AquariumBaseModel aquariumBaseModel;
    internal static CoordBaseModel coordBaseModel;
    internal static ArenaTeleporterBaseModel arenaTeleporterBaseModel;
    internal static List<GenericIsland> genericIslands;
    internal static GenericIsland GetRandomGenericIsland() => genericIslands[Random.Range(0, genericIslands.Count)];
    internal static PressuriumCrystal pressuriumCrystal;
    internal static AtmospheriumCrystal atmospheriumCrystal;
    internal static EnergyPylon energyPylon;
    internal static ArenaLightPillar arenaLightPillar;
    private static Vector3 aquariumPos = new Vector3(1450, -1000, -1450);
    private static Vector3 coordBasePos = new Vector3(1700, -1900, -200);
    private static Vector3 arenaTeleporterPos = new Vector3(650, -2200, -1587);

    internal static DataTerminalPrefab sockTankTerminal;
    internal static DataTerminalPrefab aquariumBaseCoordsTerminal;
    internal static DataTerminalPrefab dadSubTerminal;
    internal static DataTerminalPrefab coordBaseCoordsTerminal;
    internal static DataTerminalPrefab arenaBaseCoordsTerminal;

    internal static MirageFish mirageFish;
    internal static Multigarg multigarg;

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

        arenaTeleporterBaseModel = new ArenaTeleporterBaseModel();
        arenaTeleporterBaseModel.Patch();

        var aquariumBaseExteriorSpawner = new AlienBaseInitializer<AquariumBaseExteriorSpawner>("AquariumBaseExteriorSpawner", aquariumPos, LargeWorldEntity.CellLevel.VeryFar);
        aquariumBaseExteriorSpawner.Patch();

        var aquariumBaseSpawner = new AlienBaseInitializer<AquariumBaseSpawner>("AquariumBaseSpawner", aquariumPos, LargeWorldEntity.CellLevel.Medium);
        aquariumBaseSpawner.Patch();

        var coordBaseExteriorSpawner = new AlienBaseInitializer<CoordBaseExteriorSpawner>("CoordBaseExteriorSpawner", coordBasePos, LargeWorldEntity.CellLevel.VeryFar);
        coordBaseExteriorSpawner.Patch();

        var coordBaseSpawner = new AlienBaseInitializer<CoordBaseSpawner>("CoordBaseSpawner", coordBasePos, LargeWorldEntity.CellLevel.Medium);
        coordBaseSpawner.Patch();

        var arenaTeleporterBaseSpawner = new AlienBaseInitializer<ArenaTeleporterBaseSpawner>("ArenaTeleporterBaseSpawner", arenaTeleporterPos, LargeWorldEntity.CellLevel.Medium);
        arenaTeleporterBaseSpawner.Patch();

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

        atmospheriumCrystal = new AtmospheriumCrystal();
        atmospheriumCrystal.Patch();

        energyPylon = new EnergyPylon();
        energyPylon.Patch();

        arenaLightPillar = new ArenaLightPillar();
        arenaLightPillar.Patch();

        var ancientFloaterFix = new AncientFloaterFix();
        ancientFloaterFix.Patch();

        mirageFish = new MirageFish();
        mirageFish.Patch();

        multigarg = new Multigarg();
        multigarg.Patch();

        TeleporterNetwork network = new TeleporterNetwork("ArenaTeleporter", new Vector3(647, -2210, -1612), 0, new Vector3(0, -1999.77f, 430), 180);
        network.Patch();
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

        var arenaTeleporterSignal = new GenericSignalPrefab("ArenaTeleporterSignal", "PingIcon", "Reactor Access [NO EXIT]", "Reactor Access [NO EXIT]", new Vector3(647, -2204, -1607));
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
