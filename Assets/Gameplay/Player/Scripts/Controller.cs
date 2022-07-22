using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Controller : MonoBehaviour
{
    [SerializeField] [Range(0f, 4f)] float duration;
    [SerializeField] [Range(1, 10)] int numberOfClickTillRankUpdate;
    [SerializeField] float runLengtth;
    public int clickCount = 0;
    public float goalXPosition;

    public event Action<int, float> onEnoughClickCount;
    [SerializeField] PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    public void Move()
    {
        transform.DOMove(transform.position + Vector3.right * runLengtth, duration).SetEase(Ease.OutCubic);
        clickCount++;
        if (clickCount == numberOfClickTillRankUpdate)
        {
            onEnoughClickCount(playerInput.playerId, goalXPosition - transform.position.x);
            clickCount = 0;
        }
    }

}
