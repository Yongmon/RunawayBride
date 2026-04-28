using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;  // ⭐ 必加

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager Instance;

    [Header("UI")]
    public GameObject itemInfoPanel;
    public TMP_Text infoText; // ⭐ 改这里

    private SelectableClue currentSelected;

    void Awake()
    {
        Instance = this;

        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(false);
    }

    public void ShowItem(SelectableClue item, string message)
    {
        if (currentSelected == item)
        {
            item.SetSelected(false);
            Hide();
            return;
        }

        if (currentSelected != null)
        {
            currentSelected.SetSelected(false);
        }

        currentSelected = item;
        currentSelected.SetSelected(true);

        itemInfoPanel.SetActive(true);

        // ⭐ 直接支持富文本
        infoText.text = message;
    }

    public void Hide()
    {
        if (currentSelected != null)
        {
            currentSelected.SetSelected(false);
        }

        itemInfoPanel.SetActive(false);
        currentSelected = null;
    }
}