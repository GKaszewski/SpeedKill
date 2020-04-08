using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkGrenadeLauncher : MonoBehaviourPunCallbacks {
    private AudioSource src;
    public AudioClip grenadeLauncherSound;
    public float force = 1500.0f;
    public float reloadTime;
    public bool isReloading = false;
    public Transform barrel;
    public Transform camT;
    public PhotonView view;
    public GameObject grenadeLauncher;
    private void Start() {
        src = GetComponent<AudioSource>();
    }
    public void Shoot() {
        if (!photonView.IsMine) return;
        src.PlayOneShot(grenadeLauncherSound);
        var grenade = PhotonNetwork.Instantiate("Grenade", barrel.position, Quaternion.identity);
        grenade.GetComponent<NetworkGrenade>().view = view;
        var rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force * Time.fixedDeltaTime, ForceMode.Impulse);
        view.RPC("RpcSpawnGrenadeLauncherParticles", RpcTarget.AllViaServer, camT.position);
        LeanTween.rotateAroundLocal(grenadeLauncher, -Vector3.right, 360f, reloadTime - 0.15f).setEase(LeanTweenType.easeSpring);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    [PunRPC]
    private void RpcSpawnGrenadeLauncherParticles(Vector3 position) {
        var particles = Instantiate(NetworkGameManager.instance.effects.muzzleflash, position, Quaternion.identity);
        Destroy(particles, 1.0f);
    }
}
