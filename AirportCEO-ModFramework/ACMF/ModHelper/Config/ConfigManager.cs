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

            using (FileStream stream = File.Open(GetConfigPath(mod), FileMode.Open))
            {
                return (T) SerializationUtility.DeserializeValue<ACMFConfig>(stream, DataFormat.JSON);
            }
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

            string filePath = GetConfigPath(config.ModID);
            TextWriter stream = new StreamWriter(filePath, false);
            stream.WriteLine(Encoding.UTF8.GetString(SerializationUtility.SerializeValue(config, DataFormat.JSON)));
            stream.Close();
            return true;
        }
    }
}
