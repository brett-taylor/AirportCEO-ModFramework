using System.IO;
using UnityEngine;

namespace ACMF.ModHelper.Utilities
{
    public class Assets
    {
        private static readonly string ASSET_BUNDLE_NAME = "acmf";
        public static AssetBundle AssetBundle = null;
        public static GameObject MAIN_MENU_VERSION_TEXT = null;

        public static void Initialise()
        {
            string assetBundleLocation = Path.Combine(ACMF.ACMFFolderLocation, ASSET_BUNDLE_NAME);
            AssetBundle = AssetBundle.LoadFromFile(assetBundleLocation);
            MAIN_MENU_VERSION_TEXT = AssetBundle.LoadAsset<GameObject>("ACMF-Info");
        }
    }
}
