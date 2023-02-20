using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class InitiateArenaSpawnInRange : MonoBehaviour
{
    private Vector3 teleporterLoc = new Vector3(2, -1993, 500);
    private float maxRange = 20f;
    private bool spawned = false;

    private void Update()
    {
        if (spawned) return;
        if ((teleporterLoc - Player.main.transform.position).sqrMagnitude < maxRange * maxRange)
        {
            spawned = true;
            ArenaSpawner.SpawnArena();
        }
    }
}
