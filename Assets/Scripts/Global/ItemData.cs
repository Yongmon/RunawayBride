using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("物品ID")]
    public string itemId;

    [Header("物品名称")]
    public string itemName;

    // =========================
    // 图片
    // =========================

    [Header("X光图片")]
    public Sprite xraySprite;

    [Header("实物图片")]
    public Sprite realSprite;

    // =========================
    // 描述
    // =========================

    [Header("物品描述")]
    [TextArea]
    public string itemDescription;

    [Header("标题颜色")]
    public string nameColor = "#1E3A8A";
}