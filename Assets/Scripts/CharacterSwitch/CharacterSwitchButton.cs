using UnityEngine;

public class CharacterSwitchButton : MonoBehaviour
{
    [Header("要切换的人物ID")]
    public string targetCharacterId;

    // =========================
    // 按钮点击
    // =========================

    public void SwitchCharacter()
    {
        // 调用GameManager切换人物
        GameManager.Instance.SwitchCharacter(
            targetCharacterId
        );
    }
}