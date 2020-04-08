using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickupWeapon : MonoBehaviourPunCallbacks {
    private AudioSource src;
    
    public Weapon weapon;
    public AudioClip pickupSound;

    private void Start() {
        src = GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter(Collider other) {
        switch (weapon) {
            case Weapon.Shotgun:
                PickUpShotgun(other);
                break;
            case Weapon.GrenadeLauncher:
                PickUpGrenadeLauncher(other);
                break;
            default:
                break;
        }
    }

    private void PickUpGrenadeLauncher(Collider other) {
        if (other.CompareTag("Player")) {
            RpcPlayPickUpSound();
            other.GetComponent<NetworkCombatSystem>().currentWeapon = Weapon.GrenadeLauncher;
        }
    }

    private void PickUpShotgun(Collider other) {
        if (other.CompareTag("Player")) {
            RpcPlayPickUpSound();
            other.GetComponent<NetworkCombatSystem>().currentWeapon = Weapon.Shotgun;
        }
    }

    [PunRPC]
    private void RpcPlayPickUpSound() {
        src.PlayOneShot(pickupSound);
    }

}
