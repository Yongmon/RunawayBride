using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("音频源组件")]
    public AudioSource bgmSource;      // 播放背景音乐（建议设置 Loop = true）
    public AudioSource sfxSource;      // 播放音效（建议 Loop = false）
    public AudioSource ambienceSource; // 播放环境音（建议 Loop = true）

    [Header("混音器分组（用于独立音量控制）")]
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup ambienceMixer;

    // 音效字典：通过文件名（不含扩展名）快速获取 AudioClip
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 将 AudioSource 绑定到对应的 MixerGroup
        BindMixerGroups();

        // 加载 Resources 目录下的所有音效
        LoadAllSFX();
    }

    /// <summary>
    /// 将各个音频源绑定到对应的混音器分组
    /// </summary>
    private void BindMixerGroups()
    {
        if (bgmSource != null && bgmMixer != null)
            bgmSource.outputAudioMixerGroup = bgmMixer;

        if (sfxSource != null && sfxMixer != null)
            sfxSource.outputAudioMixerGroup = sfxMixer;

        if (ambienceSource != null && ambienceMixer != null)
            ambienceSource.outputAudioMixerGroup = ambienceMixer;
    }

    /// <summary>
    /// 从 Resources/Audio/SFX 文件夹加载所有音效，并存入字典
    /// </summary>
    private void LoadAllSFX()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (var clip in clips)
        {
            if (!sfxDictionary.ContainsKey(clip.name))
            {
                sfxDictionary.Add(clip.name, clip);
            }
            else
            {
                Debug.LogWarning($"音效名称重复：{clip.name}，已跳过");
            }
        }
        Debug.Log($"AudioManager：已加载 {sfxDictionary.Count} 个音效文件");
    }

    // ==================== 背景音乐（BGM）控制 ====================

    /// <summary>
    /// 播放指定 ID 的背景音乐（需将文件放在 Resources/Audio/BGM/ 下）
    /// </summary>
    public void PlayBGM(string bgmId)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/BGM/{bgmId}");
        if (clip == null)
        {
            Debug.LogWarning($"BGM 文件不存在：Audio/BGM/{bgmId}");
            return;
        }

        if (bgmSource == null)
        {
            Debug.LogError("bgmSource 未赋值！");
            return;
        }

        // 如果正在播放同一首音乐，则不重复播放
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        // --- 修复点：强制开启循环 ---
        bgmSource.loop = true;
        bgmSource.Play();
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Pause();
    }

    /// <summary>
    /// 恢复背景音乐
    /// </summary>
    public void ResumeBGM()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
            bgmSource.UnPause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    // ==================== 音效（SFX）控制 ====================

    /// <summary>
    /// 播放指定 ID 的音效（ID 即文件名，不含扩展名）
    /// </summary>
    public void PlaySFX(string sfxId)
    {
        if (sfxSource == null)
        {
            Debug.LogError("sfxSource 未赋值！");
            return;
        }

        if (sfxDictionary.TryGetValue(sfxId, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"音效不存在：{sfxId}");
        }
    }
    // 播放非重叠音效（适用于打字这类高频音效）
    public void PlayTypingSound(string sfxId)
    {
        if (sfxSource == null) return;
        if (sfxDictionary.TryGetValue(sfxId, out AudioClip clip))
        {
            sfxSource.Stop(); // 先停止当前音效
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 随机播放一组编号音效（例如文件名 Select01, Select02 ... Select06）
    /// </summary>
    /// <param name="baseId">基础名称，如 "Select"</param>
    /// <param name="startIndex">起始编号（包含）</param>
    /// <param name="endIndex">结束编号（包含）</param>
    public void PlayRandomSFX(string baseId, int startIndex, int endIndex)
    {
        int randomIndex = Random.Range(startIndex, endIndex + 1);
        string sfxId = $"{baseId}{randomIndex:D2}"; // 格式化为两位数字，如 Select01
        PlaySFX(sfxId);
    }

    // ==================== 环境音（Ambience）控制 ====================

    /// <summary>
    /// 播放指定 ID 的环境音（需放在 Resources/Audio/Ambience/ 下）
    /// </summary>
    public void PlayAmbience(string ambienceId)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Ambience/{ambienceId}");
        if (clip == null)
        {
            Debug.LogWarning($"环境音文件不存在：Audio/Ambience/{ambienceId}");
            return;
        }

        if (ambienceSource == null)
        {
            Debug.LogError("ambienceSource 未赋值！");
            return;
        }

        ambienceSource.clip = clip;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        if (ambienceSource != null)
            ambienceSource.Stop();
    }

    // ==================== 全局音量控制（可选） ====================
    // 可以通过 Mixer 的 SetFloat 方法调整音量，通常配合 UI 滑动条使用。
    // 示例：
    // public void SetBGMVolume(float volume) { bgmMixer.audioMixer.SetFloat("BGMVolume", volume); }




    // ==================== 音效（SFX）控制增强 ====================

    /// <summary>
    /// 循环播放特定音效（适用于长段打字音）
    /// </summary>
    public void PlayLoopingSFX(string sfxId)
    {
        if (sfxSource == null) return;

        if (sfxDictionary.TryGetValue(sfxId, out AudioClip clip))
        {
            // 如果已经在播放该音效，就不重复播放
            if (sfxSource.clip == clip && sfxSource.isPlaying) return;

            sfxSource.clip = clip;
            sfxSource.loop = true; // 开启循环，防止 6 秒后音效突然断掉
            sfxSource.Play();
        }
    }

    /// <summary>
    /// 强制停止当前音效源的播放
    /// </summary>
    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
            sfxSource.loop = false; // 记得重置循环状态
        }
    }





}