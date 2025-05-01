using BepInEx;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AssetBundleTools
{
    public class BundleTool
    {
        public static AssetBundle bundle;

        public static void LoadBundle(string name)
        {
            Debug.Log($"Loading asset bundle {name}");
            string bundlePath = Path.Combine(Paths.PluginPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            if (!File.Exists(bundlePath))
            {
                Debug.LogError($"AssetBundleTool: {name} not found!");
                return;
            }
            bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null)
            {
                Debug.LogError($"AssetBundleTool: Failed to load {name}!");
                return;
            }
            Debug.Log($"AssetBundleTool: Loaded {name}");
        }

        public static GameObject GetPrefab(string path)
        {
            var prefab = bundle.LoadAsset<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"AssetBundleTool: Failed to load prefab {path}");
                return null;
            }
            return prefab;
        }

        public static AudioClip GetClip(string path)
        {
            var clip = bundle.LoadAsset<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogError($"AssetBundleTool: Failed to load audio clip {path}");
                return null;
            }
            return clip;
        }
    }
}