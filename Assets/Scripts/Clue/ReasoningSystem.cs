using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReasoningSystem : MonoBehaviour
{
    public static ReasoningSystem Instance;

    private ClueItem selectedA;
    private ClueItem selectedB;


    //新连线系统
    [Header("连线UI")]
    // 临时线
    public RectTransform tempLine;

    // 永久线预制体
    public GameObject linePrefab;

    // 永久线父物体
    public Transform lineLayer;
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
        tempLine.gameObject.SetActive(true);
        tempLine.position = start;
    }

    // ⭐ 跟随鼠标
    void UpdateLine(Vector3 end)
    {
        Vector3 dir = end - tempLine.position;
        float dist = dir.magnitude;

        tempLine.sizeDelta = new Vector2(dist, 5f);
        tempLine.right = dir.normalized;
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

            CreatePersistentLine(selectedA, selectedB);

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


    //生成永久线
    void CreatePersistentLine(ClueItem a, ClueItem b)
    {
        GameObject obj = Instantiate(linePrefab, lineLayer);

        RectTransform rt = obj.GetComponent<RectTransform>();

        Vector3 start = a.transform.position;
        Vector3 end = b.transform.position;

        rt.position = start;

        Vector3 dir = end - start;
        float dist = dir.magnitude;

        rt.sizeDelta = new Vector2(dist, 5f);
        rt.right = dir.normalized;
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

        tempLine.gameObject.SetActive(false);
    }
}