using ACMF.ModHelper.ModPrefabs.Vehicles;
using UnityEngine;

namespace SampleModVehicle
{
    public class TestTruckCreator : ServiceVehicleCreator<TestTruckController>
    {
        private static TestTruckCreator InstanceInternal = null;
        public static TestTruckCreator Instance
        {
            get
            {
                if (InstanceInternal == null)
                    InstanceInternal = new TestTruckCreator();

                return InstanceInternal;
            }
        }

        protected override GameObject VehicleGameObject => Assets.Instance.AttemptLoadGameObject("TestTruck");
        protected override string[] FrontDoorTransforms => new string[2] { "Doors/FrontDoor1", "Doors/FrontDoor2" };
        protected override string[] RearDoorTransforms => new string[0];
        protected override string[] CargoDoorTransforms => new string[0];
        protected override string[] ColorableSpriteRenderers => new string[1] { "Sprite/Chassie" };

        protected override void PostCreateInstance(GameObject vehicle) { }
        private TestTruckCreator() { }
    }
}
