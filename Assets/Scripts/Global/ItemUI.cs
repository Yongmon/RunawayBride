//using UnityEngine;

//public class ItemUI : MonoBehaviour
//{
//    [Header("物品SpriteRenderer")]
//    public SpriteRenderer itemRenderer;

//    private ItemData currentItemData;

//    public void SetItem(ItemData data)
//    {
//        currentItemData = data;

//        // 默认显示X光图
//        if (itemRenderer != null)
//        {
//            itemRenderer.sprite =
//                data.xraySprite;
//        }

//        // 把数据传给 ItemInfo
//        ItemInfo itemInfo =
//            GetComponent<ItemInfo>();

//        if (itemInfo != null)
//        {
//            //itemInfo.SetItemData(data);
//        }
//    }
//}