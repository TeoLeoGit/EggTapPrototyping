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
    Animator animator;

    //Test
    public float tapRate;
    float currentTapRate = 0f;
    bool isAllowedToMove = false;
    public Vector3 target;
    private void Awake()
    {
        tapRate = UnityEngine.Random.Range(0.05f, 0.1f);
        StartCoroutine(AutoMoveTest());
    }


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    IEnumerator AutoMoveTest()
    {
        yield return new WaitForSeconds(3f);
        target = transform.position;
        isAllowedToMove = true;
    }
    private void FixedUpdate()
    {
        if (!isAllowedToMove)
            return;
        if (currentTapRate <= 0)
        {
            Move();
            currentTapRate = tapRate;
        }
        else
        {
            animator.SetBool("running", true);
            currentTapRate -= Time.fixedDeltaTime;
        }
    }
    public void Move()
    {   
        target += Vector3.right * runLengtth;

        transform.DOMove(target, duration);
        clickCount++;
        animator.SetBool("running", true);
        

        //Test
        if (clickCount == numberOfClickTillRankUpdate)
        {
            onEnoughClickCount(playerInput.playerId, goalXPosition - transform.position.x);
            clickCount = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(target, 0.2f);//ve diem target
    }

}
