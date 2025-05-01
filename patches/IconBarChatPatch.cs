using HarmonyLib;
using LibSoundPlugin;

[HarmonyPatch]
public class IconBarChatPatch
{
    [HarmonyPatch(typeof(IconBarChat), "GeneralNotification")]
    class GeneralNotificationPatch
    {
        static void Prefix(IconBarChat __instance, ref bool enable)
        {
            if (enable && !__instance.textNotification.activeSelf)
            {
                AudioManager.Singleton?.Play(AudioManager.IdSound.Popup);
            }
        }
    }
}