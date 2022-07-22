using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManagerView : MonoBehaviourPun
{
    [Header("[Do not change in inspector.]")]
    [SerializeField] PlayerSpawner playerSpawner;
    [SerializeField] PlayerManager playerManager;

    public int spawnedPlayersCount = 0;
    public int settedPlayersCount = 0;

    private void Start()
    {
        playerSpawner = GetComponent<PlayerSpawner>();
        playerManager = GetComponent<PlayerManager>();
        Debug.Log("Spawn position index: " + PlayerInitialProperties.startPositionIndex);
        playerSpawner.SpawnPlayer(PlayerInitialProperties.startPositionIndex);

        StartCoroutine(SyncPlayerListEnum());
    }

    public void UpdateLeaderboardInRoom(int playerId, float distance)
    {
        photonView.RPC(nameof(RPC_UpdateLeaderboardInRoom), RpcTarget.All, playerId, distance);
    }

    [PunRPC]
    void RPC_UpdateLeaderboardInRoom(int playerId, float distance)
    {
        playerManager.UpdatePlayerScore(playerId, distance);
    }

    IEnumerator SyncPlayerListEnum()
    {
        while (spawnedPlayersCount < PhotonNetwork.PlayerList.Length) 
        {
            yield return null;
        }
        playerManager.LocalPlayerObject.GetComponent<PlayerNetworkView>().SyncPlayerInfo();
        while (settedPlayersCount < PhotonNetwork.PlayerList.Length)
        {
            yield return null;
        }
        playerSpawner.SwapLocalPlayerToFirstPosition(playerManager.LocalPlayerObject.transform, PlayerInitialProperties.startPositionIndex);
    }
}

