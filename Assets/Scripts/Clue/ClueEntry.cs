using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClueEntry : MonoBehaviour, IPointerClickHandler
{
    public string clueID;               // 线索ID
    public TMP_Text label;              // 显示文本组件
    private ClueManager manager;

    private void Awake()
    {
        manager = ClueManager.Instance;
    }

    // 由 ClueManager 调用，初始化数据
    public void SetUp(string id, string displayName)
    {
        clueID = id;

        Debug.Log("SetUp执行：" + id);

        if (label != null)
        {
            label.text = displayName;
            Debug.Log("设置文本：" + displayName);
        }
        else
        {
            Debug.LogError("❌ label 没绑定");
        }
    }
    public void SetSelected(bool selected)
    {
        if (label != null)
            label.color = selected ? Color.yellow : Color.white;
    }

    // 点击事件
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("点到了 ClueEntry：" + gameObject.name);
        Debug.Log("ClueEntry 被点击：" + clueID);
        if (manager == null)
        {
            Debug.LogError("❌ ClueManager 没初始化");
            return;
        }

        manager.OnClueEntryClicked(this);
    }
}