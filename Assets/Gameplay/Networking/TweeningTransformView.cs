using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class TweeningTransformView : MonoBehaviourPun, IPunObservable
{
    public bool isAt1stPosition = false;
    public Vector3 gapDistance; //EggTap: Distance between real position on other clients and local client. Assign when player is spawned.

    [SerializeField] private Transform targetTransform;
    [SerializeField] float synchRate;
    [SerializeField] float duration;
    float currentRate = 0;
    private Vector3 targetPosition;

    public bool m_SynchronizePosition = true;

    [Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
    public bool m_UseLocal;


    private void Start()
    {
        if (photonView.IsMine)
        {
            targetPosition = targetPosition + gapDistance; //EggTap: Real position on other clients.

        }
       
    }

    private void Reset()
    {
        // Only default to true with new instances. useLocal will remain false for old projects that are updating PUN.
        m_UseLocal = true;
    }

    public void Update()
    {
        /*var tr = controller.target;*/
        currentRate -= Time.deltaTime;
        if (!this.photonView.IsMine)
        {
            //Will change this 0.3f magical number later I promise.
            if (currentRate <= 0)
            {
                transform.DOMove(targetPosition, duration).SetEase(Ease.OutCubic);
                currentRate = synchRate;
            }
           
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        // Write
        if (stream.IsWriting)
        {
            if (this.m_SynchronizePosition)
            {
                stream.SendNext(targetTransform.position + gapDistance);
            }

        }
        // Read
        else
        {
            if (this.m_SynchronizePosition)
            {
                this.targetPosition = (Vector3)stream.ReceiveNext();

                //EggTap: clone got swapped on client must adding gap to position.
                if (isAt1stPosition)
                {
                    this.targetPosition += gapDistance;
                }

            }

        }
    }
}
