using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BubbleData
{
    [Header("Bubble ID")]
    public string bubbleId;

    [Header("Bubble名称")]
    public string bubbleName;

    //[Header("Bubble描述")]
    //[TextArea]
    //public string description;

    //[Header("Bubble图片")]
    //public Sprite bubbleSprite;

    [Header("是否已解锁")]
    public bool unlocked;

    // =========================
    // 推理来源
    // =========================

    [Header("需要的线索ID")]
    public List<string> requiredClues;

    // =========================
    // 审问内容
    // =========================

    [Header("提问内容")]
    [TextArea]
    public string askText;

    [Header("角色回答")]
    [TextArea]
    public string answerText;
}