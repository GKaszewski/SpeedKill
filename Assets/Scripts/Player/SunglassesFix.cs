using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunglassesFix : MonoBehaviour {
    private void Start() {
        //Later add in multiplayer if localPlayer than hide it :D
        gameObject.SetActive(false);
    }
}
