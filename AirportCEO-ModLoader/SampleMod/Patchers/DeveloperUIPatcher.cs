using Harmony;
namespace SampleMod.Patchers
{
    [HarmonyPatch(typeof(DevelopmentActionDisplayUI))]
    [HarmonyPatch("Start")]
    public class DeveloperUIPatcher
    {
        [HarmonyPostfix]
        public static void Postfix(DevelopmentActionDisplayUI __instance)
        {
            //UnityEngine.GameObject.Instantiate(__instance.fuelDepotGroup, __instance.transform.parent);
        }
    }
}
