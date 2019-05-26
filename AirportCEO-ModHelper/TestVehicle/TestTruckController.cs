using System;
using System.Collections;

namespace TestVehicle
{
    public class TestTruckController : ServiceVehicleController
    {
        public TestTruckModel TestTruckModel
        {
            get
            {
                return GetModel<TestTruckModel>();
            }
        }

        public override void Initialize()
        {
            if (model == null)
            {
                model = new TestTruckModel();
            }

            resetAction = new Action<Enums.ServiceVehicleActivity, bool, string>(ResetCarModel);
            TestTruckModel.Initialize();
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
            TestTruckModel.Reset();
            base.Reset();
        }

        public override IEnumerator PerformWork()
        {
            ACMH.Utilities.Logger.ShowDialog("test test");
            yield break;
        }
    }
}
