using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
public class NetworkHealth : MonoBehaviourPunCallbacks {

    public delegate void Respawn();
    public event Respawn RespawnEvent;
    public AudioClip hitSound;
    private void Start() {
    }

    [PunRPC]
    public void RpcKill() {
        if (photonView.IsMine) {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            //id.owner.profile.deaths++;
            //id.owner.profile.CalculateKDR();
            RespawnEvent?.Invoke();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
