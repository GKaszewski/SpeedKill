using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSingleplayer : MonoBehaviour {
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
            src.PlayOneShot(pickupSound);
            other.GetComponent<CombatSystem>().currentWeapon = Weapon.GrenadeLauncher;
            other.GetComponent<CombatSystem>().grenadeLauncher.isReloading = false;
        }
    }

    private void PickUpShotgun(Collider other) {
        if (other.CompareTag("Player")) {
            src.PlayOneShot(pickupSound);
            other.GetComponent<CombatSystem>().currentWeapon = Weapon.Shotgun;
            other.GetComponent<CombatSystem>().gun.isReloading = false;
        }
    }
}
