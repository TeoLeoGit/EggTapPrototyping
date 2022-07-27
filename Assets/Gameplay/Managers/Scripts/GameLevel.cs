using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    public float startTime;
    public float timeLeft;
    public float timeLimit;
    public float distanceToGoal;

    private void Awake()
    {
        timeLeft = timeLimit;
    }
    public void EndGame()
    {
        StartCoroutine(FindObjectOfType<UIManager>().ShowFinalLeaderboardUI());

    }

}
