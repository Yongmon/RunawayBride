using UnityEngine;

public class UIAudioPlayer : MonoBehaviour
{
    [Header("UI音效")]
    public AudioClip clickSound; // 在此处拖入你的点击音效

    private AudioSource audioSource;

    void Awake()
    {
        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 配置 AudioSource
        audioSource.playOnAwake = false; // 禁止启动时播放
        audioSource.loop = false;        // 禁止循环播放
    }

    // 这个方法供UI按钮调用
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound,1.5f); // 使用 PlayOneShot，避免重叠播放[reference:2]   1.5倍音量
        else
            Debug.LogWarning("UIAudioPlayer: 点击音效或AudioSource未设置！");
    }
    
}