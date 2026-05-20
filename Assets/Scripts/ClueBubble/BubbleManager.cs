using TMPro;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance;

    [Header("气泡预制体")]
    public GameObject bubblePrefab;

    [Header("布局容器（自动排列）")]
    public RectTransform layoutParent;  // 替换原来的 container

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 保留，按需
    }

    /// <summary>
    /// 生成一个气泡并加入水平布局组
    /// </summary>
    public void CreateBubble(string clueContent)
    {
        if (bubblePrefab == null || layoutParent == null)
        {
            Debug.LogError("BubbleManager: 未设置 bubblePrefab 或 layoutParent");
            return;
        }

        // 实例化到布局容器下，位置由 HorizontalLayoutGroup 自动计算
        GameObject bubble = Instantiate(bubblePrefab, layoutParent);

        // 设置文字
        TMP_Text text = bubble.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = clueContent;
    }
}