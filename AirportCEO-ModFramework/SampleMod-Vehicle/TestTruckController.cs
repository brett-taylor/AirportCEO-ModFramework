using System.Collections;

namespace SampleModVehicle
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

            TestTruckModel.Initialize();
            base.Initialize();
        }

        public override void Launch()
        {
            base.Launch();
            StartCoroutine(ActivityDispatcher(null));
        }

        public override void Reset()
        {
            TestTruckModel.Reset();
            base.Reset();
        }

        public override IEnumerator PerformWork()
        {
            ACMF.ModHelper.DialogPopup.DialogManager.QueueMessagePanel("test test");
            yield break;
        }
    }
}
