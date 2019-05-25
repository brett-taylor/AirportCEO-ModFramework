using System.IO;
using UnityEngine;

namespace TestVehicle
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
    }
}
