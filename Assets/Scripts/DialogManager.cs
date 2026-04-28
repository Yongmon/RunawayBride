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

        int totalChars = fullText.Length;
        for (int i = 0; i < totalChars; i++)
        {
            if (skipRequested)
            {
                // 跳过：直接显示完整文本，并停止音效（如需）
                dialogText.text = fullText;
                // 可选：停止打字音效循环（如果有）
                break;
            }

            dialogText.text += fullText[i];

            // 播放打字音效（每输入一个字符）
            if (playTypingSound && !string.IsNullOrEmpty(typingSoundName))
            {
                AudioManager.Instance?.PlaySFX(typingSoundName);
            }

            // 遇到标点符号适当延长停顿（提升阅读感）
            float delay = typingSpeed;
            if (fullText[i] == '.' || fullText[i] == '!' || fullText[i] == '?' ||
                fullText[i] == '。' || fullText[i] == '！' || fullText[i] == '？')
            {
                delay = typingSpeed * 2;
            }
            yield return new WaitForSeconds(delay);
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
        isTyping = false;
        dialogPanel.SetActive(false);
        IsDialogActive = false;
        onDialogEnd.Invoke();
    }
}