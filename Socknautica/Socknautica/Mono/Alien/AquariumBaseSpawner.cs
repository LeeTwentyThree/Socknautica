using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Alien;

public class AquariumBaseSpawner : AlienBaseSpawner
{
    protected override IEnumerator ConstructBase()
    {
        var ampeel = "e69be2e8-a2e3-4c4c-a979-281fbf221729";
        var gasopod = "3c13b3a4-ac02-4601-8030-b9d7482cde1e";
        var crabsquid = "4c2808fe-e051-44d2-8e64-120ddcdc8abb";
        var warper = "510a71f0-ab6d-4c6a-aa54-a19b3f1c436c";
        var riverProwler = "e82d3c24-5a58-4307-a775-4741050c8a78";

        // aquarium
        yield return SpawnPrefabGlobally(gasopod, new Vector3(1440.86f, -992.45f, -1410.69f));
        yield return SpawnPrefabGlobally(ampeel, new Vector3(1461.62f, -1007.95f, -1498.10f));
        yield return SpawnPrefabGlobally(crabsquid, new Vector3(1464.71f, -976.63f, -1499.27f));
        yield return SpawnPrefabGlobally(gasopod, new Vector3(1428.66f, -1030.27f, -1494.81f));
        yield return SpawnPrefabGlobally(crabsquid, new Vector3(1401.17f, -1033.29f, -1457.86f));
        yield return SpawnPrefabGlobally(gasopod, new Vector3(1424.88f, -1034.59f, -1420.39f));
        yield return SpawnPrefabGlobally(ampeel, new Vector3(1466.38f, -1035.93f, -1414.86f));
        yield return SpawnPrefabGlobally(ampeel, new Vector3(1491.85f, -1018.90f, -1434.60f));
        yield return SpawnPrefabGlobally(crabsquid, new Vector3(1493.22f, -995.27f, -1435.98f));
        yield return SpawnPrefabGlobally(ampeel, new Vector3(1480.27f, -976.72f, -1417.28f));
        yield return (ampeel, new Vector3(1421.83f, -989.41f, -1436.43f));
        yield return SpawnPrefabGlobally(crabsquid, new Vector3(1422.95f, -1004.83f, -1451.61f));
        yield return SpawnPrefabGlobally(gasopod, new Vector3(1421.41f, -1003.68f, -1413.01f));
        yield return SpawnPrefabGlobally(ampeel, new Vector3(1426.69f, -974.44f, -1435.10f));

        yield return SpawnPrefabGlobally("ae06567b-4afd-4aff-9904-e518c1e8e30a", new Vector3(1459.7f, -1044.8f, -1489.2f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("583f8885-20fd-4c69-aa5a-5fcd7c58804b", new Vector3(1437.7f, -1046.9f, -1465.1f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("583f8885-20fd-4c69-aa5a-5fcd7c58804b", new Vector3(1422.3f, -1043.9f, -1445.4f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("ae06567b-4afd-4aff-9904-e518c1e8e30a", new Vector3(1475.8f, -1044.5f, -1433.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("e42243eb-4f38-42cd-acec-1d38d9b1b120", new Vector3(1457.7f, -1048.3f, -1410.3f), new Vector3(5.7f, 360.0f, 359.2f), Vector3.zero);
        yield return SpawnPrefabGlobally("e42243eb-4f38-42cd-acec-1d38d9b1b120", new Vector3(1417.1f, -1043.4f, -1407.2f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);

        // elevator
        yield return SpawnPrefabGlobally(warper, new Vector3(1492.71f, -996.57f, -1469.66f));
        yield return SpawnPrefabGlobally(warper, new Vector3(1458.55f, -999.36f, -1455.56f));
        yield return SpawnPrefabGlobally(warper, new Vector3(1457.99f, -1054.93f, -1457.77f));
        yield return SpawnPrefabGlobally("ae06567b-4afd-4aff-9904-e518c1e8e30a", new Vector3(1470.4f, -1027.0f, -1451.1f), new Vector3(24.8f, 24.8f, 90.0f), Vector3.zero);

        // lower area
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1448.45f, -1055.85f, -1482.27f));
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1475.19f, -1056.59f, -1426.84f));

        yield return SpawnPrefabGlobally("ae06567b-4afd-4aff-9904-e518c1e8e30a", new Vector3(1385.3f, -1052.0f, -1479.9f), new Vector3(0.0f, 180.0f, 180.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("583f8885-20fd-4c69-aa5a-5fcd7c58804b", new Vector3(1388.0f, -1056.2f, -1472.1f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("ae06567b-4afd-4aff-9904-e518c1e8e30a", new Vector3(1388.0f, -1057.2f, -1477.1f), new Vector3(0.0f, 0.0f, 270.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("583f8885-20fd-4c69-aa5a-5fcd7c58804b", new Vector3(1385.2f, -1058.9f, -1475.2f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("e42243eb-4f38-42cd-acec-1d38d9b1b120", new Vector3(1384.5f, -1059.0f, -1483.4f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1391.9f, -1060.0f, -1476.7f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1391.7f, -1060.0f, -1482.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1391.5f, -1060.0f, -1485.8f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1395.2f, -1060.0f, -1480.6f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1399.7f, -1060.0f, -1486.0f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);
        yield return SpawnPrefabGlobally("PressuriumCrystal", new Vector3(1401.7f, -1060.0f, -1479.7f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.zero);

        // upper area
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1480.02f, -954.65f, -1409.85f));
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1444.75f, -955.65f, -1488.90f));
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1400.60f, -954.15f, -1483.27f));
        for (int i = 0; i < 3; i++) yield return SpawnPrefabGlobally(riverProwler, new Vector3(1446.25f, -952.07f, -1393.37f));

        yield return SpawnPrefabGlobally(crabsquid, new Vector3(1401.40f, -952.89f, -1431.32f));
    }
}
