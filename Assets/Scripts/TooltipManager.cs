using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    public GameObject tooltipPanel;   // 信息面板（固定位置）
    public Text tooltipText;          // 显示信息的文本组件

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 初始时隐藏面板
        tooltipPanel.SetActive(false);
    }

    // 显示信息，固定位置（无需鼠标坐标）
    public void ShowTooltip(string message)
    {
        tooltipText.text = message;
        tooltipPanel.SetActive(true);
    }

    // 隐藏信息
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}