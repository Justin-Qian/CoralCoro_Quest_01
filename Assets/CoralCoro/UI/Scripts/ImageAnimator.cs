using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    public Image animatedImage;
    public Sprite[] sprites;  // 所有图片素材
    public float frameRate = 0.5f;  // 每张图片显示的时间（秒）
    public MenuController menuController;  // MenuController脚本的引用
    // public Button showButton;  // 按钮对象

    private int currentFrame;
    private float timer;
    private bool animationFinished = false;

    void Start()
    {
        if (sprites.Length > 0)
        {
            animatedImage.sprite = sprites[0];
        } 
        
       // 确保主菜单在动画播放期间隐藏
        if (Application.isPlaying && menuController != null)
        {
            menuController.HideMainMenu();
        }
    }

    void Update()
    {
        if (sprites.Length > 0 && !animationFinished)
        {
            timer += Time.deltaTime;
            if (timer >= frameRate)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % sprites.Length;
                animatedImage.sprite = sprites[currentFrame];

                // 检查动画是否播放完成
                if (currentFrame == sprites.Length - 1)
                {
                    animationFinished = true;
                    if (menuController != null)
                    {
                        menuController.ShowMainMenu();
                    }
                }
            }
        }
    }

}
