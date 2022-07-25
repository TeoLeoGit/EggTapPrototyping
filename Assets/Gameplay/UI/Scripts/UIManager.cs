using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button tapButton;
    [SerializeField] RectTransform leaderboard;
    [SerializeField] GameObject rowFramePrefab;

    [Header("[Do not change in editor.]")]
    [SerializeField] List<Transform> rowsTransforms;

    private void Start()
    {
        FindObjectOfType<PlayerManager>().onPlayerScoreUpdate += UpdateLeaderboardUI;
    }
    public void AddPlayerToLeaderboardUI(string playername)
    {
        GameObject rowFrame = Instantiate(rowFramePrefab, leaderboard);
        Transform playerNameText = rowFrame.transform.Find("Playername");

        //Must change later.
        playerNameText.GetComponent<Text>().text = playername;
        rowsTransforms.Add(rowFrame.transform);
    }

    public void UpdateLeaderboardUI(List<PlayerStats> playerRankList)
    {
        Debug.Log("Updating UI.");
        for (int i = 0; i < rowsTransforms.Count; i++)
        {
            rowsTransforms[i].Find("Playername").GetComponent<Text>().text = playerRankList[i].playerName;
        }
    }
}
