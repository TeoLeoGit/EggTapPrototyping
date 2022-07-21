using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PlayerStats
{
    public int playerId;
    public string playerName;
    public float distance; //Distance from the player to the goal.

    public PlayerStats(int playerId, string playerName) : this()
    {
        this.playerId = playerId;
        this.playerName = playerName;
        this.distance = 0;
    }
}
public class PlayerManager : MonoBehaviour
{
    [SerializeField] List<PlayerStats> playerRankList = new List<PlayerStats>();
    [SerializeField] List<PlayerStats> finalRankList = new List<PlayerStats>(); //list of players race time.

    public void UpdatePlayerScore(int playerId, float distance)
    {
        PlayerStats update = playerRankList.Find(x => x.playerId == playerId);
        update.distance = distance;
        ResetRank();
        foreach(var item in playerRankList)
        {
            Debug.Log("Player: " + item.playerName);
        }
    }

    public void AddPlayerToLeaderboard(int playerId, string playerName)
    {
        PlayerStats newPlayer = new PlayerStats(playerId, playerName);
        playerRankList.Add(newPlayer);
    }

    public void ResetRank()
    {
        playerRankList.Sort((x, y) => x.distance.CompareTo(y.distance));
    }
}
