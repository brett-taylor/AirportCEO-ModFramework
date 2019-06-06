using System;
using System.Collections.Generic;
using System.Linq;

namespace ACMF.ModHelper.EnumPatcher
{
    internal class EnumBackingStore<T> where T : Enum
    {
        internal Dictionary<T, string> Enums { get; private set; }
        private T[] EnumsArrayCached = null;

        internal EnumBackingStore()
        {
            Enums = new Dictionary<T, string>();
        }

        internal T IntToEnum(int i)
        {
            return (T) Enum.ToObject(typeof(T), i);
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

        internal T Patch(string name)
        {
            EnumsArrayCached = null;

            T newEnum = IntToEnum(GetNextFreeID());
            Enums.Add(newEnum, name);
            return newEnum;
        }

        internal T[] GetCachedEnumArray()
        {
            if (EnumsArrayCached == null)
                EnumsArrayCached = Enums.Keys.Cast<T>().ToArray();

            return EnumsArrayCached;
        }
    }
}