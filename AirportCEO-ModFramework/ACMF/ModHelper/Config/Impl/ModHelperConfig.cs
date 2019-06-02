namespace ACMF.ModHelper.Config
{
    [System.Serializable]
    internal class ModHelperConfig : ACMFConfig
    {
        public bool ENABLE_INSTANT_LOAD_INTO_SAVE_GAME = false;
        public string INSTANT_LOAD_INTO_SAVE_GAME_FILE = @"C:\Users\MY_USER_NAME\AppData\Roaming\Apoapsis Studios\Airport CEO\Saves\MY_AIRPORT_NAME";
        public bool UNBOUNDED_CAMERA = false;
    }
}
