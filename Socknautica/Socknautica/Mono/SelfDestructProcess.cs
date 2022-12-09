using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWE;

namespace Socknautica.Mono;

internal class SelfDestructProcess : MonoBehaviour
{
    public Rocket rocket;

    public static void Begin(Rocket rocket)
    {
        new GameObject().AddComponent<SelfDestructProcess>().rocket = rocket;
    }

    private IEnumerator Start()
    {
        CustomPDALinesManager.PlayVoiceLine("RocketSelfDestruct1");
        ErrorMessage.AddMessage("Self destruct sequence activated!");
        yield return new WaitForSeconds(18);
        Utils.PlayFMODAsset(Helpers.GetFmodAsset("event:/sub/cyclops/explode"));
        FadingOverlay.PlayFX(Color.white, 0.1f, 5f, 2f);
        yield return new WaitForSeconds(1f);
        Destroy(rocket.gameObject);
        yield return new WaitForSeconds(6f);
        CustomPDALinesManager.PlayVoiceLine("RocketSelfDestruct2");
        if (PrefabDatabase.TryGetPrefab("AquariumBaseSignal", out GameObject prefab))
        {
            GameObject pingObj = Instantiate(prefab);
            pingObj.SetActive(true);
            LargeWorld.main.streamer.cellManager.RegisterEntity(pingObj.GetComponent<LargeWorldEntity>());
        }
    }
}