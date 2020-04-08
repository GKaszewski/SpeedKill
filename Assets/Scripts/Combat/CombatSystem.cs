using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public class CombatSystem : MonoBehaviour {
    private AudioSource src;

    public Gun gun;
    public GrenadeLauncher grenadeLauncher;
    public Transform camT;
    public GameObject cameraForShake;
    public LayerMask targetLayers;
    public float shakeAmount;
    public Weapon currentWeapon;
    public AudioClip gunShot;

    private void Start() {
        src = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        switch (currentWeapon) {
            case Weapon.Shotgun:
                gun.gameObject.SetActive(true);
                grenadeLauncher.gameObject.SetActive(false);
                if (Input.GetMouseButton(0) && !gun.isReloading) {
                    Shoot();
                }
                break;
            case Weapon.GrenadeLauncher:
                gun.gameObject.SetActive(false);
                grenadeLauncher.gameObject.SetActive(true);
                if (Input.GetMouseButton(0) && !grenadeLauncher.isReloading)
                    grenadeLauncher.Shoot();
                break;
            default:
                break;
        }

       

        Debug.DrawRay(camT.position, camT.forward * 100f, Color.cyan);
    }

    private void Shoot() {
        var ray = new Ray(gameObject.transform.position, camT.forward);
        RaycastHit hit;
        //CameraShaker.Instance.ShakeOnce(shakeAmount, 10f, 0.1f,1f);
        gun.Shoot();
        src.PlayOneShot(gunShot);
        if(Physics.Raycast(ray, out hit, 100f, targetLayers)) {
            if (hit.collider != null) {
                Debug.Log("Hit: " + hit.collider.name);
                if(hit.collider.CompareTag("Bot")) {
                    var bot = hit.collider.GetComponent<BotAI>();
                    bot.Kill();
                    var particles = ObjectPooler.SharedInstance.GetPooledObject(2);
                    particles.transform.position = hit.point;
                    particles.SetActive(true);
                } else {
                    var particles = ObjectPooler.SharedInstance.GetPooledObject(0);
                    particles.transform.position = hit.point;
                    particles.SetActive(true);
                }
            }
        }
    }
}
