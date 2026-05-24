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

    // 切换人物
    public void SwitchCharacter(string characterId)
    {
        currentCharacter =
            allCharacters.Find(
                c => c.characterId == characterId
            );

        if (currentCharacter != null)
        {
            Debug.Log("切换人物：" + currentCharacter.internalName);

            UIManager.Instance.RefreshUI(currentCharacter);
        }
    }
}