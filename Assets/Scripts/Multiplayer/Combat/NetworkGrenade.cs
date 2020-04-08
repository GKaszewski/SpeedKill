using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGrenade : MonoBehaviourPunCallbacks {
    private AudioSource src;
    public AudioClip explosionSound;
    public float range = 10.0f;
    public PhotonView view;
    public LayerMask targetsLayer;

    private void Start() {
        src = GetComponent<AudioSource>();
        DestroyAfterDelay();
    }
    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(5f);
        Explode();
        PhotonNetwork.Destroy(gameObject);
    }

    private void Explode() {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        view.RPC("RpcPlayGunShot", RpcTarget.Others, 2);
        var targets = Physics.OverlapSphere(transform.position, range, targetsLayer);
        foreach (var target in targets) {
            if (target.CompareTag("Player")) {
                view.RPC("RpcOnPlayerHit", RpcTarget.AllViaServer, transform.position);
                target.GetComponent<PhotonView>().RPC("RpcKill", RpcTarget.All);
                NetworkGameManager.instance.AddKills(view.Owner);
                NetworkGameManager.instance.AddDeaths(target.GetComponent<PhotonView>().Owner);
            }
            else view.RPC("RpcOnHit", RpcTarget.AllViaServer, transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Explode();
        PhotonNetwork.Destroy(gameObject);
    }
}
