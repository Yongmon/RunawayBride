using UnityEngine;
using System.Collections.Generic;
using TMPro;  // 因为要用 TextMeshProUGUI，别忘了引用

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance;

    // ========== UI 部分 ==========
    [Header("线索显示 UI")]
    public GameObject clueEntryPrefab;   // 纯文字预制体（就是 TMP_Text 自身）
    public Transform clueListParent;     // ScrollView 的 Content

    // ========== 线索数据 ==========
    private HashSet<string> collectedClues = new HashSet<string>();

    // 线索ID → 显示名称 映射（先写死，后面可改成从人物数据里读）
    private Dictionary<string, string> clueDisplayNames = new Dictionary<string, string>
    {
        { "IDcard", "工作牌" },
        { "clothes", "制服" }
	// 按你的需要继续添加
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 添加线索（由 TextLinkClicker 调用）
    /// </summary>
    public void AddClue(string clueID)
    {
        if (collectedClues.Contains(clueID))
        {
            Debug.Log($"⚠️ 已经收集过：{clueID}");
            return;
        }

        collectedClues.Add(clueID);
        Debug.Log($"✅ 收集线索：{clueID}");

        // 在笔记本页面里创建一条纯文字线索条目
        CreateClueEntry(clueID);
    }

    /// <summary>
    /// 在 UI 中生成一条纯文字线索
    /// </summary>
    private void CreateClueEntry(string clueID)
    {
        if (clueEntryPrefab == null || clueListParent == null)
        {
            Debug.LogWarning("ClueManager: clueEntryPrefab 或 clueListParent 未赋值");
            return;
        }

        // 获取显示名称
        string displayName = clueID;
        if (clueDisplayNames.ContainsKey(clueID))
            displayName = clueDisplayNames[clueID];

        // 实例化预制体（预制体本身就是一个 TMP_Text）
        GameObject entryGO = Instantiate(clueEntryPrefab, clueListParent);
        TMP_Text tmpText = entryGO.GetComponent<TMP_Text>();
        if (tmpText == null)
            tmpText = entryGO.GetComponentInChildren<TMP_Text>();

        if (tmpText != null)
        {
            tmpText.text = displayName;
        }
        else
        {
            Debug.LogError("线索预制体没有找到 TMP_Text 组件！");
        }
    }

    // ========== 下面这些你原来就有，保持不变 ==========
    public bool HasClue(string clueID)
    {
        return collectedClues.Contains(clueID);
    }

    public List<string> GetAllClues()
    {
        return new List<string>(collectedClues);
    }

    public void ClearClues()
    {
        collectedClues.Clear();
        // 清空 UI 也需要处理（可以再加逻辑）
        foreach (Transform child in clueListParent)
        {
            Destroy(child.gameObject);
        }
    }
}