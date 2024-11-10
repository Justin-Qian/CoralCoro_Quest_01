using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

[System.Serializable]
public struct AnimationFrame
{
    public Sprite sprite;
    public float duration;
    public bool overlay; // 是否叠加显示 0:否 1:是
}

[System.Serializable]
public struct TextFrame
{
    public string text;
    public float duration;
    public bool overlay; // 是否叠加显示 0:否 1:是
}

public class IntroSequence : MonoBehaviour
{
    public Image[] animatedImage; // 分配您的Image组件，用于播放帧动画
    public AnimationFrame[] animationFrames; // 帧动画序列
    public TextFrame[] textFrames; // 文本显示序列
    public TextMeshProUGUI displayText; // 用于显示文本
    // public float frameRate = 0.2f; // 每帧显示的时间
    public GameObject startButton; // 分配您的按钮
    public GameObject introPanel; // 分配您的Panel

    public MenuController menuController; // GameController组件显示
    // private bool isAnimating = true;

    public void IntroAnimationStart()
    {
        // 隐藏按钮
        startButton.SetActive(false);
        menuController.HideMenu();

        // 调整 animatedImage 中每个图片的 RectTransform 属性
        foreach (var img in animatedImage)
        {
            RectTransform rectTransform = img.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(60, 45);  // 调整为你所需要的大小
            rectTransform.anchoredPosition = Vector2.zero;  // 居中
        }

        // 显示IntroPanel并开始播放动画
        introPanel.SetActive(true);
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        int frameCount = animationFrames.Length;
        int imageLayer = 0;
        int textIndex = 0;

        for (int i = 0; i < frameCount; i++)
        {
            if (!animationFrames[i].overlay)
            {
                // 清除之前的所有图片
                foreach (var img in animatedImage)
                {
                    img.gameObject.SetActive(false);
                }
                // imageLayer = 0; //因为每个图层形状都不一样大 所以不能共用image
            }

            // 显示当前图片
            animatedImage[imageLayer].sprite = animationFrames[i].sprite;
            animatedImage[imageLayer].gameObject.SetActive(true);

            // 如果有文本需要显示
            if (textIndex < textFrames.Length)
            {
                if (!textFrames[textIndex].overlay)
                {
                    displayText.gameObject.SetActive(false);
                }

                // 动态调整文本框的大小
                RectTransform textRectTransform = displayText.GetComponent<RectTransform>();
                textRectTransform.sizeDelta = new Vector2(70f, 30f); // 设置文本框宽高，可以根据需要调整
                textRectTransform.anchoredPosition = new Vector2(0f, -10f); // 设置文本框位置，居中并下移
                displayText.fontSize = 4; // 设置字体大小，可以根据需要调整

                StartCoroutine(DisplayText(textFrames[textIndex]));
                textIndex++;
            }

            yield return new WaitForSeconds(animationFrames[i].duration);

            // 增加层级
            if (animationFrames[i].overlay)
            {
                imageLayer++;
                if (imageLayer >= animatedImage.Length)
                {
                    Debug.LogError("Not enough image layers to support the overlay settings.");
                    break;
                }
            }
        }

        // 动画播放完毕时调用ShowStartButton
        Debug.Log("Animation completed.");
        ShowStartButton();
    }

    IEnumerator DisplayText(TextFrame textFrame)
    {
        displayText.text = textFrame.text;
        displayText.gameObject.SetActive(true);
        yield return new WaitForSeconds(textFrame.duration);
        if (!textFrame.overlay)
        {
            displayText.gameObject.SetActive(false);
        }
    }

    void ShowStartButton()
    {
        // 隐藏动画面板
        for (int i = 0; i < animatedImage.Length; i++)
        {
            animatedImage[i].gameObject.SetActive(false);
        }

        // // 显示按钮
        // startButton.gameObject.SetActive(true);

        // 显示按钮
        if (startButton != null)
        {
            startButton.SetActive(true);
            Debug.Log("startButton is now active.");

            // 设置按钮在UI层级的最前面
            startButton.transform.SetAsLastSibling();
        }

    }

    public void OnStartButtonClicked()
    {
        // 隐藏IntroPanel并显示MainMenuPanel
        introPanel.SetActive(false);
        menuController.ShowMenu("MainMenuPanel");
    }
}
