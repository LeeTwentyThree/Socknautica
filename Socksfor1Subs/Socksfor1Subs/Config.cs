using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Socksfor1Subs
{
    [Menu("D.A.D. Submersible & S.O.C.K. Tank")]
    public class Config : ConfigFile
    {
        [Toggle("Enable torpedo flash", Tooltip = "Check this box if you want the S.O.C.K. Tank's torpedoes to flash a white light onto the screen when exploding in close proximity.")]
        public bool EnableTorpedoFlash = false;
        [Toggle("Enable warning siren", Tooltip = "Check this box if you want the D.A.D. Sub to warn you of nearby leviathans.\nRESTART REQUIRED.")]
        public bool WarningSiren = false;
        [Toggle("Enable warning voice line", Tooltip = "Check this box if you want the D.A.D. Sub to warn you of nearby leviathans with a voice line.\nRESTART REQUIRED.")]
        public bool WarningVoiceLine = false;
    }
}