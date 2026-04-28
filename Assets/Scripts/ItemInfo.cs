using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfo : MonoBehaviour
{
    [Header("物品信息")]
    public string itemName = "物品名称";

    [TextArea]
    public string itemDescription = "这是物品的描述。";

    [Header("标题颜色设置")]
    public string nameColor = "#1E3A8A"; // 深蓝（推荐）

    private SelectableClue selectable;

    void Start()
    {
        selectable = GetComponent<SelectableClue>();
    }

    void OnMouseDown()
    {
        // 如果点在UI上，就忽略
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // ⭐ 给物品名字加颜色 + 加粗
        string message = $"<color={nameColor}><b>{itemName}</b></color>\n{itemDescription}";


        ItemUIManager.Instance.ShowItem(selectable, message);
        // ⭐ 点击物品时收集线索
        //ClueManager.Instance.AddClue(itemName);
    }
}