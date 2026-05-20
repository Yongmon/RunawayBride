using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 确保一开始是隐藏的
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void Show(string text)
    {
        dialogueText.text = text;
        dialoguePanel.SetActive(true);
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }

    public void OnContinueClicked()
    {
        Hide();
    }
}