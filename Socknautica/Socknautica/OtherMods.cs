using QModManager.API;

namespace Socknautica
{
    public static class OtherMods
    {
        private static bool? _submarineModExists;
        private static bool? _bloopAndBlazaModExists;

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

        public static bool BloopAndBlazaModExists
        {
            get
            {
                if (!_bloopAndBlazaModExists.HasValue)
                {
                    _bloopAndBlazaModExists = QModServices.Main.ModPresent("Socksfor1Monsters");
                }
                return _bloopAndBlazaModExists.Value;
            }
        }
    }
}
