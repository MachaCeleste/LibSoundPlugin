using HarmonyLib;
using LibSoundPlugin;

[HarmonyPatch]
public class ComputerPatch
{
    [HarmonyPatch(typeof(Computer), "CloseProgramClient")]
    class CloseProgramClientPatch
    {
        static void Postfix()
        {
            AudioManager.Singleton?.Play(AudioManager.IdSound.Minimize);
        }
    }
}