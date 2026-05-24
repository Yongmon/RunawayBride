using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{

    [System.Serializable]
    public class DialogueData
    {
        public string ask;
        public string answer;

        public DialogueData(string ask, string answer)
        {
            this.ask = ask;
            this.answer = answer;
        }
    }
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
        switch (clue)
        {
            case "安检员":
                return "是的，看来你都已经掌握了，接下来好好干，别让组织失望。";

            case "摇滚爱好":
                return "非常喜欢，有时候我会觉得如果能早点听到这首摇滚乐专辑就好了。\r\n";

            case "别人的婚礼？":
                return "密码箱里的东西非常重要，千万别弄丢。";

            case "英国":
                return "密码箱里的东西非常重要，千万别弄丢。";

            case "水果糖女主":
                return "密码箱里的东西非常重要，千万别弄丢。";

            case "纱质布料":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "留学":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "练芭蕾的女儿":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "口味":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "新娘？":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "设计师":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "伦敦":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "学习芭蕾":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "只有一半":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "薄荷糖":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "摇滚乐迷":
                return "密码箱里的东西非常重要，千万别弄丢。";
            case "水果糖女3":
                return "密码箱里的东西非常重要，千万别弄丢。";

            default:
                return "我不明白你在说什么。";
        }
    }
}