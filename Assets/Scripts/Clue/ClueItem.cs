using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClueItem : MonoBehaviour, IPointerClickHandler
{
    public string clueID;
    public TMP_Text text;

    public void Init(string id, string displayName)
    {
        clueID = id;
        text.text = displayName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ReasoningSystem.Instance.SelectClue(this);
    }

    public void SetSelected(bool selected)
    {
        text.color = selected ? Color.yellow : Color.white;
    }
}