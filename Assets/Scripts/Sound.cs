using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    private AudioSource audioSource;
    private readonly Queue<string> audioFileNameQueue = new Queue<string>();

    public void SetAudioFileNames(IEnumerable<string> audioFileNames)
    {
        Debug.Log($"Setting the following fileNames for {name}: {string.Join(", ", audioFileNames)}");
        foreach (var audioFileName in audioFileNames)
        {
            audioFileNameQueue.Enqueue(audioFileName);
        }
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();        
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (!audioSource.isPlaying &&
            other.gameObject.GetComponent<CharacterController>() != null &&
            audioFileNameQueue.Count > 0)
        {
            Debug.Log("Collider triggered, playing next audio clip.");
            var vuMeter = other.gameObject.GetComponent<VUMeter>();
            vuMeter.audioSource = audioSource;

            await PlayNextClipAsync();
        }
    }

    private async Task PlayNextClipAsync()
    {
        var fileName = audioFileNameQueue.Dequeue();
        Debug.Log($"Playing {fileName}.");
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
                
        var audioClip = await LoadClipAsync(path);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private async Task<AudioClip> LoadClipAsync(string path)
    {
        AudioClip clip = null;

        using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            unityWebRequest.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!unityWebRequest.isDone) await Task.Delay(5);

                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log($"Web request error while loading audio clip: {unityWebRequest.error}");
                }
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception while loading audio clip: {ex.Message}, {ex.StackTrace}");
            }
        }

        return clip;
    }
}
