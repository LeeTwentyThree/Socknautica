using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Alien;

public class ArenaTeleporterBaseSpawner : AlienBaseSpawner
{
    protected override IEnumerator ConstructBase()
    {
        yield return SpawnPrefab(Main.arenaTeleporterBaseModel.ClassID, Vector3.zero);
        yield return SpawnPrefabGlobally(supplies_ionCrystal, new Vector3(657.55f, -2210.00f, -1594.55f), Vector3.up * 34, Vector3.one);
    }
}
