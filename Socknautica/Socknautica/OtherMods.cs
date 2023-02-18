using QModManager.API;

namespace Socknautica
{
    public static class OtherMods
    {
        private static bool? _submarineModExists;
        private static bool? _bloopAndBlazaModExists;
        private static bool? _rotaExists; // rota is a spanish word for 'broken'! but here it means return of the ancients :)
        private static bool? _alExists;
        private static bool? _deExtinctionExists;

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

        public static bool DeExtinctionExists
        {
            get
            {
                if (!_deExtinctionExists.HasValue)
                {
                    _deExtinctionExists = QModServices.Main.ModPresent("DeExtinction");
                }
                return _deExtinctionExists.Value;
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
