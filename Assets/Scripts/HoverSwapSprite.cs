using UnityEngine;

public class HoverSwapSprite : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite highlightSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 如果未指定普通精灵，则使用物体当前精灵
        if (normalSprite == null) normalSprite = spriteRenderer.sprite;
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = highlightSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = normalSprite;
    }
}