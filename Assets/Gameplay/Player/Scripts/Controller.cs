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
    public float distanceToGoal;

    public event Action<int, float> onEnoughClickCount;
    [SerializeField] PlayerStatus playerStatus;

    //Test
    public Animator animator;
    public Vector3 target;

    public float jumpForce;
    public float jumpDuration;
    public float jumpLength;


    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(SetTargetAfterSwappingPosEnum());
    }
    IEnumerator SetTargetAfterSwappingPosEnum()
    {
        yield return new WaitForSeconds(3f);
        target = transform.position;
    }
    public void Move()
    {   
        target += Vector3.right * runLengtth;

        transform.DOMove(target, duration);
        transform.DOJump(transform.right + Vector3.right * jumpLength, jumpForce, 1, jumpDuration);
        clickCount++;
        animator.SetBool("running", true);
        

        //Test
        if (clickCount == numberOfClickTillRankUpdate)
        {
            onEnoughClickCount(playerStatus.playerId, distanceToGoal - transform.position.x); //Cuz we only moving on x axis.
            clickCount = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 0.2f);
    }

}
