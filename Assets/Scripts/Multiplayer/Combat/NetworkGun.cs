using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkGun : MonoBehaviourPunCallbacks {
    public float reloadTime;
    public Transform barrel;

    public GameObject gun;

    public bool isReloading = false;

    public void Shoot() {
        photonView.RPC("RpcSpawnParticles", RpcTarget.AllViaServer, barrel.position);
        LeanTween.rotateAroundLocal(gun, Vector3.right, 360f, reloadTime - 0.15f).setEase(LeanTweenType.easeSpring);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
    [PunRPC]
    private void RpcSpawnParticles(Vector3 position) {
        if (!photonView.IsMine) return;
        var particles = Instantiate(NetworkGameManager.instance.effects.muzzleflash, position, Quaternion.identity);
        Destroy(particles, 1.0f);
    }
}
