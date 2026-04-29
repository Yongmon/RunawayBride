using UnityEngine;
using TMPro;

public class ItemUIManager : MonoBehaviour
{
    public static ItemUIManager Instance;

    [Header("UI")]
    public GameObject itemInfoPanel;
    public TextMeshProUGUI infoText;   // 保持这个类型即可
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

        currentSelected?.SetSelected(false);
        currentSelected = item;
        currentSelected.SetSelected(true);

        itemInfoPanel.SetActive(true);
        infoText.text = message;
    }

    public void Hide()
    {
        currentSelected?.SetSelected(false);
        itemInfoPanel.SetActive(false);
        currentSelected = null;
    }
}