using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.EnumPatcher
{
    public class EnumCache<T> : IEnumCache where T : Enum
    {
        private static EnumCache<T> InternalInstance = null;
        public static EnumCache<T> Instance
        {
            get
            {
                if (InternalInstance == null)
                    InternalInstance = new EnumCache<T>();

                return InternalInstance;
            }
        }

        private EnumBackingStore<T> BackingStore = new EnumBackingStore<T>();
        public Type TypeActedUpon { get => typeof(T); }

        private EnumCache()
        {
            EnumPatcher.ActiveEnumCaches.Add(typeof(T), this);
        }

        public T Patch(string name)
        {
            return BackingStore.Patch(name);
        }

        public void EnumPostfix_GetValues(ref Array __result)
        {
            List<T> listArray = new List<T>();
            foreach (T productType in __result)
                listArray.Add(productType);

            listArray.AddRange(BackingStore.GetCachedEnumArray());
            __result = listArray.ToArray();
        }

        public bool EnumPrefix_ToString(Enum __instance, ref string __result)
        {

            if (BackingStore.Enums.ContainsKey((T) __instance) == true)
            {
                __result = BackingStore.Enums[(T) __instance];
                return false;
            }

            return true;
        }

        public bool EnumPrefix_IsDefined(object value, ref bool __result)
        {
            bool contained = BackingStore.Enums.ContainsKey((T) value);
            __result = contained;

            return !contained;
        }

        public bool EnumPrefix_Parse(string value, bool ignoreCase, ref object __result)
        {
            StringComparison stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            foreach (KeyValuePair<T, string> kvp in BackingStore.Enums)
            {
                if (string.Equals(value, kvp.Value, stringComparison) == true)
                {
                    __result = kvp.Key;
                    return false;
                }
            }

            return true;
        }

        public void EnumPostfix_GetNames(ref Array __result)
        {
            List<string> listArray = new List<string>();
            foreach (string vehicleType in __result)
                listArray.Add(vehicleType);

            listArray.AddRange(BackingStore.Enums.Values);
            __result = listArray.ToArray();
        }
    }
}
