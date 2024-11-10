using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }

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

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        // 可以在这里更新UIController的分数显示
        UIManager.Instance.UpdateScore(Score);
    }

    public void ResetScore(int scoreToReset)
    {
        Score = scoreToReset;
        // 重置分数时也更新UI
        UIManager.Instance.UpdateScore(Score);
    }

    public void AddCombo(int comboToAdd)
    {
        Combo += comboToAdd;
        if (Combo > MaxCombo)
        {
            MaxCombo = Combo;
        }
        // 可以在这里更新UIController的分数显示
        UIManager.Instance.UpdateCombo(Combo);
    }

    public void ResetCombo(int comboToReset)
    {
        Combo = comboToReset;
        // 重置分数时也更新UI
        UIManager.Instance.UpdateCombo(Combo);
    }

}
