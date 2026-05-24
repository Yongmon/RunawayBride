using UnityEngine;

[System.Serializable]
public class ClueData
{
    [Header("线索ID")]
    public string clueId;

    [Header("线索名称")]
    public string clueName;

    [Header("线索描述")]
    [TextArea]
    public string description;

    [Header("线索图片")]
    public Sprite clueSprite;

    [Header("是否已收集")]
    public bool collected;
}