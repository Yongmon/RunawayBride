using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReasoningUIManager : MonoBehaviour
{
    public GameObject reasoningPanel;

    public void ShowReasoningPanel()
    {
        reasoningPanel.SetActive(true);
    }

    public void HideReasoningPanel()
    {
        reasoningPanel.SetActive(false);
    }
}
