using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInfo :
    MonoBehaviour,
    IPointerClickHandler
{
    public static ItemInfo CurrentSelected;

    [Header("物品名称")]
    public string itemName = "物品名称";

    [TextArea]
    [Header("物品描述")]
    public string itemDescription = "这是物品描述";

    [Header("标题颜色")]
    public string nameColor = "#1E3A8A";

    [Header("X光图片")]
    public Sprite xraySprite;

    [Header("实物图片")]
    public Sprite realSprite;

    private Image img;

    private void Awake()
    {
        img = GetComponentInChildren<Image>();

        Debug.Log("当前物体：" + gameObject.name);

        if (img == null)
        {
            Debug.LogError("没有找到Image组件！");
        }
        else
        {
            Debug.Log("找到Image：" + img.gameObject.name);
        }
    }

    // =====================================
    // UI点击
    // =====================================

    public void OnPointerClick(PointerEventData eventData)
    {
        // 再点一次 -> 关闭
        if (CurrentSelected == this)
        {
            ShowXray();

            CurrentSelected = null;

            ItemUIManager.Instance.Hide();

            return;
        }

        // 取消旧选中
        if (CurrentSelected != null)
        {
            CurrentSelected.ShowXray();
        }

        CurrentSelected = this;

        ShowReal();

        string message =
            $"<color={nameColor}>"
            + $"<b>{itemName}</b>"
            + $"</color>\n"
            + itemDescription;

        ItemUIManager.Instance.ShowItem(message);
    }

    // =====================================
    // 显示X光
    // =====================================

    public void ShowXray()
    {
        if (img != null && xraySprite != null)
        {
            img.sprite = xraySprite;
        }
    }

    // =====================================
    // 显示实物
    // =====================================

    public void ShowReal()
    {
        if (img != null && realSprite != null)
        {
            img.sprite = realSprite;
        }
    }

    // =====================================
    // 初始化物品
    // =====================================

    public void SetItem(ItemData data)
    {
        itemName = data.itemName;

        itemDescription = data.itemDescription;

        xraySprite = data.xraySprite;

        realSprite = data.realSprite;

        nameColor = data.nameColor;

        if (img != null)
        {
            img.sprite = xraySprite;
        }
    }
}