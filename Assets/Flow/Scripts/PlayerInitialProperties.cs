using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitialProperties : MonoBehaviour
{
    public static int startPositionIndex;

    public int[] RandomPositionsIndex(int numberOfPlayer)
    {
        if (numberOfPlayer < 0)
            return null;
        int[] playerPositionIndexes = new int[numberOfPlayer];
        for (int i = 0; i < numberOfPlayer; i++)
        {
            int prevIndex = playerPositionIndexes[i];
            int randomIndex = Random.Range(i, numberOfPlayer);
            playerPositionIndexes[i] = randomIndex;
            playerPositionIndexes[randomIndex] = prevIndex;
        }
        return playerPositionIndexes;
    }

}
