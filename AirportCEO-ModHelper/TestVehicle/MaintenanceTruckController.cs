namespace TestVehicle
{
    public class MaintenanceTruckController : ServiceVehicleController
    {
        public MaintenanceTruckModel MaintenanceTruckModel { get { return GetModel<MaintenanceTruckModel>(); } }

        public override void Initialize()
        {
            if (model == null)
                model = new MaintenanceTruckModel();

            MaintenanceTruckModel.Initialize();
            base.Initialize();
        }
    }
}
