using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfo : MonoBehaviour
{
    // =========================================
    // 当前唯一选中的物品（核心）
    // =========================================

    public static ItemInfo CurrentSelected;

    // =========================================
    // 物品信息
    // =========================================

    [Header("物品名称")]
    public string itemName = "物品名称";

    [TextArea]
    [Header("物品描述")]
    public string itemDescription = "这是物品的描述。";

    [Header("标题颜色")]
    public string nameColor = "#1E3A8A";

    // =========================================
    // 图片
    // =========================================

    [Header("X光图片")]
    public Sprite xraySprite;

    [Header("实物图片")]
    public Sprite realSprite;

    // =========================================
    // 组件
    // =========================================

    private SpriteRenderer sr;

    // 当前是否显示实物
    private bool showingReal = false;

    // =========================================
    // 初始化
    // =========================================

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // 默认显示X光
        ShowXray();
    }

    // =========================================
    // 鼠标点击
    // =========================================

    void OnMouseDown()
    {
        // 点到UI时不处理
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // =====================================
        // 如果已经是当前物品
        // 再点一次 -> 切回X光
        // =====================================

        if (CurrentSelected == this)
        {
            ShowXray();

            CurrentSelected = null;

            return;
        }

        // =====================================
        // 如果之前有别的物品
        // 让它恢复X光
        // =====================================

        if (CurrentSelected != null)
        {
            CurrentSelected.ShowXray();
        }

        // =====================================
        // 当前物品显示实物
        // =====================================

        CurrentSelected = this;

        ShowReal();

        // =====================================
        // 显示描述框
        // =====================================

        string message =
            $"<color={nameColor}>"
            + $"<b>{itemName}</b>"
            + $"</color>\n"
            + itemDescription;

        ItemUIManager.Instance.ShowItem(
           
            message
        );
    }

    // =========================================
    // 显示X光图
    // =========================================

    public void ShowXray()
    {
        showingReal = false;

        if (sr != null && xraySprite != null)
        {
            sr.sprite = xraySprite;
        }
    }

    // =========================================
    // 显示实物图
    // =========================================

    public void ShowReal()
    {
        showingReal = true;

        if (sr != null && realSprite != null)
        {
            sr.sprite = realSprite;
        }
    }

    // =========================================
    // 点击空白恢复X光
    // =========================================

    void Update()
    {
        // 鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 点到UI则忽略
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // 如果当前不是选中物品
            // 并且点击到了空白区域
            if (CurrentSelected == this)
            {
                Ray ray =
                    Camera.main.ScreenPointToRay(
                        Input.mousePosition
                    );

                RaycastHit2D hit =
                    Physics2D.GetRayIntersection(ray);

                // 没点到任何物品
                if (hit.collider == null)
                {
                    ShowXray();

                    CurrentSelected = null;
                }
            }
        }
    }
}