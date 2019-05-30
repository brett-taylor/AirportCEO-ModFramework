using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SampleModVehicle
{
    public class Assets
    {
        private static readonly string ASSET_BUNDLE_NAME = "testvehicle";
        public static AssetBundle AssetBundle = null;
        public static GameObject TEST_VEHICLE = null;

        public static void Initialise()
        {
            string assetBundleLocation = Path.Combine(Path.GetDirectoryName(EntryPoint.Mod.Assembly.Location), ASSET_BUNDLE_NAME);
            AssetBundle = AssetBundle.LoadFromFile(assetBundleLocation);
            TEST_VEHICLE = AssetBundle.LoadAsset<GameObject>("TestCar");
        }

        public static GameObject GetGameObjectForTestTruck()
        {
            GameObject testCar = Object.Instantiate(TEST_VEHICLE);
            testCar.transform.SetParent(FolderController.Instance.GetSceneRootTransform(), false);

            //////////////////////////////////////////
            VehicleDoorManager vdm = testCar.transform.Find("Doors").gameObject.AddComponent<VehicleDoorManager>();
            vdm.frontDoorPoints = new List<Transform>();
            vdm.rearDoorPoints = new List<Transform>();
            vdm.cargoDoorPoints = new List<Transform>();
            vdm.transformsToHide = new List<Transform>();
            vdm.frontDoorPoints.Add(testCar.transform.Find("Doors/FrontDoor1"));
            vdm.frontDoorPoints.Add(testCar.transform.Find("Doors/FrontDoor2"));
            vdm.rearDoorPoints.Add(testCar.transform.Find("Doors/RearDoor1"));
            vdm.cargoDoorPoints.Add(testCar.transform.Find("Doors/CargoPoint"));
            vdm.allAccessPoints = new List<Transform>();
            vdm.allAccessPoints.AddRange(vdm.frontDoorPoints);
            vdm.allAccessPoints.AddRange(vdm.rearDoorPoints);
            //////////////////////////////////////////

            //////////////////////////////////////////
            VehicleLightManager vlm = testCar.transform.Find("Lights").gameObject.AddComponent<VehicleLightManager>();
            //////////////////////////////////////////

            //////////////////////////////////////////
            BoundaryHandler bh = testCar.transform.Find("Boundary").gameObject.AddComponent<BoundaryHandler>();
            bh.zoneType = (Enums.ZoneType)4;
            bh.boundaryType = BoundaryHandler.BoundaryType.PersonGrid;
            //////////////////////////////////////////

            //////////////////////////////////////////
            ShadowHandler shadowHandler = testCar.transform.Find("Sprite/Shadow").gameObject.AddComponent<ShadowHandler>();
            shadowHandler.shadowDistance = 0.175f;
            //////////////////////////////////////////

            //////////////////////////////////////////
            VehicleAudioManager vehicleAudio = testCar.transform.Find("Audio").gameObject.AddComponent<VehicleAudioManager>();
            //////////////////////////////////////////

            //////////////////////////////////////////
            TestTruckController scc = testCar.AddComponent<TestTruckController>();
            scc.colorableParts = new SpriteRenderer[] { testCar.transform.Find("Sprite/Chassie").gameObject.GetComponent<SpriteRenderer>() };
            scc.cargoDoors = new Transform[0];
            scc.doorManager = vdm;
            scc.lightManager = vlm;
            scc.audioManager = vehicleAudio;
            scc.exhaust = testCar.GetComponentInChildren<ParticleSystem>();
            scc.shadows = new ShadowHandler[] { shadowHandler };
            scc.boundary = bh;
            scc.thoughtsReferenceList = new List<Thought>();
            scc.currentShipment = new Shipment(Vector3.zero, Enums.DeliveryContainerType.Unspecified, Enums.DeliveryContentType.Unspecified, 0, "");
            scc.currentActionDescriptionListReference = new List<Enums.ServiceVehicleAction>();
            scc.gameObject.SetActive(false);
            scc.transform.position = Vector3.zero;
            //////////////////////////////////////////

            return testCar;
        }
    }
}
