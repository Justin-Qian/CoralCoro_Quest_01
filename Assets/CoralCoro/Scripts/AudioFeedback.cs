using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioFeedback : MonoBehaviour
{
    public bool useRecorded = false;
    public AudioClip[] DefaultClips;  // 这个数组会在Inspector中进行分配
    public AudioClip[] RecordedClips;  // 用于存储音频文件的数组
    private AudioClip[] audioClips;  // 用于存储音频文件的数组
    private AudioSource pitchAudioSource;

    void Start()
    {
        // 添加AudioSource组件用于播放pitch音频
        pitchAudioSource = gameObject.AddComponent<AudioSource>();
        RecordedClips = DefaultClips;  // 初始化RecordedClips数组，用defaultClips数组填充
    }

    // 调用这个函数来播放对应pitch的音频
    public void PlayAudio(int pitch)
    {
        if (useRecorded)
        {
            audioClips = RecordedClips;  // 如果已经录制了音频文件，就使用录制的音频文件
        }
        else
        {
            audioClips = DefaultClips;  // 如果没有录制音频文件，就使用默认音频文件
        }

        if (pitch >= 1 && pitch <= 8)
        {
            pitchAudioSource.clip = audioClips[pitch - 1];  // 由于数组是从0开始的，所以pitch需要减1
            pitchAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Pitch out of range. It should be between 1 and 8.");
        }
    }

    // 调用这个函数来更新
    // public void UpdateAudioClips()
    // {
    //     string recordingsPath = Path.Combine(Application.streamingAssetsPath, "Recordings");

    //     if (Directory.Exists(recordingsPath))
    //     {
    //         Debug.LogWarning("reset audioClips start");

    //         for (int i = 0; i < 7; i++)
    //         {
    //             string filePath = Path.Combine(recordingsPath, $"{i + 1}.wav");  // 构建文件路径

    //             if (File.Exists(filePath))
    //             {
    //                 AudioClip clip = WavUtility.Load(filePath);  // 从文件加载音频文件
    //                 if (clip != null)
    //                 {
    //                     RecordedClips[i] = clip;  // 将加载的音频文件赋值给数组
    //                     Debug.LogWarning($"{i + 1}.wav has been reset");
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError($"Failed to load {i + 1}.wav");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError($"File not found: {filePath}");
    //             }
    //         }

    //         audioClips = RecordedClips;  // 将加载的音频文件赋值给audioClips数组
    //     }
    //     else
    //     {
    //         Debug.LogWarning("No 'Recordings' directory found, using default clips.");
    //     }
    // }

    
}
