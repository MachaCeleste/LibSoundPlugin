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

        public AudioSource MailSource;
        public AudioClip MailFallback;

        public AudioSource ChatSource;
        public AudioClip ChatFallback;

        public AudioSource OpenSource;
        public AudioClip OpenFallback;

        public AudioSource CloseSource;
        public AudioClip CloseFallback;

        public AudioSource ErrorSource;
        public AudioClip ErrorFallback;

        public AudioSource TraceSource;
        public AudioClip TraceFallback;

        public AudioManager()
        {
            AudioManager.Singleton = this;
        }

        public void Init(GameObject prefab)
        {
            this.MailSource = prefab.transform.Find("MailSound").GetComponent<AudioSource>();
            this.MailFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/mail.wav");

            this.ChatSource = prefab.transform.Find("ChatSound").GetComponent<AudioSource>();
            this.ChatFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/chat.wav");

            this.OpenSource = prefab.transform.Find("OpenSound").GetComponent<AudioSource>();
            this.OpenFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/open.wav");

            this.CloseSource = prefab.transform.Find("CloseSound").GetComponent<AudioSource>();
            this.CloseFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/close.wav");

            this.ErrorSource = prefab.transform.Find("ErrorSound").GetComponent<AudioSource>();
            this.ErrorFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/error.wav");

            this.TraceSource = prefab.transform.Find("TraceSound").GetComponent<AudioSource>();
            this.TraceFallback = BundleTool.GetClip("Assets/LibSoundPlugin/Sounds/error.wav");
        }

        public void Play(IdSound ID)
        {
            switch (ID)
            {
                case IdSound.Mail:
                    if (Plugin.MailSound.Value)
                        this.MailSource.Play();
                    return;
                case IdSound.Chat:
                    if (Plugin.ChatSound.Value)
                        this.ChatSource.Play();
                    return;
                case IdSound.Open:
                    if (Plugin.OpenSound.Value)
                        this.OpenSource.Play();
                    return;
                case IdSound.Close:
                    if (Plugin.CloseSound.Value)
                        this.CloseSource.Play();
                    return;
                case IdSound.Error:
                    if (Plugin.ErrorSound.Value)
                        this.ErrorSource.Play();
                    return;
                case IdSound.Trace:
                    if (Plugin.TraceSound.Value)
                        this.TraceSource.Play();
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
                    case IdSound.Mail:
                        if (clip != null && this.MailSource.clip?.name != clip.name)
                            this.MailSource.clip = clip;
                        else if (clip == null)
                            this.MailSource.clip = this.MailFallback;
                        break;
                    case IdSound.Chat:
                        if (clip != null && this.TraceSource.clip?.name != clip.name)
                            this.TraceSource.clip = clip;
                        else if (clip == null)
                            this.TraceSource.clip = this.TraceFallback;
                        break;
                    case IdSound.Open:
                        if (clip != null && this.OpenSource.clip?.name != clip.name)
                            this.OpenSource.clip = clip;
                        else if (clip == null)
                            this.OpenSource.clip = this.OpenFallback;
                        break;
                    case IdSound.Close:
                        if (clip != null && this.CloseSource.clip?.name != clip.name)
                            this.CloseSource.clip = clip;
                        else if (clip == null)
                            this.CloseSource.clip = this.CloseFallback;
                        break;
                    case IdSound.Error:
                        if (clip != null && this.ErrorSource.clip?.name != clip.name)
                            this.ErrorSource.clip = clip;
                        else if (clip == null)
                            this.ErrorSource.clip = this.ErrorFallback;
                        break;
                    case IdSound.Trace:
                        if (clip != null && this.TraceSource.clip?.name != clip.name)
                            this.TraceSource.clip = clip;
                        else if (clip == null)
                            this.TraceSource.clip = this.TraceFallback;
                        break;
                }
            }
        }

        public enum IdSound
        {
            Mail,
            Chat,
            Open,
            Close,
            Error,
            Trace
        }
    }
}