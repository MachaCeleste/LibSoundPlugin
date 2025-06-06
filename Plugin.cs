using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace LibSoundPlugin;

[BepInPlugin("com.machaceleste.libsoundplugin", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static ConfigEntry<bool> MailSound;
    public static ConfigEntry<bool> ChatSound;
    public static ConfigEntry<bool> OpenSound;
    public static ConfigEntry<bool> CloseSound;
    public static ConfigEntry<bool> ErrorSound;
    public static ConfigEntry<bool> TraceSound;
        
    private void Awake()
    {
        MailSound = Config.Bind("Enable Sound", "Mail Sound", true, "Enables mail sound");
        ChatSound = Config.Bind("Enable Sound", "Chat Sound", true, "Enables chat sound");
        OpenSound = Config.Bind("Enable Sound", "Open Sound", true, "Enables open sound");
        CloseSound = Config.Bind("Enable Sound", "Minimize Sound", true, "Enables minimize sound");
        ErrorSound = Config.Bind("Enable Sound", "Error Sound", true, "Enable error sound");
        TraceSound = Config.Bind("Enable Sound", "Trace Sound", true, "Enable trace sound");

        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        AssetBundleTools.BundleTool.LoadBundle();
        var harmony = new Harmony("com.machaceleste.libsoundplugin");
        harmony.PatchAll();
    }
}