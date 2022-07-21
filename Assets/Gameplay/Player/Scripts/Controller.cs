using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Controller : MonoBehaviour
{
    [SerializeField] [Range(0f, 4f)] float duration;
    [SerializeField] [Range(1, 10)] int numberOfClickTillRankUpdate;
    public int clickCount = 0;
    public Transform goalTransform;

    public event Action<int, float> onEnoughClickCount;
    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        onEnoughClickCount += FindObjectOfType<PlayerManager>().UpdatePlayerScore;
    }
    public void Move()
    {
        transform.DOMove(transform.position + Vector3.right * 0.1f, duration).SetEase(Ease.OutCubic);
        clickCount++;
        if (clickCount == numberOfClickTillRankUpdate)
        {
            onEnoughClickCount(playerInput.playerId, goalTransform.position.x - transform.position.x);
            clickCount = 0;
        }
    }

}
