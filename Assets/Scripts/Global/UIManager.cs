using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("主头像")]
    public Image portraitImage;

    [Header("文件夹头像")]
    public Image folderPortraitImage;

    [Header("行李箱背景")]
    public Image suitcaseBackgroundImage;

    [Header("场景背景")]
    public Image sceneBackgroundImage;

    // ======================================
    // 文件夹线索（这个继续动态生成）
    // ======================================

    [Header("线索父物体")]
    public Transform clueFolderParent;

    [Header("线索预制体")]
    public GameObject cluePrefab;

    // ======================================
    // 固定物品槽位（重点新增）
    // ======================================

    //[Header("场景中的物品槽位")]
   // public List<ItemUI> itemSlots;

    private void Awake()
    {
        Instance = this;
    }

    // ======================================
    // 刷新整个UI
    // ======================================

    public void RefreshUI(CharacterData data)
    {
        // ======================================
        // 刷新人物图片
        // ======================================

        portraitImage.sprite = data.portrait;

        folderPortraitImage.sprite =
            data.folderPortrait;

        suitcaseBackgroundImage.sprite =
            data.suitcaseBackground;

        sceneBackgroundImage.sprite =
            data.sceneBackground;

        // ======================================
        // 刷新场景物品（重点）
        // ======================================

       // RefreshItems(data);

        // ======================================
        // 刷新文件夹线索
        // ======================================

        RefreshClues(data);
    }

    // ======================================
    // 刷新物品
    // ======================================

    //private void RefreshItems(CharacterData data)
    //{
    //    // 遍历所有物品槽位
    //    for (int i = 0; i < itemSlots.Count; i++)
    //    {
    //        // 如果当前人物有这个物品
    //        if (i < data.items.Count)
    //        {
    //            itemSlots[i].gameObject.SetActive(true);

    //            // 设置这个槽位的数据
    //            itemSlots[i].SetItem(data.items[i]);
    //        }
    //        else
    //        {
    //            // 没有物品则隐藏
    //            itemSlots[i].gameObject.SetActive(false);
    //        }
    //    }
    //}

    // ======================================
    // 刷新文件夹线索
    // ======================================

    private void RefreshClues(CharacterData data)
    {
        // 删除旧线索UI
        foreach (Transform child in clueFolderParent)
        {
            Destroy(child.gameObject);
        }

        // 生成已收集线索
        foreach (ClueData clue in data.clues)
        {
            // 未收集则跳过
            if (!clue.collected)
                continue;

            GameObject obj =
                Instantiate(
                    cluePrefab,
                    clueFolderParent
                );

            TMP_Text text =
                obj.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.text = clue.clueName;
            }
        }
    }
}