using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Socknautica
{
    [Menu("Socknautica")]
    internal class Config : ConfigFile
    {
        [Toggle("Disable leviathan fear", Tooltip = "Check this box if you want leviathans to not swim away into walls when taking damage.\nRESTART REQUIRED.")]
        public bool DisableLeviathanFear = true;
        [Toggle("More vicious Reapers", Tooltip = "Check this box if you want Reaper Leviathans to be more aggressive towards the S.O.C.K. Tank.\nRESTART REQUIRED.")]
        public bool MoreViciousReapers = true;
        [Toggle("Disable fog in Arena", Tooltip = "Check this box if you want to be able to see throughout the entire arena.\nRESTART REQUIRED.")]
        public bool NoFogInArena = true;
    }
}