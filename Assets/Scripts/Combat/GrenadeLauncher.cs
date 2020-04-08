using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour {
    private AudioSource src;

    public AudioClip grenadeLauncherSound;
    public Transform barrel;
    public GameObject cameraForShake;
    public GameObject grenadePrefab;
    public float force = 1500.0f;
    public float shakeAmount = 3.5f;
    public float reloadTime;
    public bool isReloading = false;

    private void Start() {
        src = GetComponent<AudioSource>();
    }

    public void Shoot() {
        src.PlayOneShot(grenadeLauncherSound);
        var grenade = Instantiate(grenadePrefab, barrel.position, Quaternion.identity);
        var rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * force * Time.fixedDeltaTime, ForceMode.Impulse);
        //CameraShaker.Instance.ShakeOnce(shakeAmount, 14f, 0.1f, 1f);
        var particles = ObjectPooler.SharedInstance.GetPooledObject(1);
        particles.transform.position = barrel.position;
        particles.SetActive(true);
        LeanTween.rotateAroundLocal(gameObject, -Vector3.right, 360f, reloadTime - 0.15f).setEase(LeanTweenType.easeSpring);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
