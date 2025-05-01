using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LibSoundPlugin;

[BepInPlugin("com.greyhack.libsoundplugin", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
        
    private void Awake()
    {
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        AssetBundleTools.BundleTool.LoadBundle("asset.bundle");
        var harmony = new Harmony("com.greyhack.libsoundplugin");
        harmony.PatchAll();
    }
}