using Harmony;
using UnityEngine;

namespace SampleMod.Patchers
{
    [HarmonyPatch(typeof(SelectionController))]
    [HarmonyPatch("SelectClickableObject")]
    public class ClickVehiclePatcher
    {
        [HarmonyPostfix]
        public static void Postfix(IGameWorldClickHandler clickableObject, bool shouldLoadPanel)
        {
            if (clickableObject is PlaceableItem)
            {
                PlaceableItem vehicleController = (PlaceableItem) clickableObject;
                ACMH.Utilities.Logger.ShowDialog($"You clicked on a: {vehicleController.gameObject.name}");

                System.Console.WriteLine("");
                foreach (Component c in vehicleController.gameObject.GetComponentsInChildren<Component>())
                {
                    System.Console.WriteLine($"Object: {c.gameObject.name} has {c.GetType().ToString()}");
                }
                System.Console.WriteLine("");
            }
        }
    }
}
