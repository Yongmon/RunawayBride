using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [TextArea] public string[] defaultDialogLines;   // 场景开始时自动播放的对话（可选）
    private string[] currentDialogLines;
    private int currentLine = 0;

    public Text dialogText;
    public GameObject dialogPanel;
    public UnityEvent onDialogEnd;

    public static bool IsDialogActive { get; private set; } = false;

    [Header("打字机效果设置")]
    public float typingSpeed = 0.05f;          // 每个字符的显示间隔（秒）
    public bool playTypingSound = true;        // 是否播放打字音效
    public string typingSoundName = "typing";  // 音效名称（需与 AudioManager 中注册的名称一致）

    private Coroutine typingCoroutine;
    private bool isTyping = false;              // 是否正在逐字显示
    private bool skipRequested = false;         // 是否请求跳过当前段落

    void Start()
    {
        if (defaultDialogLines != null && defaultDialogLines.Length > 0)
        {
            StartDialog(defaultDialogLines);
        }
        else
        {
            dialogPanel.SetActive(false);
            IsDialogActive = false;
        }
    }

    void Update()
    {
        if (IsDialogActive && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // 正在逐字显示 -> 跳过当前段落，直接显示完整文本
                SkipTyping();
            }
            else
            {
                // 当前段落已完整显示 -> 进入下一段对话
                NextLine();
            }
        }
    }

    // 公开方法：开始新对话
    public void StartDialog(string[] lines)
    {
        Debug.Log("StartDialog 被调用");
        if (IsDialogActive) { Debug.Log("已有对话，忽略"); return; }
        if (lines == null || lines.Length == 0) { Debug.LogWarning("对话数组为空"); return; }

        if (dialogPanel == null) { Debug.LogError("dialogPanel 为空！"); return; }
        if (dialogText == null) { Debug.LogError("dialogText 为空！"); return; }

        currentDialogLines = lines;
        currentLine = 0;
        dialogPanel.SetActive(true);
        IsDialogActive = true;

        // 开始逐字显示第一段
        ShowLineWithTyping(currentDialogLines[currentLine]);
    }

    // 逐字显示一段文本
    private void ShowLineWithTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line));
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        skipRequested = false;
        dialogText.text = "";

        // 【新增】打字开始：开启长段循环音效
        if (playTypingSound && !string.IsNullOrEmpty(typingSoundName))
        {
            AudioManager.Instance?.PlayLoopingSFX(typingSoundName);
        }

        int totalChars = fullText.Length;
        for (int i = 0; i < totalChars; i++)
        {
            if (skipRequested)
            {
                dialogText.text = fullText;
                break;
            }

            dialogText.text += fullText[i];

            float delay = typingSpeed;
            if (fullText[i] == '.' || fullText[i] == '!' || fullText[i] == '?' ||
                fullText[i] == '。' || fullText[i] == '！' || fullText[i] == '？')
            {
                // 遇到标点符号时，如果你想让声音也停顿，可以临时暂停声音
                // 但对于长音效，不停顿通常听起来更自然。如果需要停顿，可以在这里 Stop 再在 delay 后 Play
                delay = typingSpeed * 2;
            }
            yield return new WaitForSeconds(delay);
        }

        // 【新增】打字结束或被跳过：停止音效
        if (playTypingSound)
        {
            AudioManager.Instance?.StopSFX();
        }

        isTyping = false;
        typingCoroutine = null;
    }

    // 跳过当前段落的逐字显示
    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            skipRequested = true;
            // 可选：立即停止打字音效（如果音效是持续循环的，需要额外处理）
        }
    }

    // 显示下一段对话
    private void NextLine()
    {
        if (isTyping) return; // 安全起见，不应在逐字显示中调用

        currentLine++;
        if (currentLine < currentDialogLines.Length)
        {
            // 显示下一段，同样使用逐字显示
            ShowLineWithTyping(currentDialogLines[currentLine]);
        }
        else
        {
            EndDialog();
        }
    }

    // 结束对话（可被外部调用）
    public void EndDialog()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // 【保险】结束对话时务必关闭声音
        if (playTypingSound)
        {
            AudioManager.Instance?.StopSFX();
        }
        isTyping = false;
        dialogPanel.SetActive(false);
        IsDialogActive = false;
        onDialogEnd.Invoke();
    }
}