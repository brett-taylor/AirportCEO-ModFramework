using Harmony;
using UnityEngine;

namespace ACMF.ModHelper.Camera
{
    [HarmonyPatch(typeof(CameraController))]
    [HarmonyPatch("InitializeCamera")]
    public class UnboundedCamera
    {
        [HarmonyPostfix]
        public static void Postfix(CameraController __instance)
        {
            if (ACMF.Config.UNBOUNDED_CAMERA == false)
                return;

            __instance.genericMoveCamera.XRangeMin = int.MinValue;
            __instance.genericMoveCamera.XRangeMax = int.MaxValue;
            __instance.genericMoveCamera.YRangeMin = int.MinValue;
            __instance.genericMoveCamera.YRangeMax = int.MaxValue;
            __instance.genericMoveCamera.ZRangeMin = -2000f;
            __instance.genericMoveCamera.ZRangeMax = -2f;
            __instance.mainCamera.nearClipPlane = 2f;
            __instance.mainCamera.farClipPlane = 2100f;
        }
    }
}
