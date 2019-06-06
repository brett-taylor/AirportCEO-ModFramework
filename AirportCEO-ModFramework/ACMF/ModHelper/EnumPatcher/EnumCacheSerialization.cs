using System;
using System.IO;

namespace ACMF.ModHelper.EnumPatcher
{
    public partial class EnumCache<T> : IEnumCache where T : Enum
    {
        private static readonly string ENUM_CACHE_FILE_ENDING = ".acmfCache.json";
        private static readonly string ENUM_CACHE_FOLDER_NAME = "GlobalEnumCache";
        private static string SERIALIZED_ENUM_CACHE_FOLDER { get => Path.Combine(ACMF.ACMFFolderLocation, ENUM_CACHE_FOLDER_NAME); }

        private void Serialize()
        {
            Utilities.JsonSerialization.Serialize(BackingStore, GetStringForEnumCache<T>());
        }

        private static bool Deserialize<A>(out EnumBackingStore<A> enumCache) where A : Enum
        {
            return Utilities.JsonSerialization.Deserialize(out enumCache, GetStringForEnumCache<T>());
        }

        private static string GetStringForEnumCache<A>() where A : Enum
        {
            return Path.Combine(SERIALIZED_ENUM_CACHE_FOLDER, typeof(A).Namespace + typeof(A).Name + ENUM_CACHE_FILE_ENDING);
        }

        private static EnumCache<A> LoadOrCreateInstance<A>() where A : Enum
        {
            bool wasSuccessful = Deserialize(out EnumBackingStore<A> backingStore);

            return new EnumCache<A>()
            {
                BackingStore = wasSuccessful ? backingStore : new EnumBackingStore<A>()
            };
        }
    }
}
