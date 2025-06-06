using HarmonyLib;
using LibSoundPlugin;

[HarmonyPatch]
public class NotificationsPatch
{
    [HarmonyPatch(typeof(Notifications), "ShowNotification")]
    class ShowNotificationPatch
    {
        static void Postfix(Notifications __instance, ref string message, ref Notifications.NotifType notifType, ref bool openProgram)
        {
            switch (notifType)
            {
                case Notifications.NotifType.MAIL:
                    AudioManager.Singleton?.Play(AudioManager.IdSound.Mail);
                    break;
                default:
                    break;
            }
        }
    }
}