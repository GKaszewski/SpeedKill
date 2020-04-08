using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkPlayerCamera : MonoBehaviourPunCallbacks {
    public GameObject cameras;
    public GameObject player;
    public GameObject thirdPersonGun;
    public GameObject grenadeLaucher;
    public GameObject sunglasses;
    public GameObject ui;

    private NetworkCombatSystem combatSystem;
    private void Start() {
        combatSystem = GetComponent<NetworkCombatSystem>();
        cameras.SetActive(photonView.IsMine);
        thirdPersonGun.SetActive(!photonView.IsMine);
        grenadeLaucher.SetActive(false);
        sunglasses.SetActive(!photonView.IsMine);
        ui.SetActive(photonView.IsMine);

        if (!photonView.IsMine) player.layer = LayerMask.NameToLayer("Player");
        else player.layer = LayerMask.NameToLayer("LocalPlayer");
    }

    private void Update() {
        switch (combatSystem.currentWeapon) {
            case Weapon.Shotgun:
                thirdPersonGun.SetActive(!photonView.IsMine);
                grenadeLaucher.SetActive(false);
                break;
            case Weapon.GrenadeLauncher:
                grenadeLaucher.SetActive(!photonView.IsMine);
                thirdPersonGun.SetActive(false);
                break;
            default:
                break;
        }
    }
}
