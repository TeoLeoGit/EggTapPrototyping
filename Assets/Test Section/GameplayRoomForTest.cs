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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 8;
        if (PhotonNetwork.JoinOrCreateRoom("Test", roomOptions, TypedLobby.Default))
        {
            Debug.Log("Created/Joined room Test");
            /*foreach (Player player in PhotonNetwork.PlayerList)
                Instantiate(playerNameRow, playerListTransform);*/
            GameObject temp = Instantiate(playerNameRow, playerListTransform);
            temp.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GameObject temp = Instantiate(playerNameRow, playerListTransform);
        temp.GetComponent<Text>().text = newPlayer.NickName;
    }

    public void StartGame()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.LoadLevel("Gameplay");
    }

}
