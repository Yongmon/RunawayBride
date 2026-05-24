using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance;

    // =====================================
    // 文件夹UI
    // =====================================

    [Header("线索预制体")]
    public GameObject clueEntryPrefab;

    [Header("线索父物体")]
    public Transform clueListParent;

    // =====================================
    // 推理失败提示
    // =====================================

    [Header("推理失败提示")]
    public GameObject failTipPrefab;

    public Transform failTipParent;

    // =====================================
    // 当前选中的两个线索
    // =====================================

    private ClueEntry selectedEntryA;
    private ClueEntry selectedEntryB;

    // =====================================
    // 初始化
    // =====================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // =====================================
    // 收集线索
    // =====================================

    public void AddClue(string clueID)
    {
        CharacterData current =
            GameManager.Instance.currentCharacter;

        // 已收集
        if (current.collectedClueIDs.Contains(clueID))
            return;

        // 加入当前人物
        current.collectedClueIDs.Add(clueID);

        // 生成UI
        CreateClueEntry(clueID);

        Debug.Log("收集线索：" + clueID);
    }

    // =====================================
    // 创建文件夹线索
    // =====================================

    private void CreateClueEntry(string clueID)
    {
        if (clueEntryPrefab == null
            || clueListParent == null)
        {
            Debug.LogError("线索Prefab或Parent为空");
            return;
        }

        GameObject obj =
            Instantiate(
                clueEntryPrefab,
                clueListParent
            );

        ClueEntry entry =
            obj.GetComponent<ClueEntry>();

        if (entry != null)
        {
            entry.SetUp(
                clueID,
                GetDisplayName(clueID)
            );
        }
    }

    // =====================================
    // 获取显示名字
    // =====================================

    private string GetDisplayName(string id)
    {
        CharacterData current =
            GameManager.Instance.currentCharacter;

        // 普通线索
        foreach (ClueData clue in current.clues)
        {
            if (clue.clueId == id)
            {
                return clue.clueName;
            }
        }

        // Bubble
        foreach (BubbleData bubble in current.bubbles)
        {
            if (bubble.bubbleId == id)
            {
                return bubble.bubbleName;
            }
        }

        return id;
    }

    // =====================================
    // 是否拥有线索
    // =====================================

    public bool HasClue(string clueID)
    {
        CharacterData current =
            GameManager.Instance.currentCharacter;

        return current.collectedClueIDs.Contains(clueID);
    }

    // =====================================
    // 点击线索
    // =====================================

    public void OnClueEntryClicked(
        ClueEntry entry
    )
    {
        // 第一个
        if (selectedEntryA == null)
        {
            selectedEntryA = entry;

            Highlight(
                selectedEntryA,
                true
            );

            return;
        }

        // 点同一个取消
        if (selectedEntryA == entry)
        {
            ClearSelection();
            return;
        }

        // 第二个
        if (selectedEntryB == null)
        {
            selectedEntryB = entry;

            Highlight(
                selectedEntryB,
                true
            );
        }
    }

    // =====================================
    // 高亮
    // =====================================

    private void Highlight(
        ClueEntry entry,
        bool on
    )
    {
        if (entry.label != null)
        {
            entry.label.color =
                on
                ? Color.yellow
                : Color.white;
        }
    }

    // =====================================
    // Update
    // =====================================

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedEntryA != null
                && selectedEntryB != null)
            {
                TryReasoning();
            }
        }
    }

    // =====================================
    // 推理逻辑
    // =====================================

    private void TryReasoning()
    {
        string idA = selectedEntryA.clueID;
        string idB = selectedEntryB.clueID;

        CharacterData current =
            GameManager.Instance.currentCharacter;

        foreach (BubbleData bubble in current.bubbles)
        {
            // 已解锁
            if (bubble.unlocked)
                continue;

            // 必须两个条件
            if (bubble.requiredClues.Count != 2)
                continue;

            string needA =
                bubble.requiredClues[0];

            string needB =
                bubble.requiredClues[1];

            bool success =
                (idA == needA && idB == needB)
                ||
                (idA == needB && idB == needA);

            if (success)
            {
                Debug.Log(
                    "推理成功："
                    + bubble.bubbleName
                );

                bubble.unlocked = true;

                AddConclusionBubble(
                    bubble
                );

                ClearSelection();

                return;
            }
        }

        // 失败
        Debug.Log("推理失败");

        StartCoroutine(
            ShowFailTip()
        );

        ClearSelection();
    }

    // =====================================
    // 添加推理结果
    // =====================================

    private void AddConclusionBubble(
        BubbleData bubble
    )
    {
        CharacterData current =
            GameManager.Instance.currentCharacter;

        // 已拥有
        if (current.collectedClueIDs.Contains(
            bubble.bubbleId))
        {
            return;
        }

        // 加入线索
        current.collectedClueIDs.Add(
            bubble.bubbleId
        );

        // 文件夹生成文字
        CreateClueEntry(
            bubble.bubbleId
        );

        // 主界面生成Bubble
        if (BubbleManager.Instance != null)
        {
            BubbleManager.Instance.CreateBubble(
                bubble.bubbleName
            );
        }
    }

    // =====================================
    // 清除选中
    // =====================================

    public void ClearSelection()
    {
        if (selectedEntryA != null)
        {
            Highlight(
                selectedEntryA,
                false
            );
        }

        if (selectedEntryB != null)
        {
            Highlight(
                selectedEntryB,
                false
            );
        }

        selectedEntryA = null;
        selectedEntryB = null;
    }

    // =====================================
    // 推理失败提示
    // =====================================

    private IEnumerator ShowFailTip()
    {
        if (failTipPrefab == null
            || failTipParent == null)
        {
            yield break;
        }

        GameObject tip =
            Instantiate(
                failTipPrefab,
                failTipParent
            );

        TMP_Text text =
            tip.GetComponentInChildren<TMP_Text>();

        if (text != null)
        {
            text.text = "好像没什么关系。";
        }

        yield return new WaitForSeconds(2f);

        Destroy(tip);
    }

}