using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //Spawn position test.
    public int numberOfPlayer;
    public Transform[] spawnPositions;
    public Transform clientPosition;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Test random position.
    public void RandomSpawnPosition()
    {
        for (int i = 0; i < numberOfPlayer; i++)
        {
            int randomIndex = Random.Range(i, numberOfPlayer);
            clientPosition = spawnPositions[randomIndex];
            //playerList[randomIndex] = temp;
        }
    }
}
