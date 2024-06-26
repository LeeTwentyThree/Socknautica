namespace Socknautica;

using FMOD;
using System.Linq;
using SMLHelper.V2.FMod;
using ECCLibrary;

public partial class Main
{
    /// <summary>
    /// 3D sounds
    /// </summary>
    public const MODE k3DSoundModes = MODE.DEFAULT | MODE._3D | MODE.ACCURATETIME | MODE._3D_LINEARSQUAREROLLOFF;
    /// <summary>
    /// 2D sounds
    /// </summary>
    public const MODE k2DSoundModes = MODE.DEFAULT | MODE._2D | MODE.ACCURATETIME;
    /// <summary>
    /// For music, PDA and any 2D sounds that cant have more than one instance playing at a time.
    /// </summary>
    public const MODE kStreamSoundModes = k2DSoundModes | MODE.CREATESTREAM;
    
    public const string kGargSFXBus = "bus:/master/all/SFX/reverbsend";
    public const string kCreatureSFXBus = "bus:/master/all/SFX/creatures";
    public const string kMusicSFXBus = "bus:/master/nofilter/music";
    public const string kUnderwaterAmbientSFXBus = "bus:/master/all/SFX/backgrounds/underwater_backgrounds";
    public const string kSFXBus = "bus:/master/all/SFX";
    public const string kPDABus = "bus:/master/all/all voice/AI voice";
    public const string kCyclopsBus = "bus:/master/all/all voice/cyclops voice";
    public const string kRocketBus = "bus:/master/all/misc/rocket loops";

    // 3D Min distance should never be more than 5m, otherwise the rolloff will have a rough time figuring out the best numbers.

    public static FMODAsset sound_bossAmbience = Helpers.GetFmodAsset("BossAmbience");
    public static FMODAsset sound_bossRoar = Helpers.GetFmodAsset("BossRoar");
    public static FMODAsset sound_bossAttack = Helpers.GetFmodAsset("BossAttack");
    public static FMODAsset sound_bossBite = Helpers.GetFmodAsset("BossBite");
    public static FMODAsset sound_bossDeath = Helpers.GetFmodAsset("BossDeath");
    public static FMODAsset sound_bossGrowl = Helpers.GetFmodAsset("BossGrowl");
    public static FMODAsset sound_bossFear = Helpers.GetFmodAsset("BossFear");
    public static FMODAsset sound_bossHypnosis = Helpers.GetFmodAsset("BossHypnosis");

    private static void PatchFMODAudio()
    {
        PatchCreatureAudio();
        PatchPDALines();
        PatchTechAudio();
        PatchMusic();
    }
    
    private static void PatchCreatureAudio()
    {
        PatchBossSounds("BossAmbience", "BossAmbience");
        PatchBossSounds("BossRoar", "BossRoar");
        PatchBossSounds("BossAttack", "BossAttack");
        PatchBossSounds("BossBite", "BossBite");
        PatchBossSounds("BossDeath", "BossDeath");
        PatchBossSounds("BossGrowl", "BossGrowl");
        PatchBossSounds("BossFear", "BossFear");
        PatchBossSounds("BossHypnosis", "BossHypnosis");

        PatchCreatureSounds("AnglerJumpscare", "AnglerJumpscare", 200f);
        PatchCreatureSounds("MassiveLeviathanIdle", "MassiveLeviathanIdle", 9999f);

        /*
        var gargJuvenileFarSounds = AudioUtils.CreateSounds(ECCAudio.CreateClipPool("garg_for_anth_distant").clips, k3DSoundModes).ToArray();
        for (int i = 0; i < gargJuvenileFarSounds.Length; i++)
        {
            gargJuvenileFarSounds[i].set3DMinMaxDistance(5f, 500f);
        }
        var gargJuvenileFarEvent = new FModMultiSounds(gargJuvenileFarSounds, kCreatureSFXBus, true);
        CustomSoundHandler.RegisterCustomSound("GargJuvenileRoarFar", gargJuvenileFarEvent);

        var biteSounds = AudioUtils.CreateSounds(ECCAudio.CreateClipPool("GargBiteAttack").clips, k3DSoundModes).ToArray();
        for (int i = 0; i < biteSounds.Length; i++)
        {
            biteSounds[i].set3DMinMaxDistance(10f, 100f);
        }
        var biteEvent = new FModMultiSounds(biteSounds, kGargSFXBus, true);
        CustomSoundHandler.RegisterCustomSound("GargBite", biteEvent);
        */
    }

    private static void PatchBossSounds(string clipPrefix, string eventName)
    {
        var roarSounds = AudioUtils.CreateSounds(ECCAudio.CreateClipPool(clipPrefix).clips, k3DSoundModes).ToArray();
        for (int i = 0; i < roarSounds.Length; i++)
        {
            roarSounds[i].set3DMinMaxDistance(5f, 700f);
        }
        var roarEvent = new FModMultiSounds(roarSounds, kSFXBus, true);
        CustomSoundHandler.RegisterCustomSound(eventName, roarEvent);
    }

    private static void PatchCreatureSounds(string clipPrefix, string eventName, float maxDist)
    {
        var roarSounds = AudioUtils.CreateSounds(ECCAudio.CreateClipPool(clipPrefix).clips, k3DSoundModes).ToArray();
        for (int i = 0; i < roarSounds.Length; i++)
        {
            roarSounds[i].set3DMinMaxDistance(5f, maxDist);
        }
        var roarEvent = new FModMultiSounds(roarSounds, kSFXBus, true);
        CustomSoundHandler.RegisterCustomSound(eventName, roarEvent);
    }

    private static void PatchTechAudio()
    {
        AddWorldSoundEffect(assetBundle.LoadAsset<AudioClip>("EnergyPylonExplosion"), "EnergyPylonExplosion", 1f, 450);
    }

    private static void PatchMusic()
    {
        var reuniteMusic = AudioUtils.CreateSound(assetBundle.LoadAsset<AudioClip>("BossMusicQuiet"), kStreamSoundModes);
        CustomSoundHandler.RegisterCustomSound("BossMusic", reuniteMusic, kMusicSFXBus);
    }

    private static void PatchPDALines()
    {
        AddPDAVoiceLine(assetBundle.LoadAsset<AudioClip>("SignalAudio"), "SocksSignalSubtitles");
        AddPDAVoiceLine(assetBundle.LoadAsset<AudioClip>("SelfDestruct1"), "RocketSelfDestruct1");
        AddPDAVoiceLine(assetBundle.LoadAsset<AudioClip>("SelfDestruct2"), "RocketSelfDestruct2");
    }

    private static void AddPDAVoiceLine(AudioClip clip, string soundPath) // has no attached Goal
    {
        var sound = AudioUtils.CreateSound(clip, kStreamSoundModes);
        CustomSoundHandler.RegisterCustomSound(soundPath, sound, kPDABus);
    }

    private static void AddWorldSoundEffect(AudioClip clip, string soundPath, float minDistance = 1f, float maxDistance = 100f, string overrideBus = null)
    {
        var sound = AudioUtils.CreateSound(clip, k3DSoundModes);
        if (maxDistance > 0f)
        {
            sound.set3DMinMaxDistance(minDistance, maxDistance);
        }
        CustomSoundHandler.RegisterCustomSound(soundPath, sound, string.IsNullOrEmpty(overrideBus) ? kSFXBus : overrideBus);
    }
}
