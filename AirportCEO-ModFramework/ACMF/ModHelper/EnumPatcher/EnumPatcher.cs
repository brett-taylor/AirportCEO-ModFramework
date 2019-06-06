using Harmony;
using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.EnumPatcher
{
    internal static class EnumPatcher
    {
        internal static Dictionary<Type, IEnumCache> ActiveEnumCaches = new Dictionary<Type, IEnumCache>();
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("GetValues")]
    internal class EnumGetValuesPatcher
    {
        [HarmonyPostfix]
        private static void Postfix_GetValues(Type enumType, ref Array __result)
        {
            if (EnumPatcher.ActiveEnumCaches.TryGetValue(enumType, out IEnumCache enumCache))
                enumCache.EnumPostfix_GetValues(ref __result);
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("ToString")]
    [HarmonyPatch(new Type[0])]
    internal class EnumToStringPatcher
    {
        [HarmonyPrefix]
        private static bool Prefix(Enum __instance, ref string __result)
        {
            if (EnumPatcher.ActiveEnumCaches.TryGetValue(__instance.GetType(), out IEnumCache enumCache))
                return enumCache.EnumPrefix_ToString(__instance, ref __result);
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("IsDefined")]
    [HarmonyPatch(new Type[] { typeof(Type), typeof(object) })]
    internal class EnumIsDefinedPatcher
    {
        [HarmonyPrefix]
        private static bool Prefix_IsDefined(Type enumType, object value, ref bool __result)
        {
            if (EnumPatcher.ActiveEnumCaches.TryGetValue(enumType, out IEnumCache enumCache))
                return enumCache.EnumPrefix_IsDefined(value, ref __result);
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("Parse")]
    [HarmonyPatch(new Type[] { typeof(Type), typeof(string), typeof(bool) })]
    internal class EnumParsePatcher
    {
        private static bool Prefix_Parse(Type enumType, string value, bool ignoreCase, ref object __result)
        {
            if (EnumPatcher.ActiveEnumCaches.TryGetValue(enumType, out IEnumCache enumCache))
                return enumCache.EnumPrefix_Parse(value, ignoreCase, ref __result);
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(Enum))]
    [HarmonyPatch("GetNames")]
    [HarmonyPatch(new Type[] { typeof(Type) })]
    internal class EnumGetNamesPatcher
    {
        [HarmonyPostfix]
        private static void Postfix(Type enumType, ref Array __result)
        {
            if (EnumPatcher.ActiveEnumCaches.TryGetValue(enumType, out IEnumCache enumCache))
                enumCache.EnumPostfix_GetNames(ref __result);
        }
    }
}
