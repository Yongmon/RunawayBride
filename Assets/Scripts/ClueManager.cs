//using UnityEngine;
//using System.Collections.Generic;

//public class ClueManager : MonoBehaviour
//{
//    public static ClueManager Instance;

//    public Transform cluePanel;          // 线索UI父物体
//    public GameObject clueItemPrefab;    // 线索按钮

//    private List<string> clues = new List<string>();

//    void Awake()
//    {
//        Instance = this;
//    }

//    public void AddClue(string clueName)
//    {
//        if (clues.Contains(clueName)) return;

//        clues.Add(clueName);

//        GameObject obj = Instantiate(clueItemPrefab, cluePanel);
//        obj.GetComponent<ClueItemUI>().Init(clueName);
//    }
//}