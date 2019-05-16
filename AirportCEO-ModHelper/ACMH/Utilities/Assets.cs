using System.IO;
using UnityEngine;

namespace ACMH.Utilities
{
    public class Assets
    {
        private static readonly string ASSET_BUNDLE_NAME = "acmh";
        public static AssetBundle AssetBundle = null;
        public static GameObject MAIN_MENU_VERSION_TEXT = null;

        public static void Initialise()
        {
            string assetBundleLocation = Path.Combine(Path.GetDirectoryName(ACMH.Mod.Assembly.Location), ASSET_BUNDLE_NAME);
            AssetBundle = AssetBundle.LoadFromFile(assetBundleLocation);
            MAIN_MENU_VERSION_TEXT = AssetBundle.LoadAsset<GameObject>("ACML_H-Info");
        }
    }
}
