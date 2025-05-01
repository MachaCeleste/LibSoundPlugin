using AssetBundleTools;
using BepInEx;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace LibSoundPlugin
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Singleton;

        public AudioSource ConfirmSource;
        public AudioClip ConfirmFallback;

        public AudioSource MailSource;
        public AudioClip MailFallback;

        public AudioSource OpenSource;
        public AudioClip OpenFallback;

        public AudioSource PopupSource;
        public AudioClip PopupFallback;

        public AudioSource MinimizeSource;
        public AudioClip MinimizeFallback;

        public AudioSource ErrorSource;
        public AudioClip ErrorFallback;

        //public AudioSource PcSpeakerSource;

        public AudioManager()
        {
            AudioManager.Singleton = this;
        }

        public void Init(GameObject prefab)
        {
            this.ConfirmSource = prefab.transform.Find("ConfirmSound").GetComponent<AudioSource>();
            this.ConfirmFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/confirm.wav");

            this.MailSource = prefab.transform.Find("MailSound").GetComponent<AudioSource>();
            this.MailFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/mail.wav");

            this.OpenSource = prefab.transform.Find("OpenSound").GetComponent<AudioSource>();
            this.OpenFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/open.wav");

            this.PopupSource = prefab.transform.Find("PopupSound").GetComponent<AudioSource>();
            this.PopupFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/popup.wav");

            this.MinimizeSource = prefab.transform.Find("MinimizeSound").GetComponent<AudioSource>();
            this.MinimizeFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/minimize.wav");

            this.ErrorSource = prefab.transform.Find("ErrorSound").GetComponent<AudioSource>();
            this.ErrorFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/faitalerror.wav");

            //this.PcSpeakerSource = prefab.transform.Find("PcSpeakerSound").GetComponent<AudioSource>();
        }

        public void Play(IdSound ID)
        {
            switch (ID)
            {
                case IdSound.Confirm:
                    this.ConfirmSource.Play();
                    return;
                case IdSound.Mail:
                    this.MailSource.Play();
                    return;
                case IdSound.Open:
                    this.OpenSource.Play();
                    return;
                case IdSound.Popup:
                    this.PopupSource.Play();
                    return;
                case IdSound.Minimize:
                    this.MinimizeSource.Play();
                    return;
                case IdSound.Error:
                    this.ErrorSource.Play();
                    return;
                case IdSound.Spk:
                    //this.PcSpeakerSource.Play();
                    return;
            }
        }

        public void LoadSounds()
        {
            StartCoroutine(GetSounds());
        }

        private IEnumerator GetSounds()
        {
            var soundsPath = Path.Combine(Paths.PluginPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Sounds");
            if (!Directory.Exists(soundsPath))
            {
                Directory.CreateDirectory(soundsPath);
            }
            var readme = Path.Combine(soundsPath, "ReadMe.txt");
            string data = "To use this mod with custom sounds place the files in this folder named appropriately.\nFiles can be either \".wav\", \".ogg\" or \".mp3\"\nFile names:\n";
            foreach (IdSound ID in Enum.GetValues(typeof(IdSound)))
            {
                data += $"\n{ID.ToString()}";
            }
            File.WriteAllText(readme, data);
            string[] extensions = new string[]
            {
                ".wav",
                ".mp3",
                ".ogg"
            };
            foreach (IdSound ID in Enum.GetValues(typeof(IdSound)))
            {
                if (ID > IdSound.Error)
                    yield break;
                AudioClip clip = null;
                for (int i = 0; i < extensions.Length; i++)
                {
                    string soundPath = Path.Combine(soundsPath, $"{ID}{extensions[i]}");
                    if (File.Exists(soundPath))
                    {
                        var uri = new System.Uri(soundPath).AbsoluteUri;
                        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.UNKNOWN))
                        {
                            yield return www.SendWebRequest();
                            if (www.result != UnityWebRequest.Result.Success)
                            {
                                Plugin.Logger.LogError($"Failed to load audio: {www.error} - Path: {uri}");
                            }
                            else
                            {
                                clip = DownloadHandlerAudioClip.GetContent(www);
                                clip.name = $"Custom{Path.GetFileNameWithoutExtension(soundPath)}";
                                if (clip == null || clip.length == 0)
                                {
                                    Plugin.Logger.LogError($"Error loading audio: {www.error}");
                                    yield break;
                                }
                            }
                        }
                    }
                }
                switch (ID)
                {
                    case IdSound.Confirm:
                        if (clip != null && this.ConfirmSource.clip?.name != clip.name)
                            this.ConfirmSource.clip = clip;
                        else if (clip == null)
                            this.ConfirmSource.clip = this.ConfirmFallback;
                        break;
                    case IdSound.Mail:
                        if (clip != null && this.MailSource.clip?.name != clip.name)
                            this.MailSource.clip = clip;
                        else if (clip == null)
                            this.MailSource.clip = this.MailFallback;
                        break;
                    case IdSound.Open:
                        if (clip != null && this.OpenSource.clip?.name != clip.name)
                            this.OpenSource.clip = clip;
                        else if (clip == null)
                            this.OpenSource.clip = this.OpenFallback;
                        break;
                    case IdSound.Popup:
                        if (clip != null && this.PopupSource.clip?.name != clip.name)
                            this.PopupSource.clip = clip;
                        else if (clip == null)
                            this.PopupSource.clip = this.PopupFallback;
                        break;
                    case IdSound.Minimize:
                        if (clip != null && this.MinimizeSource.clip?.name != clip.name)
                            this.MinimizeSource.clip = clip;
                        else if (clip == null)
                            this.MinimizeSource.clip = this.MinimizeFallback;
                        break;
                    case IdSound.Error:
                        if (clip != null && this.ErrorSource.clip?.name != clip.name)
                            this.ErrorSource.clip = clip;
                        else if (clip != null)
                            this.ErrorSource.clip = this.ErrorFallback;
                        break;
                    case IdSound.Spk:
                        break;
                }
            }
        }

        public enum IdSound
        {
            Confirm,
            Mail,
            Open,
            Popup,
            Minimize,
            Error,
            Spk
        }
    }
}