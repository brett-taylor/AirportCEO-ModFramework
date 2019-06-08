using System;
using System.Collections.Generic;
using System.Linq;

namespace ACMF.ModHelper.EnumPatcher
{
    [Serializable]
    internal class EnumBackingStore<T> where T : Enum
    {
        [OdinSerializer.OdinSerialize]
        internal Dictionary<T, string> Enums { get; private set; }
        private T[] EnumsArrayCached = null;

        internal EnumBackingStore()
        {
            Enums = new Dictionary<T, string>();
        }

        internal T IntToEnum(int i)
        {
            return (T)Enum.ToObject(typeof(T), i);
        }

        internal int EnumToInt(Enum t)
        {
            if (t == null)
                return 0;

            return Convert.ToInt32(t);
        }

        private int GetBiggestIDInNativeGame()
        {
            T[] ts = Enum.GetValues(typeof(T)).Cast<T>().ToArray<T>();
            return EnumToInt(ts.Last());
        }

        private int GetNextFreeID()
        {
            if (Enums.Count == 0)
                return GetBiggestIDInNativeGame() + 1;

            return EnumToInt(Enums.Last().Key) + 1;
        }

        internal bool Patch(string name, out T t)
        {
            if (GetEnumViaName(name, out t) == true)
                return false;

            t = IntToEnum(GetNextFreeID());
            Enums.Add(t, name);
            RegenerateEnumCachedArray();
            return true;
        }

        private void RegenerateEnumCachedArray()
        {
            EnumsArrayCached = Enums.Keys.Cast<T>().ToArray();
        }

        internal T[] GetCachedEnumArray()
        {
            if (EnumsArrayCached == null)
                RegenerateEnumCachedArray();

            return EnumsArrayCached;
        }

        private bool GetEnumViaName(string value, out T t)
        {
            foreach(KeyValuePair<T, string> kvp in Enums)
            {
                if (kvp.Value.Equals(value) == true)
                {
                    t = kvp.Key;
                    return true;
                }
            }

            t = default;
            return false;
        }
    }
}