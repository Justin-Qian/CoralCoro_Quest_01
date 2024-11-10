using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public AudioSource musicSource;
    public Spawner spawner;
    public MenuController menuController;
    public GameObject gameasset;

    private bool hasStarted = false;
    public void GameStart()
    {

        if (!hasStarted)
        {
            menuController.HideMenu(); // 隐藏菜单
            menuController.ShowScorePanel(); // 显示分数面板

            gameasset.SetActive(true);
            ScoreManager.Instance.ResetScore(0); // 重置分数
            ScoreManager.Instance.ResetCombo(0); // 重置连击数
            musicSource.time = 0; // 重置音乐时间
            musicSource.pitch = 1.0f; // 确保音频以正常速度播放
            musicSource.Play();   // 开始播放音乐
            Debug.Log("Music Played");

            // fog.Play();
            spawner.enabled = true;
            spawner.restart();

            hasStarted = true;    // 标记为已开始，防止重复播放
            Debug.LogWarning("GameStart");
        }
        else
        {
            Debug.Log("Game has already started.");
        }
    }

    void Update()
    {
        // 检查游戏是否已开始且音乐是否停止播放
        if (hasStarted && !musicSource.isPlaying)
        {
            // 在这里触发你想要的事件
            GameEnd();
            hasStarted = false; // 如果需要，重置游戏开始标志
        }
    }

    void GameEnd()
    {
        Debug.Log("Game Ended");
        gameasset.SetActive(false);
        menuController.ShowMenu("EndPannel");
        UIManager.Instance.UpdateFinalScore();

    }


    public void GameQuit()
    {
        Application.Quit();   // 退出游戏
    }

}
