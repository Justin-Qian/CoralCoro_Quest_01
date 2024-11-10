using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class SoundPanelController : MonoBehaviour
{

    public GameObject okButton; // Reimplement!
    public TextMeshPro instructionText;

    private AudioClip recordedClip;
    private AudioSource audioSource;
    private bool isRecording = false;
    private string[] notes = { "Do", "Re", "Mi", "Fa", "Sol", "La", "Ti" };
    private int currentNoteIndex = 0;

    void Start()
    {
        instructionText.text = "";
        okButton.SetActive(false);
    }

    public void StartRecording()
    {
        if (!isRecording && currentNoteIndex < notes.Length)
        {
            StartCoroutine(StartRecordingWithCountdown());
        }
    }

    private IEnumerator StartRecordingWithCountdown()
    {
        instructionText.text = $"请在倒数三秒后发出{notes[currentNoteIndex]}的声音, 结束录制按OK按钮";
        yield return new WaitForSeconds(2);
        instructionText.text = "3";
        yield return new WaitForSeconds(1);
        instructionText.text = "2";
        yield return new WaitForSeconds(1);
        instructionText.text = "1";

        recordedClip = Microphone.Start(null, false, 10, 44100);
        isRecording = true;
        instructionText.text = $"正在录制{notes[currentNoteIndex]}的声音";
        okButton.SetActive(true);
        Debug.Log("Recording started...");
    }

    public void OkButtonClicked()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            SaveRecording();
            instructionText.text = ""; // 清空文本
            currentNoteIndex++;
            if (currentNoteIndex < notes.Length)
            {
                StartRecording();
            }
            else
            {
                instructionText.text = "All notes recorded!";
                okButton.SetActive(false);
            }
        }
    }

    public void RedoRecording()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            Debug.Log("Recording stopped for redo...");
        }
        currentNoteIndex = 0; // 重置到第一个音阶
        StartRecording();
    }

    public void SaveRecording()
    {
        if (recordedClip != null)
        {
            string filename = $"{currentNoteIndex + 1}.wav";
            string folderPath = Path.Combine(Application.streamingAssetsPath, "Recordings");
            Directory.CreateDirectory(folderPath);
            string filepath = Path.Combine(folderPath, filename);
            WavUtility.Save(filepath, recordedClip); // 假设你有一个WavUtility类来保存录音到文件
            Debug.Log($"Recording saved to {filepath}");
        }
    }

    public void PlayRecording()
    {
        if (recordedClip != null)
        {
            audioSource.clip = recordedClip;
            audioSource.Play();
            Debug.Log("Playing recording...");
        }
    }

}
