using ECCLibrary;

namespace Socknautica
{
    [QModCore]
    public partial class Main
    {
        internal static AssetBundle assetBundle;
        internal static Assembly assembly = Assembly.GetExecutingAssembly();
        internal static Harmony harmony;

        [QModPrePatch]
        public static void Prepatch()
        {
            assetBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(assembly, "socknautica");
            harmony = new Harmony("Socksfor1.Socknautica");
            harmony.PatchAll(assembly);
            PatchPrefabsEarly();
        }

        [QModPatch]
        public static void Patch()
        {
            MaterialUtils.LoadMaterials();
            PatchPrefabs();
            PatchFMODAudio();
        }
    }
}