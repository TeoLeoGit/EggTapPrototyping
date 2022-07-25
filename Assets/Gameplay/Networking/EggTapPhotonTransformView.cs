using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using DG.Tweening;

public class EggTapPhotonTransformView : MonoBehaviourPun, IPunObservable
{
    /*public Controller controller;*/ //EggTap: For debugging.
    public bool isAt1stPosition = false;
    public Vector3 gapDistance; //EggTap: Distance between real position on other clients and local client. Assign when player is spawned.
    private float m_Distance;
    private float m_Angle;

    private Vector3 m_Direction;
    private Vector3 m_NetworkPosition;
    private Vector3 m_StoredPosition;

    private Quaternion m_NetworkRotation;

    public bool m_SynchronizePosition = true;

    [Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
    public bool m_UseLocal;

    bool m_firstTake = false;

    public void Awake()
    {
        m_StoredPosition = transform.localPosition;
        /*m_StoredPosition = controller.target.localPosition;*/

        m_NetworkPosition = Vector3.zero;

        m_NetworkRotation = Quaternion.identity;
    }

    private void Start()
    {
        if(photonView.IsMine)
        {
            m_StoredPosition = transform.localPosition + gapDistance; //EggTap: Real position on other clients.
            /*m_StoredPosition = controller.target.localPosition + gapDistance;*/ //EggTap: Real position on other clients.

        }
    }

    private void Reset()
    {
        // Only default to true with new instances. useLocal will remain false for old projects that are updating PUN.
        m_UseLocal = true;
    }

    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        var tr = transform;
        /*var tr = controller.target;*/

        if (!this.photonView.IsMine)
        {
            if (m_UseLocal)

            {
                tr.localPosition = Vector3.MoveTowards(tr.localPosition, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * PhotonNetwork.SerializationRate);
               /* transform.DOMove(controller.target.position, 1.5f).SetEase(Ease.OutCubic);*/
                tr.localRotation = Quaternion.RotateTowards(tr.localRotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * PhotonNetwork.SerializationRate);
            }
            else
            {
                /*transform.DOMove(controller.target.position, 1.5f).SetEase(Ease.OutCubic);*/
                tr.position = Vector3.MoveTowards(tr.position, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * PhotonNetwork.SerializationRate);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * PhotonNetwork.SerializationRate);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        var tr = transform;
        /*var tr = controller.target;*/

        // Write
        if (stream.IsWriting)
        {
            if (this.m_SynchronizePosition)
            {
                //EggTap: Modify data before sending to match the exact position.
                if (m_UseLocal)
                {
                    this.m_Direction = tr.localPosition + gapDistance - this.m_StoredPosition;
                    this.m_StoredPosition = tr.localPosition + gapDistance;
                    /*stream.SendNext(tr.localPosition + gapDistance);*/
                    stream.SendNext(tr.localPosition + gapDistance);
                    stream.SendNext(this.m_Direction);
                }
                else
                {
                    this.m_Direction = tr.position + gapDistance - this.m_StoredPosition;
                    this.m_StoredPosition = tr.position + gapDistance;
                    /*stream.SendNext(tr.position + gapDistance);*/
                    stream.SendNext(tr.position + gapDistance);
                    stream.SendNext(this.m_Direction);
                }
            }

        }
        // Read
        else
        {
            if (this.m_SynchronizePosition)
            {
                this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                this.m_Direction = (Vector3)stream.ReceiveNext();

                //EggTap: clone got swapped on client must adding gap to position.
                if (isAt1stPosition)
                {
                    this.m_NetworkPosition += gapDistance;
                    this.m_Direction += gapDistance;
                }

                if (m_firstTake)
                {
                    if (m_UseLocal)
                        tr.localPosition = this.m_NetworkPosition;
                    else
                        tr.position = this.m_NetworkPosition;

                    this.m_Distance = 0f;
                }
                else
                {
                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    this.m_NetworkPosition += this.m_Direction * lag;
                    if (m_UseLocal)
                    {
                        this.m_Distance = Vector3.Distance(tr.localPosition, this.m_NetworkPosition);
                    }
                    else
                    {
                        this.m_Distance = Vector3.Distance(tr.position, this.m_NetworkPosition);
                    }
                }

            }

        }
    }
}
