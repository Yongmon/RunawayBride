using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLinkClicker : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textComponent;

    [Header("可选：UI相机（Screen Space - Camera模式需要）")]
    public Camera uiCamera;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("【诊断】鼠标点击坐标: " + eventData.position);

        textComponent.ForceMeshUpdate();

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            textComponent,
            eventData.position,
            eventData.pressEventCamera
        );

        if (linkIndex == -1)
        {
            Debug.LogWarning("❌ 没点到link");
            return;
        }

        TMP_LinkInfo linkInfo = textComponent.textInfo.linkInfo[linkIndex];
        string clueID = linkInfo.GetLinkID();

        Debug.Log("✅ 点击线索：" + clueID);

        // ⭐ 1. 加入 ClueManager（核心）
        if (ClueManager.Instance != null)
        {
            if (!ClueManager.Instance.HasClue(clueID))
            {
                ClueManager.Instance.AddClue(clueID);

                // ⭐ 2. 触发事件（如果你还想用这个）
               // OnClueAdded?.Invoke(clueID);

                // ⭐ 3. 闪烁效果
                StartCoroutine(FlickerEffect(linkInfo));
            }
            else
            {
                Debug.Log("⚠️ 已经收集过");
            }
        }
    }

    // ✨ 点击闪烁效果（只闪被点击的那一段）
    private IEnumerator FlickerEffect(TMP_LinkInfo linkInfo)
    {
        textComponent.ForceMeshUpdate(); // 确保数据最新
        // 获取文字mesh
        TMP_TextInfo textInfo = textComponent.textInfo;

        Color32[] originalColors = new Color32[textInfo.meshInfo[0].colors32.Length];
        textInfo.meshInfo[0].colors32.CopyTo(originalColors, 0);

        // 修改link对应字符颜色
        for (int i = 0; i < linkInfo.linkTextLength; i++)
        {
            int charIndex = linkInfo.linkTextfirstCharacterIndex + i;

            if (charIndex >= textInfo.characterCount)
                continue;

            int meshIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

            Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;

            // 改为黄色
            vertexColors[vertexIndex + 0] = Color.yellow;
            vertexColors[vertexIndex + 1] = Color.yellow;
            vertexColors[vertexIndex + 2] = Color.yellow;
            vertexColors[vertexIndex + 3] = Color.yellow;
        }

        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        yield return new WaitForSeconds(0.15f);

        // 恢复颜色
        textInfo.meshInfo[0].colors32 = originalColors;
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}