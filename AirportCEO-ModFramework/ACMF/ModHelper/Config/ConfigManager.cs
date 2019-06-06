using ACMF.ModLoader;
using OdinSerializer;
using System;
using System.IO;
using System.Text;

namespace ACMF.ModHelper.Config
{
    public static class ACMFConfigManager
    {
        private static readonly string FileExtension = ".acmfconfig.json";

        public static T LoadConfig<T>(Mod mod) where T : ACMFConfig
        {
            return LoadConfig<T>(mod.ModInfo.ID);
        }

        internal static T LoadConfig<T>(string mod) where T : ACMFConfig
        {
            if (DoesConfigFileExist(mod) == false)
                return CreateNewConfig<T>(mod);

            return Utilities.JsonSerialization.Deserialize(out T config, GetConfigPath(mod)) ? config : CreateNewConfig<T>(mod);
        }

        private static string GetConfigPath(string mod)
        {
            return Path.Combine(ModLoader.ModLoader.GetModPath(), mod) + FileExtension;
        }

        private static bool DoesConfigFileExist(string mod)
        {
            return File.Exists(GetConfigPath(mod));
        }

        private static T CreateNewConfig<T>(string mod) where T : ACMFConfig
        {
            T config = (T) Activator.CreateInstance(typeof(T));
            config.ModID = mod;
            SaveConfig(config);
            return config;
        }

        public static bool SaveConfig(ACMFConfig config)
        {
            if (string.IsNullOrEmpty(config.ModID))
                return false;

            return Utilities.JsonSerialization.Serialize(config, GetConfigPath(config.ModID));
        }
    }
}
