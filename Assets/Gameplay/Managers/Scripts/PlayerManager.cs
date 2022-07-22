using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct PlayerStats
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
    GameObject localPlayer;

    public event Action<List<PlayerStats>> onPlayerScoreUpdate;

    public List<PlayerStats> PlayerRanks { get { return playerRankList; } }
    public GameObject LocalPlayerObject { set { localPlayer = value; } get { return localPlayer; } }
    public void UpdatePlayerScore(int playerId, float distance)
    {
        PlayerStats update = playerRankList.Find(x => x.playerId == playerId);
        update.distance = distance;
        ResetRank();
        onPlayerScoreUpdate(playerRankList);
    }

    public void AddPlayerToLeaderboard(int playerId, string playername)
    {
        PlayerStats newPlayer = new PlayerStats(playerId, playername);
        playerRankList.Add(newPlayer);
        FindObjectOfType<UIManager>().AddPlayerToLeaderboardUI(playername);
    }

    public void ResetRank()
    {
        playerRankList.Sort((x, y) => x.distance.CompareTo(y.distance));
    }
}
