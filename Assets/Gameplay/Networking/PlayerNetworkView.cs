using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerNetworkView : MonoBehaviourPun
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Text PlayernameText;

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
            GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
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
            PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName, (PhotonNetwork.PlayerList.Length - 1) -
                PlayerInitialProperties.startPositionIndex);
    }

    [PunRPC]
    void RPC_SyncPlayerInfo(int id, string playername, int layerOrder)
    {
        playerInput.playerId = id;
        playerInput.playerName = playername;
        FindObjectOfType<PlayerManager>().AddPlayerToLeaderboard(id, playername);
        GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
        GetComponentInChildren<MeshRenderer>().sortingOrder = layerOrder;
        FindObjectOfType<PlayerManagerView>().settedPlayersCount++;

        //
        PlayernameText.text = playername;
    }
}
