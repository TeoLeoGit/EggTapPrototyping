using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        localPlayerTransform.GetComponent<EggTapPhotonTransformView>().gapDistance = startPositions[posIndex].position - startPositions[0].position;
        localPlayerTransform.GetComponent<EggTapPhotonTransformView>().startPosition = startPositions[posIndex];
        GetComponent<PlayerManager>().LocalPlayerObject = localPlayerTransform.gameObject;

        FindObjectOfType<UIManager>().tapButton.onClick.AddListener(localPlayerTransform.GetComponent<Controller>().Move);
    }

    public void SwapLocalPlayerToFirstPosition(Transform playerTransform, int prevPosIndex)
    {
        if (prevPosIndex == 0)
            return;
        playerTransform.position = startPositions[0].position;

        //Find and swap position with player at first place.
        PlayerNetworkView[] playersControllers = FindObjectsOfType<PlayerNetworkView>();
        
        foreach (PlayerNetworkView playerView in playersControllers)
        {
            SpriteRenderer renderer = playerView.GetComponent<SpriteRenderer>();
            /*Debug.Log("Player " + playerView.GetComponent<PhotonView>().Owner.NickName + " has sprite order: " + renderer.sortingOrder);*/
            if (renderer.sortingOrder == 0)
            {
                renderer.sortingOrder = prevPosIndex;
                playerView.transform.position = startPositions[prevPosIndex].position;
                playerTransform.GetComponent<SpriteRenderer>().sortingOrder = 0;

                //Mark the previous player at 1st position because he has difference way to sync movement.
                EggTapPhotonTransformView transformView = playerView.GetComponent<EggTapPhotonTransformView>();
                transformView.isAt1stPosition = true;
                transformView.gapDistance = startPositions[prevPosIndex].position - startPositions[0].position;

                return;
            }
        }
    }
}
