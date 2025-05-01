using HarmonyLib;
using AssetBundleTools;
using UnityEngine;
using LibSoundPlugin;

[HarmonyPatch]
public class BiosMenuPatch
{
    [HarmonyPatch(typeof(BiosMenu), "Start")]
    class StartPatch
    {
        static void Postfix()
        {
            var prefab = BundleTool.GetPrefab("Assets/LibSoundPlugin/Mixer.prefab");
            if (prefab == null)
            {
                return;
            }
            var mixerObj = Object.Instantiate(prefab);
            mixerObj.name = "Mixer";
            mixerObj.AddComponent<AudioManager>();
        }
    }
}