using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] Transform playerPrefab;
    [SerializeField] List<Transform> startPositions;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer(PlayerInitialProperties.startPositionIndex);
    }
    
    void SpawnPlayer(int posIndex)
    {
        Transform playerTransform = PhotonNetwork.Instantiate(playerPrefab.name, startPositions[posIndex].position, Quaternion.identity).transform;
        SwapLocalPlayerToFirstPosition(playerTransform);

    }

    void SwapLocalPlayerToFirstPosition(Transform playerTransform)
    {

    }
}
