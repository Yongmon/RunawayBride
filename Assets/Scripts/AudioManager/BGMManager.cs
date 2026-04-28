using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [Header("背景音乐设置")]
    public AudioClip menuBGM; // 在这里拖拽你的背景音乐文件
    private AudioSource audioSource;

    void Start()
    {
        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // 设置音频源参数
        audioSource.clip = menuBGM;   // 设置要播放的音频
        audioSource.loop = true;      // 开启循环
        audioSource.playOnAwake = true; // 场景启动时自动播放
        audioSource.Play();           // 开始播放
    }
}