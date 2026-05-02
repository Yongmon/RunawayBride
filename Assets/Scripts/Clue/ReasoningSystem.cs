using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReasoningSystem : MonoBehaviour
{
    public static ReasoningSystem Instance;

    private ClueItem selectedA;
    private ClueItem selectedB;

    [Header("连线UI")]
    public RectTransform line;
    public Canvas canvas;

    [Header("失败提示")]
    public GameObject failPopup;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // ⭐ 正在连线时，线跟随鼠标
        if (selectedA != null && selectedB == null)
        {
            UpdateLine(Input.mousePosition);
        }

        // ⭐ 按空格推理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryReason();
        }
    }

    public void SelectClue(ClueItem clue)
    {
        // 第一次点击
        if (selectedA == null)
        {
            selectedA = clue;
            clue.SetSelected(true);
            ShowLine(clue.transform.position);
        }
        // 第二次点击
        else if (selectedB == null && clue != selectedA)
        {
            selectedB = clue;
            clue.SetSelected(true);

            FixLine(clue.transform.position);
        }
    }

    // ⭐ 显示线
    void ShowLine(Vector3 start)
    {
        line.gameObject.SetActive(true);
        line.position = start;
    }

    // ⭐ 跟随鼠标
    void UpdateLine(Vector3 end)
    {
        Vector3 dir = end - line.position;
        float dist = dir.magnitude;

        line.sizeDelta = new Vector2(dist, 5f);
        line.right = dir.normalized;
    }

    // ⭐ 固定到第二个点
    void FixLine(Vector3 end)
    {
        UpdateLine(end);
    }

    // ⭐ 推理判断
    void TryReason()
    {
        if (selectedA == null || selectedB == null)
            return;

        string a = selectedA.clueID;
        string b = selectedB.clueID;

        // ⭐ 规则判断（你可以扩展）
        if (IsCorrectPair(a, b))
        {
            Debug.Log("✅ 推理成功！");

            // 👉 添加新线索
            ClueManager.Instance.AddClue("result_clue");
        }
        else
        {
            Debug.Log("❌ 推理失败");

            StartCoroutine(ShowFail());
        }

        ResetSelection();
    }

    bool IsCorrectPair(string a, string b)
    {
        return (a == "blood_clue" && b == "engrave_clue") ||
               (a == "engrave_clue" && b == "blood_clue");
    }

    System.Collections.IEnumerator ShowFail()
    {
        failPopup.SetActive(true);
        yield return new WaitForSeconds(2f);
        failPopup.SetActive(false);
    }

    void ResetSelection()
    {
        if (selectedA) selectedA.SetSelected(false);
        if (selectedB) selectedB.SetSelected(false);

        selectedA = null;
        selectedB = null;

        line.gameObject.SetActive(false);
    }
}