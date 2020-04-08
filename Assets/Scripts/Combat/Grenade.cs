using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
    private AudioSource src;

    public AudioClip explosionSound;
    public float range = 10.0f;
    public LayerMask targetsLayer;
    private void Start() {
        src = GetComponent<AudioSource>();
        StartCoroutine(DestroyAfterDelay());
    }

    private void Explode() {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 150.0f);
        var targets = Physics.OverlapSphere(transform.position, range, targetsLayer);
        foreach (var target in targets) {
            if (target.CompareTag("Bot")) target.GetComponent<BotAI>().Kill();
        }
        var particles = ObjectPooler.SharedInstance.GetPooledObject(2);
        particles.transform.position = transform.position;
        particles.SetActive(true);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Explode();
    }

    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(5f);
        Explode();
    }
}
