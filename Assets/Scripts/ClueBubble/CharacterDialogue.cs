using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public void StartDialogue(string clueText)
    {
        Debug.Log("DialogueUI 存在：" + (DialogueUI.Instance != null));
        string response = GetResponse(clueText);

        // 显示在屏幕上
        if (DialogueUI.Instance != null)
            DialogueUI.Instance.Show(response);
        else
            Debug.LogWarning("场景中没有 DialogueUI 实例");
    }

    private string GetResponse(string clue)
    {
        // 简单匹配
        if (clue.Contains("掌握了"))
            return "是的，看来你都已经掌握了，接下来好好干，别让组织失望。";
        // 这里可以继续添加其他线索判断...
        return "我不明白你在说什么。";
    }
}