using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("所有人物数据")]
    public List<CharacterData> allCharacters;

    [Header("当前人物")]
    public CharacterData currentCharacter;

    private void Awake()
    {
        Instance = this;
    }
    // =========================
    // 游戏开始默认加载第一个人物
    // =========================

    private void Start()
    {
        if (allCharacters.Count > 0)
        {
            SwitchCharacter(
                allCharacters[0].characterId
            );
        }
    }


    // 切换人物
    public void SwitchCharacter(string characterId)
    {
        CharacterData target = null;

        foreach (CharacterData c in allCharacters)
        {
            if (c.characterId == characterId)
            {
                target = c;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("找不到角色：" + characterId);
            return;
        }

        currentCharacter = target;

        Debug.Log("切换人物：" + target.internalName);

        // 刷新UI
        UIManager.Instance.RefreshUI(target);

        // 刷新物品
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RefreshItems(target);
        }
    }
}