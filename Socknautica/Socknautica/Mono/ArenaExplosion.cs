using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Socknautica.Mono;

internal class ArenaExplosion : MonoBehaviour
{
    private GameObject sphere;
    private float sphereScale;
    private Light light;
    private float lightScale;

    private bool started;

    public static void Explode()
    {
        new GameObject("ArenaExplosion").AddComponent<ArenaExplosion>();
    }

    private IEnumerator Start()
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(sphere.GetComponent<Collider>());
        sphere.GetComponent<Renderer>().material = MaterialUtils.IonCubeMaterial;
        sphere.transform.position = ArenaSpawner.main.center.position;

        var light = new GameObject("Light").AddComponent<Light>();
        light.range = 0f;
        light.intensity = 5f;
        light.color = Color.green;
        light.transform.position = ArenaSpawner.main.center.position;

        for (int i = 0; i < 15; i++)
        {
            MainCameraControl.main.ShakeCamera(3f, 2f);
            yield return new WaitForSeconds(1f);
        }
        started = true;
        yield return new WaitForSeconds(5f);
        KillBoss();
        FadingOverlay.PlayFX(Color.green, 0.1f, 2f, 0.1f);
        yield return new WaitForSeconds(3f);
        Destroy(sphere);
        Destroy(light);
        Destroy(gameObject);
        SceneManager.LoadSceneAsync("EndCreditsSceneCleaner", LoadSceneMode.Single);
    }

    private void KillBoss()
    {

    }

    private void Update()
    {
        if (!started) return;
        light.range = lightScale;
        sphere.transform.localScale = Vector3.one * sphereScale;
        sphereScale += 90 * Time.deltaTime;
        lightScale += 90 * Time.deltaTime;
    }
}
