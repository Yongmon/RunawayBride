using UnityEngine;

public class SelectableClue : MonoBehaviour
{
    public string clueID;
    public Sprite normalSprite;
    public Sprite highlightSprite;

    private SpriteRenderer sr;
    private bool isSelected = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (normalSprite == null)
            normalSprite = sr.sprite;
    }

    // ⭐ 只负责显示，不处理点击
    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (sr != null)
            sr.sprite = selected ? highlightSprite : normalSprite;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}