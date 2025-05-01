using HarmonyLib;
using LibSoundPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

[HarmonyPatch]
public class SettingsWindowPatch
{
    [HarmonyPatch(typeof(SettingsWindow), "Start")]
    class StartPatch
    {
        static void Postfix(SettingsWindow __instance)
        {
            var panel = __instance.gameObject.transform;
            var appearance = panel.transform.Find("Dialog/Container/Viewport/Content/Settings/Preferences/Appearance");
            if (Networking.IsSinglePlayer())
                panel.transform.Find("Dialog/Container/Viewport/Content/Settings/Preferences/Blank(Clone)").gameObject.SetActive(false);
            var parent = appearance.parent;
            var soundRefresh = GameObject.Instantiate(appearance, parent);
            soundRefresh.name = "Sound";
            var text = soundRefresh.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Reload Sounds";
            var oldButton = soundRefresh.GetComponent<Button>();
            var colors = oldButton.colors;
            Object.DestroyImmediate(oldButton);
            var button = soundRefresh.gameObject.AddComponent<Button>();
            button.colors = colors;
            button.onClick.AddListener(() =>
            {
                AudioManager.Singleton.LoadSounds();
                OS.ShowError("Sounds reloaded!");
            });
            if (Networking.IsSinglePlayer())
                __instance.templateBlankOption.transform.SetAsLastSibling();
            else
                __instance.templateBlankOption.SetActive(false);
        }
    }
}