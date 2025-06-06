using HarmonyLib;
using LibSoundPlugin;

[HarmonyPatch]
public class AdminMonitorPatch
{
    [HarmonyPatch(typeof(AdminMonitor), "CuentaAtras")]
    class CuentaAtrasPatch
    {
        static void Prefix()
        {
            AudioManager.Singleton?.Play(AudioManager.IdSound.Trace);
        }
    }
}