using UWE;

namespace Socknautica.Mono.Alien;

// for terminals
public class DataTerminalUnlockPing : MonoBehaviour
{
    public string classId;
    public string pingTypeName;

    public void OnStoryHandTarget()
    {
        foreach (SignalPingDelayedInitialize spawnedPing in SignalPingDelayedInitialize.spawnedPings)
        {
            {
                if (spawnedPing == null)
                {
                    continue;
                }
                if (spawnedPing.pingTypeName == pingTypeName)
                {
                    return;
                }
            }
        }
#if SN1
        if (PrefabDatabase.TryGetPrefab(classId, out GameObject prefab))
        {
            GameObject pingObj = Instantiate(prefab);
            pingObj.SetActive(true);
            LargeWorld.main.streamer.cellManager.RegisterEntity(pingObj.GetComponent<LargeWorldEntity>());
        }
#elif SN1_exp
            AddressablesUtility.LoadAsync<GameObject>(classId).Completed += (_) =>
            {
                GameObject prefab = _.Result;

                GameObject pingObj = Instantiate(prefab);
                pingObj.SetActive(true);
                LargeWorld.main.streamer.cellManager.RegisterCellEntity(pingObj.GetComponent<LargeWorldEntity>());
            };
#endif
    }
}