using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance;

    // ========== 原有部分 ==========
    [Header("线索显示")]
    public GameObject clueEntryPrefab;
    public Transform clueListParent;
    private HashSet<string> collectedClues = new HashSet<string>();

    private Dictionary<string, string> clueDisplayNames = new Dictionary<string, string>
    {
        { "IDcard", "工作牌" },
        { "clothes", "制服" }
    };

    // ========== 新增：连线推理部分 ==========
    [Header("连线设置")]
    public LineRenderer lineRenderer;        // 拖入 LineRendererObject
    public Camera uiCamera;                  // 如果是 Screen Space - Camera 模式需要；Overlay 模式可留空

    [Header("推理反馈")]
    public GameObject failTipPrefab;         // 失败提示预制体（纯文字 TMP_Text）
    public Transform failTipParent;          // 提示出现位置（可放在 Canvas 下）

    // 内部状态
    private ClueEntry selectedEntryA = null; // 第一个选中的线索
    private ClueEntry selectedEntryB = null; // 第二个选中的线索
    private bool isDrawingLine = false;

    // 推理表（暂时硬编码，后续可从 CharacterData 读取）
    private Dictionary<(string, string), string> reasoningTable = new Dictionary<(string, string), string>
    {
        { ("clothes", "IDcard"), "安检员" },
        { ("IDcard", "clothes1"), "安检员" },
    };

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 初始隐藏连线
        if (lineRenderer != null)
            lineRenderer.positionCount = 0;
    }

    // ========== 收集线索 ==========
    public void AddClue(string clueID)
    {
        Debug.Log($"[路径追踪] 1. AddClue 被调用: {clueID}");

        if (collectedClues.Contains(clueID)) return;
        collectedClues.Add(clueID);

        Debug.Log($"[路径追踪] 2. 准备跳转 CreateClueEntry");

        try
        {
            CreateClueEntry(clueID);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[崩溃捕捉] 调用 CreateClueEntry 时发生异常: {e.Message}\n{e.StackTrace}");
        }
    }

    private void CreateClueEntry(string clueID)
    {
        // 这一行必须是函数的第一行！
        Debug.Log($"[路径追踪] 3. 已经成功进入 CreateClueEntry !! 传入 ID 是: {clueID}");

        if (clueEntryPrefab == null || clueListParent == null)
        {
            Debug.LogError("❌ 实例化失败：Prefab 或 Parent 为空！请检查 Inspector 面板赋值。");
            return;
        }

        // 执行实例化
        GameObject entryGO = Instantiate(clueEntryPrefab, clueListParent);
        Debug.Log($"[路径追踪] 4. Instantiate 执行完毕，生成物体: {entryGO.name}");

        // 强制显示
        entryGO.SetActive(true);

        // 重置 UI 坐标（防止飞出屏幕）
        RectTransform rt = entryGO.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;

        ClueEntry entryScript = entryGO.GetComponent<ClueEntry>();
        if (entryScript != null)
        {
            string displayName = clueDisplayNames.ContainsKey(clueID) ? clueDisplayNames[clueID] : clueID;
            entryScript.SetUp(clueID, displayName);
        }
    }
    public bool HasClue(string clueID) => collectedClues.Contains(clueID);

    // ========== 新增：点击线索处理 ==========
    public void OnClueEntryClicked(ClueEntry entry)
    {
        Debug.Log("Manager收到点击：" + entry.clueID);
        if (selectedEntryA == null)
        {
            // 第一次选中
            selectedEntryA = entry;
            Highlight(selectedEntryA, true);
            Debug.Log("✔ 选中第一个");
            StartDrawingLine(entry);
        }
        else if (selectedEntryA == entry)
        {
            // 再次点击同一个：取消选中
            ClearSelection();
        }
        else if (selectedEntryB == null)
        {
            // 选中第二个
            selectedEntryB = entry;
            Highlight(selectedEntryB, true);
            Debug.Log("✔ 选中第二个");
            CompleteLine(entry);   // 连线固定到第二个
        }
    }

    // 高亮/取消高亮
    private void Highlight(ClueEntry entry, bool on)
    {
        if (entry.label != null)
            entry.label.color = on ? Color.yellow : Color.white; // 简单颜色切换
    }

    // 开始画线（跟随鼠标）
    private void StartDrawingLine(ClueEntry startEntry)
    {
        if (lineRenderer == null) return;

        isDrawingLine = true;
        lineRenderer.positionCount = 2;

        Vector3 startScreen = RectTransformUtility.WorldToScreenPoint(
            uiCamera, startEntry.transform.position);

        Vector3 startWorld = ScreenToWorld(startScreen);

        lineRenderer.SetPosition(0, startWorld);
        lineRenderer.SetPosition(1, startWorld);
    }

    // 完成连线（固定到第二个线索）
    private void CompleteLine(ClueEntry endEntry)
    {
        isDrawingLine = false;

        Vector3 endScreen = RectTransformUtility.WorldToScreenPoint(
            uiCamera, endEntry.transform.position);

        Vector3 endWorld = ScreenToWorld(endScreen);

        lineRenderer.SetPosition(1, endWorld);
    }

    // 每帧更新线条终点（跟随鼠标）
    private void Update()
    {
        if (isDrawingLine && selectedEntryA != null && lineRenderer != null)
        {
            UpdateLineEndPoint();
        }

        // 空格推理
        if (Input.GetKeyDown(KeyCode.Space) && selectedEntryA != null && selectedEntryB != null)
        {
            TryReasoning();
        }
    }

    private void UpdateLineEndPoint()
    {
        Vector3 worldPos = ScreenToWorld(Input.mousePosition);
        lineRenderer.SetPosition(1, worldPos);
    }

    // 清除选中和连线
    public void ClearSelection()
    {
        if (selectedEntryA != null) Highlight(selectedEntryA, false);
        if (selectedEntryB != null) Highlight(selectedEntryB, false);
        selectedEntryA = null;
        selectedEntryB = null;
        isDrawingLine = false;
        if (lineRenderer != null) lineRenderer.positionCount = 0;
    }

    // ========== 推理逻辑 ==========
    private void TryReasoning()
    {
        string idA = selectedEntryA.clueID;
        string idB = selectedEntryB.clueID;

        // ----- 新增：按字母序排序，忽略用户点击顺序 -----
        string first = string.Compare(idA, idB) < 0 ? idA : idB;
        string second = string.Compare(idA, idB) < 0 ? idB : idA;

        if (reasoningTable.TryGetValue((first, second), out string conclusionID))
        {
            Debug.Log($"推理成功，得到结论：{conclusionID}");
            AddConclusionClue(conclusionID);
            ClearSelection();
        }
        else
        {
            Debug.Log("推理失败");
            StartCoroutine(ShowFailTip());
            ClearSelection();
        }
    }

    // 添加结论线索（特殊显示）
    private void AddConclusionClue(string clueID)
    {
        if (collectedClues.Contains(clueID)) return;
        collectedClues.Add(clueID);

        // 生成条目，但用金色字体
        if (clueEntryPrefab != null && clueListParent != null)
        {
            string displayName = clueDisplayNames.ContainsKey(clueID) ? clueDisplayNames[clueID] : clueID;
            GameObject entryGO = Instantiate(clueEntryPrefab, clueListParent);
            TMP_Text tmpText = entryGO.GetComponentInChildren<TMP_Text>();
            if (tmpText == null) tmpText = entryGO.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = displayName;
                tmpText.color = new Color(1f, 0.84f, 0f); // 金色
            }
            ClueEntry clueEntry = entryGO.GetComponent<ClueEntry>();
            if (clueEntry != null)
            {
                clueEntry.SetUp(clueID, displayName);
                clueEntry.label.color = new Color(1f, 0.84f, 0f);
            }
        }
    }
    // 【新增】屏幕坐标 → 世界坐标
    Vector3 ScreenToWorld(Vector3 screenPos)
    {
        screenPos.z = 50f; // ⭐ 很关键！控制深度（看不到线就调大）

        if (uiCamera != null)
            return uiCamera.ScreenToWorldPoint(screenPos);
        else
            return Camera.main.ScreenToWorldPoint(screenPos);
    }
    // 失败提示框
    private IEnumerator ShowFailTip()
    {
        if (failTipPrefab == null || failTipParent == null) yield break;

        GameObject tip = Instantiate(failTipPrefab, failTipParent);

        TMP_Text text = tip.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = "好像没什么关系。";

        tip.transform.SetAsLastSibling();

        yield return new WaitForSeconds(2f);

        Destroy(tip);
    }
}

