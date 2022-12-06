using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono.Alien;

public class AquariumBaseExteriorSpawner : AlienBaseSpawner
{
    protected override IEnumerator ConstructBase()
    {
        yield return SpawnPrefab(Main.aquariumBaseModel.ClassID, Vector3.zero);
    }
}
