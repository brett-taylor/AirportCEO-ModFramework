using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestVehicle
{
    public class MaintenanceTruckController : ServiceVehicleController
    {
        public VehicleModel VehicleModelBlah { get { return GetModel<ServiceCarModel>(); } }

        public override void Initialize()
        {
            if (model == null)
                model = new ServiceCarModel();

            colorableParts = new SpriteRenderer[0];

            doorManager = gameObject.AddComponent<VehicleDoorManager>();
            doorManager.frontDoorPoints = new List<Transform>();
            doorManager.rearDoorPoints = new List<Transform>();
            doorManager.cargoDoorPoints = new List<Transform>();
            doorManager.allAccessPoints = new List<Transform>();
            doorManager.transformsToHide = new List<Transform>();
            cargoDoors = new Transform[0];

            resetAction = new Action<Enums.ServiceVehicleActivity, bool, string>(ResetCarModel);
            VehicleModelBlah.Initialize();
            base.Initialize();
        }

        public override void Launch()
        {
            base.Launch();
            StartCoroutine(ActivityDispatcher(null));
        }

        private void ResetCarModel(Enums.ServiceVehicleActivity serviceVehicleActivity, bool activityUnsuccessful, string evaluationMessage)
        {
        }
    }
}
