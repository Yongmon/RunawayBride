using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Notebook : MonoBehaviour
{
    [TextArea]
    public string[] notebookDialog = new string[]
    {
        "点击你觉得有用的线索，把它们收集到笔记本上吧，聪明的侦探。……不，这并不是一句夸奖。",
        "打开笔记本就能查看已经记录下来的线索和进行推理了，我知道你非常需要这个功能。",
        "对了，选中笔记本上记录的线索就可以尝试关联它们，按下space键进行结论的推理吧，祝你今天早点下班。"
    };

    [Header("推理页面跳转事件")]
    public UnityEvent onGoToReasoningPage;

    private bool hasIntroduced;
    private const string INTRODUCED_KEY = "NotebookHasIntroduced";

    private void Awake()
    {
        hasIntroduced = PlayerPrefs.GetInt(INTRODUCED_KEY, 0) == 1;
    }

    private void OnMouseDown()
    {
        // 对话进行中不能重复点击
        if (DialogManager.IsDialogActive)   // ⬅️ 修正：用类名，不用实例
        {
            Debug.Log("对话进行中，忽略点击");
            return;
        }

        if (!hasIntroduced)
        {
            // 第一次点击：显示对话介绍
            DialogManager dialogManager = FindObjectOfType<DialogManager>();
            if (dialogManager == null)
            {
                Debug.LogError("未找到 DialogManager！");
                return;
            }
            dialogManager.StartDialog(notebookDialog);
            // 启动协程，等待对话结束后自动跳转
            StartCoroutine(WaitForDialogEnd());
        }
        else
        {
            GoToReasoningPage();
        }
    }

    private IEnumerator WaitForDialogEnd()
    {
        // 等待对话结束（静态属性变为 false）
        while (DialogManager.IsDialogActive)   // ⬅️ 修正：类名访问
        {
            yield return null;
        }

        // 标记已介绍并保存
        hasIntroduced = true;
        PlayerPrefs.SetInt(INTRODUCED_KEY, 1);
        PlayerPrefs.Save();



        // 进入推理页面
        GoToReasoningPage();
    }

    private void GoToReasoningPage()
    {
        onGoToReasoningPage?.Invoke();
    }

    [ContextMenu("Reset Introduction State")]
    private void ResetIntroduction()
    {
        PlayerPrefs.DeleteKey(INTRODUCED_KEY);
        hasIntroduced = false;
        Debug.Log("笔记本介绍状态已重置");
    }
}