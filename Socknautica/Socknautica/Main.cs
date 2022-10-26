using HarmonyLib;
using QModManager.API.ModLoading;
using System.Reflection;

namespace Socknautica
{
    [QModCore]
    public static class Main
    {
        internal static Assembly assembly;
        internal static Harmony harmony;

        [QModPrePatch]
        public static void Prepatch()
        {
            assembly = Assembly.GetExecutingAssembly();
            harmony = new Harmony("Socksfor1.Socknautica");
        }

        [QModPatch]
        public static void Patch()
        {

        }
    }
}