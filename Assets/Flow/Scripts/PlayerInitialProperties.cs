using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialProperties : MonoBehaviour
{
    public static int startPositionIndex;
    public int readyPlayerCount = 0;

    public int[] RandomPositionsIndex(int numberOfPlayer)
    {
        if (numberOfPlayer < 0)
            return null;
        int[] playerPositionIndexes = new int[numberOfPlayer];
        for (int i = 0; i < numberOfPlayer; i++)
        {
            playerPositionIndexes[i] = i;
        }

        for (int i = 0; i < numberOfPlayer; i++)
        {
            int prevIndex = playerPositionIndexes[i];
            int randomIndex = Random.Range(i, numberOfPlayer - 1);
            playerPositionIndexes[i] = playerPositionIndexes[randomIndex];
            playerPositionIndexes[randomIndex] = prevIndex;
        }

        //Debuging
        foreach(int i in playerPositionIndexes)
        {
            Debug.Log("Position random: "+ i);
        }
        return playerPositionIndexes;
    }

}
