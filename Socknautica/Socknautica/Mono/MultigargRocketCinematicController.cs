using Socknautica;
using System.Collections;
using UWE;

namespace Socknautica.Mono;

internal class MultigargRocketCinematicController : MonoBehaviour
{
    private Rocket rocket;
    private Transform rocketReference;
    private Transform cameraReference;
    private Transform rocketRoot;
    private Transform playerRoot;
    private GameObject seamothExplode;

    private bool running = true;

    private string[] legNames = new string[] { "Rocketship_leg_01", "Rocketship_leg_01 1", "Rocketship_leg_02", "Rocketship_leg_04" };

    private CinematicSound[] audio = new CinematicSound[]
    {
        new CinematicSound(0, Main.sound_bossAmbience),
        new CinematicSound(3, Main.sound_bossGrowl),
        new CinematicSound(4, Main.sound_bossAttack),
        new CinematicSound(5, Main.sound_bossRoar),
        new CinematicSound(8.4f, Main.sound_bossRoar),
        new CinematicSound(8.8f, Main.sound_bossBite),
        new CinematicSound(9f, Main.sound_bossBite),
        new CinematicSound(9.2f, Main.sound_bossBite),
        new CinematicSound(9.4f, Main.sound_bossBite),
        new CinematicSound(11, Main.sound_bossBite),
        new CinematicSound(12, Main.sound_bossBite),
        new CinematicSound(12.4f, Main.sound_bossAttack),
        new CinematicSound(16f, Main.sound_bossAttack),
        new CinematicSound(17, Main.sound_bossGrowl),
        new CinematicSound(18, Main.sound_bossGrowl),
        new CinematicSound(19, Main.sound_bossGrowl),
        new CinematicSound(20, Main.sound_bossGrowl),
    };

    public static void PlayCinematic(Rocket rocket)
    {
        var prefab = Main.assetBundle.LoadAsset<GameObject>("MultiGargAnimPrefab");
        var cin = Instantiate(prefab).AddComponent<MultigargRocketCinematicController>();
        cin.transform.position = rocket.transform.position;
        cin.gameObject.SetActive(true);
        cin.rocket = rocket;
        cin.rocketReference = cin.transform.Find("MultiGargAnim/rocketship_model_reference");
        cin.cameraReference = cin.transform.Find("MultiGargAnim/Camera");
        cin.seamothExplode = CraftData.GetPrefabForTechType(TechType.Seamoth).GetComponent<SeaMoth>().destructionEffect;
        cin.GetComponentInChildren<Animator>().cullingMode = AnimatorCullingMode.AlwaysAnimate;
    }

    private IEnumerator Start()
    {
        // Timer.Show();

        // START ANIMATION

        uGUI.main.transform.Find("ScreenCanvas/Pings").gameObject.SetActive(false);

        gameObject.AddComponent<SkyApplier>().renderers = gameObject.GetComponentsInChildren<Renderer>(true);
        MaterialUtils.ApplySNShaders(gameObject);

        rocketRoot = new GameObject("RocketRoot").transform;
        rocketRoot.transform.position = rocket.transform.position;

        var stage1 = rocket.transform.Find("Stage01");
        var legRoot = stage1.Find("rocketship_stage_01/Rocket_Geo");
        foreach (var legName in legNames)
        {
            legRoot.transform.Find(legName).gameObject.SetActive(false);
        }
        stage1.parent = rocketRoot;
        rocket.transform.Find("Stage02").parent = rocketRoot;
        rocket.transform.Find("Stage03").parent = rocketRoot;

        playerRoot = Player.main.transform;
        playerRoot.GetComponent<PlayerController>().enabled = false;

        float oldFOV = MainCamera.camera.fieldOfView;
        MainCamera.camera.fieldOfView = 30;
        MainCamera.camera.farClipPlane = 2500;

        Player.main.SetPrecursorOutOfWater(false);

        ToggleRenderers(false);

        // NOW WAIT

        yield return new WaitForSeconds(4.4f);
        ToggleRenderers(true);

        yield return new WaitForSeconds(2.6f);

        CreateExplosion(rocketReference.position, 4, 6, 2);

        yield return new WaitForSeconds(5.2f);

        // CHOMP!

        CreateExplosion(rocketReference.position + rocketReference.up * 20, 10, 2);

        yield return new WaitForSeconds(4.4f);

        CreateExplosion(rocketReference.position + rocketReference.up * 20, 10, 10);

        // YOU DIED!

        Destroy(rocket.gameObject);
        Destroy(rocketRoot.gameObject);

        LaunchRocket.launchStarted = true;

        yield return new WaitForSeconds(6f);

        Player.main.liveMixin.Kill();

        // END THE ANIMATION!

        yield return new WaitForSeconds(3);

        running = false;
        playerRoot.GetComponent<PlayerController>().enabled = true;
        MainCamera.camera.transform.localRotation = Quaternion.identity;

        HandReticle.main.UnrequestCrosshairHide();

        LaunchRocket.launchStarted = false;

        uGUI.main.transform.Find("ScreenCanvas/Pings").gameObject.SetActive(true);

        MainCamera.camera.fieldOfView = oldFOV;
        MainCamera.camera.farClipPlane = 1700;

        LaunchRocket.launchStarted = false;

        yield return new WaitForSeconds(10f);

        CustomPDALinesManager.PlayVoiceLine("RocketSelfDestruct2");
        if (PrefabDatabase.TryGetPrefab("AquariumBaseSignal", out GameObject prefab))
        {
            GameObject pingObj = Instantiate(prefab);
            pingObj.SetActive(true);
            LargeWorld.main.streamer.cellManager.RegisterEntity(pingObj.GetComponent<LargeWorldEntity>());
        }

        Destroy(gameObject);
    }

    private void ToggleRenderers(bool e)
    {
        foreach (var renderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = e;
        }
    }

    private void CreateExplosion(Vector3 pos, float radius, int explosions, float scale = 6)
    {
        for (int i = 0; i < explosions; i++)
        {
            SpawnSeamothExplosion(pos + (Random.insideUnitSphere * radius), scale);
        }
    }

    private GameObject SpawnSeamothExplosion(Vector3 pos, float scale)
    {
        var go = Instantiate(seamothExplode, pos, Quaternion.identity);
        Helpers.MakeParticleSystemScaleable(go);
        go.transform.GetChild(1).gameObject.SetActive(false);
        go.transform.localScale = Vector3.one * scale;
        go.GetComponent<FMOD_StudioEventEmitter>().enabled = false;
        return go;
    }

    private void Update()
    {
        foreach (var snd in audio)
        {
            snd.OnUpdate(rocketReference.transform.position);
        }
    }

    private void LateUpdate()
    {
        if (running)
        {
            if (rocketRoot != null)
            {
                rocketRoot.position = rocketReference.position;
                rocketRoot.rotation = rocketReference.rotation;
            }
            playerRoot.position = cameraReference.position;
            MainCamera.camera.transform.rotation = cameraReference.rotation;
        }
    }

    private class CinematicSound
    {
        public float startTime;
        public FMODAsset sound;

        public CinematicSound(float startTime, FMODAsset sound)
        {
            this.startTime = startTime;
            this.sound = sound;
            _timeCinematicStarted = Time.time;
        }

        private bool _played;
        private float _timeCinematicStarted;

        public void OnUpdate(Vector3 loc)
        {
            if (_played)
                return;

            if (Time.time > _timeCinematicStarted + startTime)
            {
                Utils.PlayFMODAsset(sound, loc);
                _played = true;
            }
        }
    }
}