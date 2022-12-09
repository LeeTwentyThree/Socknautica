using QModManager.API;

namespace Socknautica
{
    public static class OtherMods
    {
        private static bool? _submarineModExists;
        private static bool? _bloopAndBlazaModExists;
        private static bool? _rotaExists; // rota is a spanish word for 'broken'! but here it means return of the ancients :)
        private static bool? _alExists;

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

        public static bool RotAExists
        {
            get
            {
                if (!_rotaExists.HasValue)
                {
                    _rotaExists = QModServices.Main.ModPresent("ProjectAncients");
                }
                return _rotaExists.Value;
            }
        }

        public static bool ArchitectsLibraryExists
        {
            get
            {
                if (!_alExists.HasValue)
                {
                    _alExists = QModServices.Main.ModPresent("ArchitectsLibray");
                }
                return _alExists.Value;
            }
        }
    }
}
