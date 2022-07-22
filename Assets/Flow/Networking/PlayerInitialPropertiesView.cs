using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInitialPropertiesView : MonoBehaviourPun
{
    // Start is called before the first frame update
    [Header("Properties that's initialized at runtime.., do not change in inspector.")]
    [SerializeField] PlayerInitialProperties playerInitialProperties;
    void Start()
    {
        playerInitialProperties = GetComponent<PlayerInitialProperties>();
    }

    public void RandomPlayersStartPositions()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int[] playerPositionIndexes = playerInitialProperties.RandomPositionsIndex(PhotonNetwork.PlayerList.Length);
            //int[] playerPositionIndexes = playerInitialProperties.RandomPositionsIndex(8);

            int i = 0;
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                photonView.RPC(nameof(RPC_SetStartPositionIndex), player, playerPositionIndexes[i]);
                i++;
            }
        } 
        
    }

    [PunRPC]
    void RPC_SetStartPositionIndex(int index)
    {
        PlayerInitialProperties.startPositionIndex = index;
        photonView.RPC(nameof(RPC_NotifyPlayerReady), RpcTarget.MasterClient);
    }

    [PunRPC]
    void RPC_NotifyPlayerReady()
    {
        GetComponent<PlayerInitialProperties>().readyPlayerCount++;
    }
}
