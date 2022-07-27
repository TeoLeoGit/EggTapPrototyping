using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Spine.Unity;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] Transform playerPrefab;
    [SerializeField] List<Transform> startPositions;
    public Transform localPlayerTransform;
    
    public void SpawnPlayer(int posIndex)
    {
        //Have to use network instantiate here, Spawner must be a network component.
        //Instantiate player -> Adding gap distance between real position and first position to network view.
        //Add listener to UI for events.
        localPlayerTransform = PhotonNetwork.Instantiate(playerPrefab.name, startPositions[posIndex].position, Quaternion.identity).transform;
        /*localPlayerTransform.GetComponent<EggTapPhotonTransformView>().gapDistance = startPositions[posIndex].position - startPositions[0].position;*/

        //Test tweening transformview
        localPlayerTransform.GetComponent<TweeningTransformView>().gapDistance = startPositions[posIndex].position - startPositions[0].position;
        GetComponent<PlayerManager>().LocalPlayerObject = localPlayerTransform.gameObject;

        FindObjectOfType<UIManager>().tapButton.onClick.AddListener(localPlayerTransform.GetComponent<Controller>().Move);
    }

    public void SwapLocalPlayerToFirstPosition(Transform playerTransform, int prevPosIndex)
    {
        if (prevPosIndex == 0)
        {
            FindObjectOfType<LevelNetworking>().NotifyLocalPlayerReady();
            return;
        }
        playerTransform.position = startPositions[0].position;

        //Find and swap position with player at first place.
        PlayerNetworkView[] playersControllers = FindObjectsOfType<PlayerNetworkView>();
        
        foreach (PlayerNetworkView playerView in playersControllers)
        {
            SpriteRenderer renderer = playerView.GetComponent<SpriteRenderer>();

            /*Debug.Log("Player " + playerView.GetComponent<PhotonView>().Owner.NickName + " has sprite order: " + renderer.sortingOrder);*/
            if (renderer.sortingOrder == PhotonNetwork.PlayerList.Length - 1)
            {
                renderer.sortingOrder = playerTransform.GetComponent<SpriteRenderer>().sortingOrder;
                renderer.GetComponentInChildren<MeshRenderer>().sortingOrder = playerTransform.GetComponent<SpriteRenderer>().sortingOrder;

                playerView.transform.position = startPositions[prevPosIndex].position;

                playerTransform.GetComponent<SpriteRenderer>().sortingOrder = PhotonNetwork.PlayerList.Length - 1;
                playerTransform.GetComponentInChildren<MeshRenderer>().sortingOrder = PhotonNetwork.PlayerList.Length - 1;

                //Mark the previous player at 1st position because he has difference way to sync movement.
                /* EggTapPhotonTransformView transformView = playerView.GetComponent<EggTapPhotonTransformView>();
                 transformView.isAt1stPosition = true;
                 transformView.gapDistance = startPositions[prevPosIndex].position - startPositions[0].position;*/

                //Testing tweening synch
                TweeningTransformView transformView = playerView.GetComponent<TweeningTransformView>();
                transformView.isAt1stPosition = true;
                transformView.gapDistance = startPositions[prevPosIndex].position - startPositions[0].position;
                //

                FindObjectOfType<LevelNetworking>().NotifyLocalPlayerReady(); //Ready
                return;
            }
        }
    }
}
