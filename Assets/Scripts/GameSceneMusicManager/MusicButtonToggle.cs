using UnityEngine;
using UnityEngine.UI;

public class MusicButtonToggle : MonoBehaviour
{
    [Header("图片设置")]
    public Sprite musicOnSprite;   // 音乐开启时显示的图片
    public Sprite musicOffSprite;  // 音乐关闭时显示的图片

    [Header("音频设置")]
    public AudioSource bgmSource;  // 拖入你的 BGM_Player 对象

    private Image buttonImage;     // 按钮自身的 Image 组件
    private bool isMusicOn = true; // 当前状态，默认开启

    void Start()
    {
        // 获取按钮上的 Image 组件，用于更换图片
        buttonImage = GetComponent<Image>();

        // 确保一开始显示正确的图片，并播放音乐
        UpdateButtonAndMusic();
    }

    // 这个方法将绑定到按钮的 OnClick 事件上
    public void OnButtonClick()
    {
        // 翻转开关状态
        isMusicOn = !isMusicOn;
        UpdateButtonAndMusic();
    }

    private void UpdateButtonAndMusic()
    {
        // 1. 根据状态更换按钮图片
        if (isMusicOn)
        {
            buttonImage.sprite = musicOnSprite;
        }
        else
        {
            buttonImage.sprite = musicOffSprite;
        }

        // 2. 根据状态控制背景音乐播放/暂停
        if (bgmSource != null)
        {
            if (isMusicOn)
            {
                // 如果音乐没有在播放，就让它播放
                if (!bgmSource.isPlaying) bgmSource.Play();
            }
            else
            {
                // 暂停音乐
                if (bgmSource.isPlaying) bgmSource.Pause();
            }
        }
        else
        {
            Debug.LogWarning("BGM Source 没有赋值！");
        }
    }
}