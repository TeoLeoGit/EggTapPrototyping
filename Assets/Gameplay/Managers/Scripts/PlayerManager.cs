using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct PlayerStats
{
    public int playerId;
    public string playername;
    public float distance; //Distance from the player to the goal.

    public PlayerStats(int playerId, string playername) : this()
    {
        this.playerId = playerId;
        this.playername = playername;
        this.distance = 0;
    }

    public PlayerStats(int playerId, string playername, float distance) : this()
    {
        this.playerId = playerId;
        this.playername = playername;
        this.distance = distance;
    }
}

public struct FinishedPlayer
{
    public int playerId;
    public string playername;
    public float raceTime; //Total racing time of a player.

    public FinishedPlayer(int playerId, string playername, float raceTime) : this()
    {
        this.playerId = playerId;
        this.playername = playername;
        this.raceTime = raceTime;
    }
}
public class PlayerManager : MonoBehaviour
{

    List<PlayerStats> playerRankList = new List<PlayerStats>();
    List<FinishedPlayer> finishedPlayersList = new List<FinishedPlayer>(); //List of players race time.

    GameObject localPlayer;
    public event Action<List<PlayerStats>, List<FinishedPlayer>> onPlayerScoreUpdate;

    public List<PlayerStats> PlayerRanks { get { return playerRankList; } }
    public GameObject LocalPlayerObject { set { localPlayer = value; } get { return localPlayer; } }

    //Events
    public event Action<int> onPlayerFinishedRacing; //Assign in PlayerManagerView.
    public event Action onAllPlayersFinished;

    public void UpdatePlayerScore(int playerId, float distance)
    {
        if (distance <= 0 && finishedPlayersList.FindIndex(x => x.playerId == playerId) == -1)
        {
            onPlayerFinishedRacing(playerId);
            return;
        }
        int index = playerRankList.FindIndex(x => x.playerId == playerId);

        if (index > -1)
        {
            playerRankList[index] = new PlayerStats(playerId, playerRankList[index].playername, distance);
            ResetRank();
        }

        onPlayerScoreUpdate(playerRankList, finishedPlayersList);
    }

    public void PlayerFinishedRacing(int playerId, float raceTime)
    {
        int index = playerRankList.FindIndex(x => x.playerId == playerId);
        if (index == -1)
            return;
        FinishedPlayer finisher = new FinishedPlayer(playerId, playerRankList[index].playername, raceTime);
        finishedPlayersList.Add(finisher);
        finishedPlayersList.Sort((x, y) => x.raceTime.CompareTo(y.raceTime));

        playerRankList.RemoveAt(index);
        ResetRank();


        //Check what this do in LevelNetworking
        if (playerRankList.Count == 0)
            onAllPlayersFinished();
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
