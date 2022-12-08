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
    }
}
