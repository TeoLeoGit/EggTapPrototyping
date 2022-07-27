using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelNetworking : MonoBehaviourPun
{
    [SerializeField] GameLevel level;
    bool isStarted = false;
    bool isFinishedByAllPlayers = false;
    public int readyPlayersCount = 0;

    private void Start()
    {
        FindObjectOfType<PlayerManager>().onAllPlayersFinished += TriggerEndGame;
        StartCoroutine(WaitForAllPlayersReadyEnum());
    }

    private void FixedUpdate()
    {
        if (isStarted)
            level.timeLeft = level.timeLimit - ((float)PhotonNetwork.Time - level.startTime);
        else
            return;

        if (level.timeLeft <= 0 || isFinishedByAllPlayers)
        {
            level.EndGame();
            isStarted = false;
        }
        
    }

    IEnumerator WaitForAllPlayersReadyEnum()
    {
        while(true)
        {
            yield return null;
            if (readyPlayersCount == PhotonNetwork.PlayerList.Length)
            {
                isStarted = true;
                level.startTime = (float)PhotonNetwork.Time;
                break;
            }
            
        }
    }

    void TriggerEndGame()
    {
        isFinishedByAllPlayers = true;
    }

    public void NotifyLocalPlayerReady()
    {
        photonView.RPC(nameof(RPC_NotifyReadyPlayer), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_NotifyReadyPlayer()
    {
        readyPlayersCount++;
    }
}
