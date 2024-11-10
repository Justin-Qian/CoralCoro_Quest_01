﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject feedbackTextPrefab;
    public TextMeshPro scoreText;
    public TextMeshPro comboText;

    public TextMeshPro heightText;
    public TextMeshPro finalscore;
    public TextMeshPro finalcombomax;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保证单例在场景加载时不被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 更新分数显示
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    // 更新连击数显示
    public void UpdateCombo(int combo)
    {
        comboText.text = "Combo: " + combo;
    }

    public void UpdateHeight()
    {
        MusicBlock.origin.y = InputManager.Instance.GetHeadPosition().y;
        float height = float.Parse(MusicBlock.origin.y.ToString("F2")); // 保留两位小数
        heightText.text = "Current Height: " + height + "m";
    }

    public void ShowFeedbackText(string message, float duration, Vector3 position, Quaternion rotation)
    {
        StartCoroutine(ShowFeedbackTextCoroutine(message, duration, position, rotation));
    }

    private IEnumerator ShowFeedbackTextCoroutine(string message, float duration,Vector3 position, Quaternion rotation)
    {
        GameObject textObject = Instantiate(feedbackTextPrefab, position, rotation);
        TextMeshPro feedbackText = textObject.GetComponent<TextMeshPro>();
        feedbackText.text = message;
        yield return new WaitForSeconds(duration);
        feedbackText.text = "";
        Destroy(textObject);
    }

    public void UpdateFinalScore()
    {
        finalscore.text = "Your Score: " + ScoreManager.Instance.Score;
        finalcombomax.text = "Your Max Combo: " + ScoreManager.Instance.MaxCombo;
    }
}