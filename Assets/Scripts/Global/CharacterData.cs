using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("人物唯一ID")]
    public string characterId;

    [Header("内部名称（不给玩家看）")]
    public string internalName;

    [Header("人物头像（主界面）")]
    public Sprite portrait;

    [Header("文件夹里的头像")]
    public Sprite folderPortrait;

    [Header("行李箱背景")]
    public Sprite suitcaseBackground;

    [Header("人物场景背景")]
    public Sprite sceneBackground;

    [Header("普通线索")]
    public List<ClueData> clues;

    [Header("推理Bubble")]
    public List<BubbleData> bubbles;

    [Header("该人物拥有的物品")]
  //  public List<ItemData> items;

    [Header("是否调查完成")]
    public bool isCompleted;
}