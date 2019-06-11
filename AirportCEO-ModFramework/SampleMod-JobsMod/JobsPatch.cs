using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace SampleMod_JobsMod
{
    class ActionPatch
    {
        //Currently a lot of these methods are commented out as example code.
        //What these example methods do is allow replacing actions globally, 
        //replacing the methods that actions call and replacing the actions
        //enqueued by an activity.

        //The Structure of service vehicles is:
        //Activity > Action > ActionOrders
        //Current Activity > Actions to complete that activity > Methods to complete those actions
        //An activity is a list of actions and an action has a 1-1 translation of action orders.
        
        //This'll be quite easy to make generic with a dictionary to allow the api to
        //customise default vehicle actions and make new ones.

        //I'm currently trying to figure out how activities are set which I believe,
        //Is determined by the job system.

        //May be useful for new actions.
        [HarmonyPatch(typeof(ServiceVehicleModel))]
        [HarmonyPatch("AddToActionList")]
        public class ServiceVehicleModel_AddToActionList_Patch
        {
            //Overwriting of basic actions
            [HarmonyPrefix]
            public static bool Prefix(ServiceVehicleModel __instance,Enums.ServiceVehicleAction action)
            {
                /*if (action == Enums.ServiceVehicleAction.PassCheckPoint)
                {
                    //Currently replaces the action "pass check point" with "move to world exit".
                    //Making vehicles exit the world as soon as they spawn.
                    __instance.AddToActionList(Enums.ServiceVehicleAction.MoveToWorldExit);
                    return false;
                } else
                {
                    return true;
                }*/
                return true;
            }
        }

        
        [HarmonyPatch(typeof(ServiceVehicleController))]
        [HarmonyPatch("GenerateActionList")]
        public class ServiceVehicleController_GenerateActionList_Patch
        {
            //Overwriting of basic activities
            [HarmonyPrefix]
            public static bool Prefix(ServiceVehicleController __instance)
            {
                /*
                ServiceVehicleModel mainModel = __instance.ServiceVehicleModel;
                if (mainModel.currentActivity == Enums.ServiceVehicleActivity.GoingToAirport)
                {
                    mainModel.AddToActionList(Enums.ServiceVehicleAction.MoveToCheckPoint);
                    mainModel.AddToActionList(Enums.ServiceVehicleAction.MoveToWorldExit);
                    mainModel.RemoveActionDuplicates();
                    return false;
                }
                else
                {
                    return true;
                }*/
                return true;
            }
        }

        [HarmonyPatch(typeof(ServiceVehicleController))]
        [HarmonyPatch("ConvertDescriptionsToActions")]
        public class ServiceVehicleModel_ConvertDescriptionsToActions_Patch
        {
            //Overwriting of basic activities
            [HarmonyPrefix]
            public static void Postfix(ServiceVehicleController __instance, List<ActionOrder> __result)
            {
                //An implementation dependent on a not implemented global Dict<vehicleType, Dict<>>,
                //After getting the dict in the values array it uses the indexes of the currentActionDescList
                //And replaces any actions in the actionorder that are defined in the dict.
                /*
                ServiceVehicleModel mainModel = __instance.ServiceVehicleModel;
                Dictionary<Enums.ServiceVehicleAction, ActionOrder> patchedActions = null;
                if(actionsDict.TryGetValue(__instance.VehicleType), patchedActions)) 
                {
                    for(int i = 0; i < mainModel.currentActionDescriptionList.Count; i++)
                    {
                        ActionOrder tempVal;
                        patchedActions.TryGetValue(mainModel.currentActionDescriptionList[i], out tempVal);
                        __result[i] = tempVal;   
                    }
                }*/
            }
        }
    }
}
