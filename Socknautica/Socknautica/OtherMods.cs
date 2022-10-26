using QModManager.API;

namespace Socknautica
{
    public static class OtherMods
    {
        private static bool? _submarineModExists;

        public static bool SubmarineModExists
        {
            get
            {
                if (!_submarineModExists.HasValue)
                {
                    _submarineModExists = QModServices.Main.ModPresent("DADTankSubPack");
                }
                return _submarineModExists.Value;
            }
        }
    }
}
