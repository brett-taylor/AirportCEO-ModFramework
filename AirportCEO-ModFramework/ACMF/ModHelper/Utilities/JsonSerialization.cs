using System;
using System.IO;
using System.Text;
using OdinSerializer;

namespace ACMF.ModHelper.Utilities
{
    public static class JsonSerialization
    {
        public static bool Serialize(object toSerialize, string filePath)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (Directory.Exists(directory) == false)
                    Directory.CreateDirectory(directory);

                using (TextWriter stream = new StreamWriter(filePath, false))
                    stream.WriteLine(Encoding.UTF8.GetString(SerializationUtility.SerializeValue(toSerialize, DataFormat.JSON)));

                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"JsonSerialization Failed: {e.ToString()}");
                Logger.Error(e.StackTrace);
                return false;
            }
        }

        public static bool Deserialize<T>(out T deserializedObject, string filePath)
        {
            try
            {
                if (File.Exists(filePath) == false)
                    throw new FileNotFoundException();

                using (TextReader stream = new StreamReader(filePath, Encoding.UTF8))
                {
                    string json = stream.ReadToEnd();
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    deserializedObject = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"JsonDeserialization Failed: {e.ToString()}");
                Logger.Error(e.StackTrace);
                deserializedObject = default;
                return false;
            }
        }
    }
}
