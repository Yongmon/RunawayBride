//using UnityEngine;
//using TMPro;

//public class ItemUIManager : MonoBehaviour
//{
//    public static ItemUIManager Instance;

//    [Header("UI")]
//    public GameObject itemInfoPanel;
//    public TextMeshProUGUI infoText;   // 保持这个类型即可
//    private SelectableClue currentSelected;

//    void Awake()
//    {
//        Instance = this;
//        if (itemInfoPanel != null)
//            itemInfoPanel.SetActive(false);
//    }

//    public void ShowItem(SelectableClue item, string message)
//    {
//        if (currentSelected == item)
//        {
//            item.SetSelected(false);
//            Hide();
//            return;
//        }

//        currentSelected?.SetSelected(false);
//        currentSelected = item;
//        currentSelected.SetSelected(true);

//        itemInfoPanel.SetActive(true);
//        infoText.text = message;
//    }

//    public void Hide()
//    {
//        currentSelected?.SetSelected(false);
//        itemInfoPanel.SetActive(false);
//        currentSelected = null;
//    }
//“物品信息框管理器”代码
//}

using UnityEngine;
using TMPro;

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager Instance;

    [Header("UI")]
    public GameObject itemInfoPanel;

    [Header("描述文字")]
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        Instance = this;

        // 开始时隐藏
        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
    }

    // =====================================
    // 显示物品描述
    // =====================================

    public void ShowItem(string message)
    {
        itemInfoPanel.SetActive(true);

        infoText.text = message;
    }

    // =====================================
    // 隐藏描述框
    // =====================================

    public void Hide()
    {
        itemInfoPanel.SetActive(false);
    }
}