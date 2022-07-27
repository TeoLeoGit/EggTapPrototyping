using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerNetworkView : MonoBehaviourPun
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] Text PlayernameText;

    //test
    public Controller controller;
    public float tapRate;
    float currentTapRate = 0f;
    bool isAllowedToMove = false;


    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
        tapRate = Random.Range(0.2f, 0.8f);

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
            StartCoroutine(AutoMoveTest());
        }
        

    }

    IEnumerator AutoMoveTest()
    {
        yield return new WaitForSeconds(3f);
        controller.target = transform.position;
        isAllowedToMove = true;
    }
    private void FixedUpdate()
    {
        if (!isAllowedToMove)
            return;
        if (currentTapRate <= 0)
        {
            controller.Move();
            currentTapRate = tapRate;
        }
        else
        {
            controller.animator.SetBool("running", true);
            currentTapRate -= Time.fixedDeltaTime;
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
        playerStatus.playerId = id;
        playerStatus.playerName = playername;
        FindObjectOfType<PlayerManager>().AddPlayerToLeaderboard(id, playername);
        GetComponent<SpriteRenderer>().sortingOrder = layerOrder;
        GetComponentInChildren<MeshRenderer>().sortingOrder = layerOrder;
        FindObjectOfType<PlayerManagerView>().settedPlayersCount++;
        controller.distanceToGoal = FindObjectOfType<GameLevel>().distanceToGoal;
        //Test
        PlayernameText.text = playername;
    }
}
