using System;

namespace ACMF.ModHelper.EnumPatcher
{
    internal interface IEnumCache
    {
        Type TypeActedUpon { get; }
        void EnumPostfix_GetValues(ref Array __result);
        bool EnumPrefix_ToString(Enum __instance, ref string __result);
        bool EnumPrefix_IsDefined(object value, ref bool __result);
        bool EnumPrefix_Parse(string value, bool ignoreCase, ref object __result);
        void EnumPostfix_GetNames(ref Array __result);
    }
}
