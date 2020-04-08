using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotatetor : MonoBehaviour {
    private void Update() {
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 2f, 5f * Time.deltaTime);
    }
}
