using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Alien;

public class CoordBaseSpawner : AlienBaseSpawner
{
    protected override IEnumerator ConstructBase()
    {
        yield return SpawnPrefabGlobally(Main.sockTankTerminal.ClassID, new Vector3(1710.68f, -1900.00f, -221.77f), Vector3.up * 90, Vector3.one);
        yield return SpawnPrefabGlobally(Main.arenaBaseCoordsTerminal.ClassID, new Vector3(1684.00f, -1900.00f, -221.77f), Vector3.up * -90, Vector3.one);

        var warper = "510a71f0-ab6d-4c6a-aa54-a19b3f1c436c";
        yield return SpawnPrefabGlobally(warper, new Vector3(1701.24f, -1893.26f, -184.25f));
        yield return SpawnPrefabGlobally(warper, new Vector3(1688.81f, -1894.67f, -178.55f));
        yield return SpawnPrefabGlobally(warper, new Vector3(1697.11f, -1895.97f, -207.89f));

        yield return SpawnPrefabGlobally("4fae8fa4-0280-43bd-bcf1-f3cba97eed77", new Vector3(1697.50f, -1897.56f, -211.17f));

        yield return SpawnPrefabGlobally("1b85a183-2084-42a6-8d85-7e58dd6864bd", new Vector3(1697.00f, -1902.38f, -226.00f), Vector3.up * 90, new Vector3(3, 5, 4));
        yield return SpawnPrefabGlobally("GiantTooth", new Vector3(1694.00f, -1894.47f, -224.60f), new Vector3(15, 355, 180), new Vector3(9, 8, 6));

        yield return SpawnPrefabGlobally("AtmospheriumCrystal", new Vector3(1708.2f, -1900.0f, -225.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("AtmospheriumCrystal", new Vector3(1688.9f, -1900.0f, -217.6f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);

        yield return SpawnPrefabGlobally("ce3c3053-5022-404e-a165-e31abe495f1b", new Vector3(1687.3f, -1890.0f, -221.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("ce3c3053-5022-404e-a165-e31abe495f1b", new Vector3(1697.2f, -1890.0f, -221.7f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("ce3c3053-5022-404e-a165-e31abe495f1b", new Vector3(1707.1f, -1890.0f, -221.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("ce3c3053-5022-404e-a165-e31abe495f1b", new Vector3(1697.1f, -1890.0f, -209.9f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("ce3c3053-5022-404e-a165-e31abe495f1b", new Vector3(1697.1f, -1890.0f, -201.7f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);

        yield return SpawnPrefabGlobally("AbyssalOculus", new Vector3(1687.5f, -1897.1f, -220.1f), new Vector3(17.5f, 295.9f, 0.4f), Vector3.one);
        yield return SpawnPrefabGlobally("AbyssalOculus", new Vector3(1700.2f, -1896.9f, -217.8f), new Vector3(0.7f, 272.5f, 0.1f), Vector3.one);

        yield return SpawnPrefabGlobally("a30d0115-c37e-40ec-a5d9-318819e94f81", new Vector3(1686.1f, -1900.0f, -225.5f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.one);
        yield return SpawnPrefabGlobally("a30d0115-c37e-40ec-a5d9-318819e94f81", new Vector3(1684.1f, -1900.0f, -219.2f), new Vector3(0.0f, 120.0f, 0.0f), Vector3.one);

        yield return SpawnPrefabGlobally("172d9440-2670-45a3-93c7-104fee6da6bc", new Vector3(1702.2f, -1899.0f, -206.6f), new Vector3(0.0f, 90.0f, 0.0f), Vector3.one);
    }
}
