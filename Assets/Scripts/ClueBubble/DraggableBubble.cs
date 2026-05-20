using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class DraggableBubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;                 // 根 Canvas
    private Transform originalParent;      // 气泡原本所在的父级（布局容器）
    private Vector2 originalLocalPosition; // 拖拽前在布局容器里的位置
    private Vector2 screenOffset;          // 鼠标点击位置相对于气泡中心的屏幕坐标差值

    [Header("拖放检测")]
    [SerializeField] private LayerMask characterLayerMask = -1;
    [SerializeField] private float maxRayDistance = 100f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 获取根 Canvas（避免多层 Canvas 干扰）
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
            canvas = parentCanvas.rootCanvas;
        else
            canvas = FindObjectOfType<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 记录原始父级和位置
        originalParent = rectTransform.parent;
        originalLocalPosition = rectTransform.localPosition;

        // 将气泡移到根 Canvas 下，脱离布局组约束
        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        // 计算屏幕空间偏移：气泡中心屏幕坐标 - 鼠标点击屏幕坐标
        Vector3 bubbleScreenPos = RectTransformUtility.WorldToScreenPoint(
            eventData.pressEventCamera,
            rectTransform.position);
        screenOffset = (Vector2)bubbleScreenPos - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 目标屏幕坐标 = 当前鼠标位置 + 初始偏移
        Vector2 targetScreenPos = eventData.position + screenOffset;

        // 将屏幕坐标转为世界坐标，直接设置气泡位置
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            targetScreenPos,
            eventData.pressEventCamera,
            out worldPos))
        {
            rectTransform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 1. 获取 2D 物理需要的世界坐标
        Vector3 worldPoint = Vector3.zero;

        if (Camera.main != null)
        {
            // 主相机存在时，直接转换屏幕坐标到世界坐标（注意 z 轴设为 0 或相机近裁面距离）
            worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            worldPoint.z = 0; // 确保在 2D 平面上
        }
        else
        {
            // 如果没有主相机，尝试用 Canvas 的 worldCamera（适用于 Screen Space - Camera 模式）
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.worldCamera != null)
            {
                worldPoint = canvas.worldCamera.ScreenToWorldPoint(eventData.position);
                worldPoint.z = 0;
            }
            else
            {
                Debug.LogError("没有可用的相机来转换坐标，请检查场景中的 Camera 设置。");
                // 回到原位
                transform.SetParent(originalParent, true);
                rectTransform.localPosition = originalLocalPosition;
                return;
            }
        }

        // 2. 使用 2D 重叠点检测是否有人物
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPoint, characterLayerMask);

        if (hitCollider != null)
        {
            Debug.Log($"2D 检测命中：{hitCollider.name}，标签：{hitCollider.tag}");

            if (hitCollider.CompareTag("Character"))
            {
                CharacterDialogue dialogue = hitCollider.GetComponent<CharacterDialogue>();
                if (dialogue != null)
                {
                    TMP_Text bubbleText = GetComponentInChildren<TMP_Text>();
                    string clue = bubbleText != null ? bubbleText.text : "默认线索";
                    dialogue.StartDialogue(clue);
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    Debug.LogWarning("命中人物上缺少 CharacterDialogue 组件");
                }
            }
        }
        else
        {
            Debug.Log("2D 检测未命中任何人物");
        }

        // 未命中或条件不满足：回到原位
        transform.SetParent(originalParent, true);
        rectTransform.localPosition = originalLocalPosition;
    }
}