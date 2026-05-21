using UnityEngine;
using TMPro;
using System.Text;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("竖排设置")]
    [SerializeField] private bool verticalMode = true;

    [SerializeField] private int charsPerColumn = 8;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void Show(string text)
    {
        if (verticalMode)
        {
            dialogueText.text = ConvertVertical(text);
        }
        else
        {
            dialogueText.text = text;
        }

        dialoguePanel.SetActive(true);
    }

    private string ConvertVertical(string str)
    {
        int rows = charsPerColumn;

        // 计算需要多少列
        int cols = Mathf.CeilToInt((float)str.Length / rows);

        char[,] grid = new char[rows, cols];

        int index = 0;

        // 从右往左填充
        for (int col = cols - 1; col >= 0; col--)
        {
            for (int row = 0; row < rows; row++)
            {
                if (index < str.Length)
                {
                    grid[row, col] = str[index];
                    index++;
                }
            }
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // 按行输出
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                char c = grid[row, col];

                if (c == '\0')
                    sb.Append("　"); // 中文空格
                else
                    sb.Append(c);

                sb.Append(" ");
            }

            sb.Append("\n");
        }

        return sb.ToString();
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