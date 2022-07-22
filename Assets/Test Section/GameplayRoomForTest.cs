using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameplayRoomForTest : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerNameRow;
    [SerializeField] Transform playerListTransform;


    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        PhotonNetwork.LocalPlayer.NickName = "Tester" + Random.Range(1, 100);
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        if (PhotonNetwork.JoinOrCreateRoom("Test", roomOptions, TypedLobby.Default))
        {
            Debug.Log("Created/Joined room Test");
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                GameObject temp = Instantiate(playerNameRow, playerListTransform);
                temp.GetComponent<Text>().text = player.NickName;
            }
        }
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GameObject temp = Instantiate(playerNameRow, playerListTransform);
        temp.GetComponent<Text>().text = newPlayer.NickName;
        Debug.Log("Number of player: " + PhotonNetwork.PlayerList.Length);
        Debug.Log("View ID of the object: " + photonView);

    }

    public void StartGame()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            GetComponent<PlayerInitialPropertiesView>().RandomPlayersStartPositions();
            StartCoroutine(StartGameEnum());
        }
    }

    IEnumerator StartGameEnum()
    {
        while (GetComponent<PlayerInitialProperties>().readyPlayerCount < PhotonNetwork.PlayerList.Length)
        {
            yield return new WaitForSeconds(0.2f);
        }
        PhotonNetwork.LoadLevel("Gameplay");
    }

}
