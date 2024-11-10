using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject Menu; // Menu组件显示
    public GameObject welcomePanel;
    public GameObject mainMenuPanel;
    public GameObject levelPanel;
    public GameObject soundPanel;
    public GameObject profilePanel;
    public GameObject recordingPanel;
    public GameObject preparationPanel;
    public GameObject endPanel;
    public GameObject ScorePanel;


    void Start()
    {
        ShowMainMenuPanel();
    }

    /// <summary>
    /// 显示菜单父物件，并显示指定的子面板
    /// </summary>
    /// <param name="pannelName"></param>
    public void ShowMenu(string pannelName)
    {
        Menu.SetActive(true);
        switch (pannelName){
            case "MainMenuPanel":
                ShowMainMenuPanel();
                break;
            case "EndPannel":
                ShowEndPanel();
                break;
            default:
                Debug.LogError("Invalid pannel name.");
                break;
        }
    }

    /// <summary>
    /// 隐藏菜单父物件，并隐藏所有子面板
    /// </summary>
    public void HideMenu()
    {
        HideAllPanels();
        Menu.SetActive(false);
    }

    // 以下是控制面板的函数
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
    }

    public void ShowLevelPanel()
    {
        HideAllPanels();
        levelPanel.SetActive(true);
    }

    public void ShowSoundPanel()
    {
        HideAllPanels();
        soundPanel.SetActive(true);
    }

    public void ShowRecordingPanel()
    {
        HideAllPanels();
        recordingPanel.SetActive(true);
    }

    public void ShowProfilePanel()
    {
        HideAllPanels();
        profilePanel.SetActive(true);
    }

    public void ShowMainMenuPanel()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }

    public void ShowWelcomePanel()
    {
        HideAllPanels();
        welcomePanel.SetActive(true);
    }

    public void ShowPreparationPanel()
    {
        HideAllPanels();
        preparationPanel.SetActive(true);
    }

    public void ShowEndPanel()
    {
        HideAllPanels();
        endPanel.SetActive(true);
    }


    public void HideAllPanels()
    {
        welcomePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(false);
        soundPanel.SetActive(false);
        profilePanel.SetActive(false);
        recordingPanel.SetActive(false);
        preparationPanel.SetActive(false);
        endPanel.SetActive(false);
        soundPanel.SetActive(false);
    }

    public void ShowScorePanel()
    {
        if (ScorePanel != null)
        {
            ScorePanel.SetActive(true);
        }
    }
}
