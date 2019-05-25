#if UNITY_EDITOR
using UnityEditor;

public class BuildAssetBundles
{
    [MenuItem("ACMH/Build Asset Bundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Built Asset Bundles/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
#endif