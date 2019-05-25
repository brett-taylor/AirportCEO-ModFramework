using System;
using System.Collections;

namespace TestVehicle
{
    public class TestTruckController : ServiceVehicleController
    {
        public override void Initialize()
        {
            model = new TestTruckModel();
            resetAction = new Action<Enums.ServiceVehicleActivity, bool, string>(ResetCarModel);
            model.Initialize();
            base.Initialize();
        }

        public override void Launch()
        {
            base.Launch();
            StartCoroutine(ActivityDispatcher(null));
            GetComponentInChildren<VehicleLightManager>().ToggleWarningLights(true);
        }

        private void ResetCarModel(Enums.ServiceVehicleActivity serviceVehicleActivity, bool activityUnsuccessful, string evaluationMessage)
        {
        }

        public override void Reset()
        {
            model.Reset();
            base.Reset();
        }

        public override IEnumerator PerformWork()
        {
            ACMH.Utilities.Logger.ShowDialog("test test");
            yield break;
        }
    }
}
