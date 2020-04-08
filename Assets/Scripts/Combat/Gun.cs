using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun : MonoBehaviour {
    public float reloadTime;
    public Transform barrel;

    public bool isReloading = false;

    public void Shoot() {
        if (ObjectPooler.SharedInstance.GetPooledObject(1) == null) return;
        var particles = ObjectPooler.SharedInstance.GetPooledObject(1);
        particles.transform.position = barrel.position;
        particles.SetActive(true);
        LeanTween.rotateAroundLocal(gameObject, Vector3.right, 360f, reloadTime-0.15f).setEase(LeanTweenType.easeSpring);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
