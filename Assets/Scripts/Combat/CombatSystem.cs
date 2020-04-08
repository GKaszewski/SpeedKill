using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour {
    public Gun gun;
    public Transform camT;
    public GameObject cameraForShake;
    public LayerMask targetLayers;
    public float shakeAmount;
    private void Start() {
        GameManager.instance.eventsManager.OnGunShoot += Shoot;
    }

    private void OnEnable() {
        GameManager.instance.eventsManager.OnGunShoot -= Shoot;
    }

    private void OnDisable() {
        GameManager.instance.eventsManager.OnGunShoot += Shoot;
    }

    private void Update() {
        if (Input.GetMouseButton(0) && !gun.isReloading) {
            //GameManager.instance.eventsManager.ShootGun();
            Shoot();
        }

        Debug.DrawRay(camT.position, camT.forward * 100f, Color.cyan);
    }

    private void Shoot() {
        var ray = new Ray(gameObject.transform.position, camT.forward);
        RaycastHit hit;
        LeanTween.rotateLocal(cameraForShake, Vector3.one * shakeAmount, 0.1f).setEaseShake();
        gun.Shoot();
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
