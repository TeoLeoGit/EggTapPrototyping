using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float stallTime;
    public float showTime;
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

    public void UpdateLeaderboardUI(List<PlayerStats> playerRankList, List<FinishedPlayer> finishedPlayersList)
    {
        //Debug.Log("Updating UI.");
        for (int i = 0; i < finishedPlayersList.Count; i++)
        {
            rowsTransforms[i].Find("Playername").GetComponent<Text>().text = finishedPlayersList[i].playername + "  " + finishedPlayersList[i].raceTime + " s";
        }

 /*       foreach (PlayerStats player in playerRankList)
            Debug.Log("Distance: " + player.distance);

        foreach (FinishedPlayer player in finishedPlayersList)
            Debug.Log("Racetime: " + player.raceTime);
*/
        int j = 0;
        for (int i = finishedPlayersList.Count; i < rowsTransforms.Count; i++)
        {
            rowsTransforms[i].Find("Playername").GetComponent<Text>().text = playerRankList[j].playername + " " + playerRankList[j].distance;
            j++;
        }
    }

    public IEnumerator ShowFinalLeaderboardUI()
    {
        yield return new WaitForSeconds(stallTime);
        Debug.Log("End!");
        yield return new WaitForSeconds(showTime);
        Debug.Log("Result is ...");
    }
}
