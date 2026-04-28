using UnityEngine;
using UnityEngine.UI;

public class ToggleMusicController : MonoBehaviour
{
    public AudioSource bgmSource;
    private Toggle musicToggle;

    void Start()
    {
        musicToggle = GetComponent<Toggle>();

        // 初始化：默认开启音乐，Toggle状态与之同步
        musicToggle.isOn = true;
        bgmSource.Play();
    }

    // 这个方法将绑定到Toggle的OnValueChanged事件上
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            // 如果音乐不在播放，则开始播放
            if (!bgmSource.isPlaying) bgmSource.Play();
        }
        else
        {
            // 暂停音乐播放
            if (bgmSource.isPlaying) bgmSource.Pause();
        }
    }
}