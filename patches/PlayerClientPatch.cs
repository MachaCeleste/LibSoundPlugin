using HarmonyLib;
using LibSoundPlugin;
using UnityEngine;

[HarmonyPatch]
public class PlayerClientPatch
{
    [HarmonyPatch(typeof(PlayerClient), "ConnectToServer")]
    class ConnectToServerPatch
    {
        static void Postfix()
        {
            var mixer = GameObject.Find("Mixer");
            AudioManager.Singleton.Init(mixer);
            AudioManager.Singleton.LoadSounds();
        }
    }
}