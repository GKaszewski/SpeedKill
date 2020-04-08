using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour {
    private void OnEnable() {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide() {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
