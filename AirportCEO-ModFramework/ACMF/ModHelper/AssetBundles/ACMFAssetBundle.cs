using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ACMF.ModHelper.AssetBundles
{
    public abstract class ACMFAssetBundle
    {
        public AssetBundle AssetBundle { get; private set; }
        public Dictionary<string, GameObject> GameObjects { get; private set; }

        protected abstract string AssetBundleName { get; }
        protected abstract string AssetBundleLocation { get; }
        protected abstract bool InSameDirectoryAsDLL { get; }

        public ACMFAssetBundle()
        {
            GameObjects = new Dictionary<string, GameObject>();
            string assetBundleFileLocation = GetAssetBundleLocation(Assembly.GetCallingAssembly().Location);
            if (DoesAssetBundleExist(assetBundleFileLocation) == false)
            {
                Utilities.Logger.Error($"{GetType().FullName} Attempted to load a non-existent asset bundle at {assetBundleFileLocation}");
                return;
            }

            LoadAssetBundle(assetBundleFileLocation);
        }

        private bool DoesAssetBundleExist(string assetBundleLocation)
        {
            return File.Exists(assetBundleLocation);
        }

        private void LoadAssetBundle(string assetBundleLocation)
        {
            try
            {
                AssetBundle = AssetBundle.LoadFromFile(assetBundleLocation);
                foreach(GameObject gameObject in AssetBundle.LoadAllAssets<GameObject>())
                {
                    GameObjects.Add(gameObject.name, gameObject);
                }
            }
            catch (Exception e)
            {
                Utilities.Logger.Error($"Failed to load {AssetBundleName} : {e.ToString()}");
            }
        }

        private string GetAssetBundleLocation(string dllLocation)
        {
            if (InSameDirectoryAsDLL)
            {
                string assemblyDirectory = Path.GetDirectoryName(dllLocation);
                return Path.Combine(assemblyDirectory, AssetBundleName);
            }
            else
            {
                return Path.Combine(AssetBundleLocation, AssetBundleName);
            }
        }

        public GameObject AttemptLoadGameObject(string name)
        {
            return GameObjects.ContainsKey(name) ? GameObjects[name] : null;
        }
    }
}
