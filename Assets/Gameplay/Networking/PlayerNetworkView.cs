using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkView : MonoBehaviourPun
{
    [SerializeField] PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

    }
    private void Start()
    {
        FindObjectOfType<PlayerManagerView>().spawnedPlayersCount++;

        if (!photonView.IsMine)
        {
            Destroy(GetComponent<Controller>());

            //Test.
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        }
        else
        {
            GetComponent<Controller>().onEnoughClickCount += FindObjectOfType<PlayerManagerView>().UpdateLeaderboardInRoom;
            //Test.
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        

    }

    public void SyncPlayerInfo()
    {
        photonView.RPC(nameof(RPC_SyncPlayerInfo), RpcTarget.AllBuffered,
            PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName, PlayerInitialProperties.startPositionIndex);
    }

    [PunRPC]
    void RPC_SyncPlayerInfo(int id, string playername, int layerOrder)
    {
        playerInput.playerId = id;
        playerInput.playerName = playername;
        FindObjectOfType<PlayerManager>().AddPlayerToLeaderboard(id, playername);
        GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
        FindObjectOfType<PlayerManagerView>().settedPlayersCount++;
    }
}
