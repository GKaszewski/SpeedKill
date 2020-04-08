using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Weapon { Shotgun, GrenadeLauncher }
public class NetworkCombatSystem : MonoBehaviourPunCallbacks {
    public NetworkGun gun;
    public NetworkGrenadeLauncher grenadeLauncher;
    public Transform camT;
    public GameObject cameraForShake;
    public LayerMask targetLayers;
    public float shakeAmount;
    public Weapon currentWeapon = Weapon.Shotgun;

    public GameObject gunPrefab;
    public GameObject grenadeLauncherPrefab;

    private AudioSource src;
    public AudioClip gunshot;
    public AudioClip grenadeLauncherShotSound;
    public AudioClip explosionSound;

    private void Start() {
        src = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        if (!photonView.IsMine) return;
        switch (currentWeapon) {
            case Weapon.Shotgun:
                grenadeLauncherPrefab.SetActive(false);
                gunPrefab.SetActive(true);
                if (Input.GetMouseButton(0) && !gun.isReloading) {
                    Shoot();
                    //photonView.RPC("RpcPlayGunShot", RpcTarget.Others, 0);
                }
                break;
            case Weapon.GrenadeLauncher:
                gunPrefab.SetActive(false);
                grenadeLauncherPrefab.SetActive(true);
                grenadeLauncher.view = photonView;
                if (Input.GetMouseButton(0) && !grenadeLauncher.isReloading) { 
                    grenadeLauncher.Shoot();
                    //photonView.RPC("RpcPlayGunShot", RpcTarget.Others, 1);
                }
                break;
            default:
                break;
        }

        Debug.DrawRay(camT.position, camT.forward * 100f, Color.cyan);
    }

    [PunRPC]
    private void RpcOnPlayerHit(Vector3 position) {
        var particles = Instantiate(NetworkGameManager.instance.effects.poofParticles, position, Quaternion.identity);
        Destroy(particles, 1.0f);
    }

    [PunRPC]
    private void RpcOnHit(Vector3 position) {
        var particles = Instantiate(NetworkGameManager.instance.effects.boomParticles, position, Quaternion.identity);
        Destroy(particles, 1.0f);
    }

    [PunRPC]
    private void RpcPlayGunShot(int id) {
        if (photonView.IsMine) return;
        switch (id) {
            case 0:
                src.PlayOneShot(gunshot);
                break;
            case 1:
                src.PlayOneShot(grenadeLauncherShotSound);
                break;
            case 2:
                src.PlayOneShot(explosionSound);
                break;
            default:
                break;
        }
    }
    private void Shoot() {
        if (!photonView.IsMine) return;
        var ray = new Ray(camT.position, camT.forward);
        RaycastHit hit;
        LeanTween.rotateLocal(cameraForShake, Vector3.one * shakeAmount, 0.1f).setEaseShake();
        src.PlayOneShot(gunshot);
        gun.Shoot();
        photonView.RPC("AnimateThirdPersonGun", RpcTarget.AllBufferedViaServer, 1.35f);
        if (Physics.Raycast(ray, out hit, 100f, targetLayers)) {
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Player")) {
                    photonView.RPC("RpcOnPlayerHit", RpcTarget.AllViaServer, hit.point);
                    hit.collider.GetComponent<PhotonView>().RPC("RpcKill", RpcTarget.All);
                    NetworkGameManager.instance.AddKills(PhotonNetwork.LocalPlayer);
                    NetworkGameManager.instance.AddDeaths(hit.collider.GetComponent<PhotonView>().Owner);
                }
                else {
                    photonView.RPC("RpcOnHit", RpcTarget.AllViaServer, hit.point);
                }
            }
        }
    }
}
