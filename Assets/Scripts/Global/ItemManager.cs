using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("物品预制体")]
    public GameObject itemPrefab;

    [Header("物品父物体")]
    public Transform itemParent;

    private List<GameObject> currentItems =
        new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // 刷新当前人物物品
    public void RefreshItems(CharacterData data)
    {
        // 删除旧物品
        foreach (GameObject obj in currentItems)
        {
            Destroy(obj);
        }

        currentItems.Clear();

        // 生成新物品
        foreach (ItemData item in data.items)
        {
            GameObject obj =
                Instantiate(
                    itemPrefab,
                    itemParent
                );

            obj.transform.localPosition =
                item.position;

            obj.transform.localScale =
                item.scale;

            // 初始化数据
            ItemInfo info =
                obj.GetComponent<ItemInfo>();

            info.SetItem(item);

            currentItems.Add(obj);
        }
    }
}