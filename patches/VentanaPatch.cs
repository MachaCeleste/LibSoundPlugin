using HarmonyLib;
using LibSoundPlugin;

[HarmonyPatch]
public class VentanaPatch
{
    [HarmonyPatch(typeof(Ventana), "Awake")]
    class AwakePatch
    {
        static void Postfix(Ventana __instance)
        {
            if (__instance is ErrorWindow)
                AudioManager.Singleton?.Play(AudioManager.IdSound.Error);
            else
                AudioManager.Singleton?.Play(AudioManager.IdSound.Open);
        }
    }
}