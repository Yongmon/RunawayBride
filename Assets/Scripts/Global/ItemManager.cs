using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("物品预制体")]
    public GameObject itemPrefab;

    [Header("物品父物体")]
    public Transform itemParent;

    // 当前场景中的物品
    private List<GameObject> currentItems =
        new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // =====================================
    // 刷新当前人物物品
    // =====================================

    public void RefreshItems(CharacterData data)
    {
        Debug.Log("===== 开始刷新物品 =====");

        // =========================
        // 安全检查
        // =========================

        if (data == null)
        {
            Debug.LogError("data为空！");
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("itemPrefab 没拖！");
            return;
        }

        if (itemParent == null)
        {
            Debug.LogError("itemParent 没拖！");
            return;
        }

        // =========================
        // 删除旧物品
        // =========================

        foreach (GameObject obj in currentItems)
        {
            Destroy(obj);
        }

        currentItems.Clear();

        // =========================
        // 生成新物品
        // =========================

        foreach (ItemData item in data.items)
        {
            Debug.Log("正在生成物品：" + item.itemName);

            GameObject obj =
                Instantiate(
                    itemPrefab,
                    itemParent
                );

            // =========================
            // RectTransform
            // =========================

            RectTransform rt =
                obj.GetComponent<RectTransform>();

            if (rt == null)
            {
                Debug.LogError("ItemPrefab 没有 RectTransform！");
                continue;
            }

            // UI位置
            rt.anchoredPosition =
                item.uiPosition;

            // UI大小
            rt.sizeDelta =
                item.uiSize;

            // 缩放固定
            rt.localScale =
                Vector3.one;

            // =========================
            // 设置图片
            // =========================

            ItemInfo info =
                obj.GetComponent<ItemInfo>();

            if (info == null)
            {
                Debug.LogError("itemPrefab 上没有 ItemInfo！");
                continue;
            }

            info.SetItem(item);

            currentItems.Add(obj);
        }

        Debug.Log("===== 物品刷新完成 =====");
    }
}