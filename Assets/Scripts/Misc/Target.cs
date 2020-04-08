using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Target : MonoBehaviour {
    public int id;
    private void OnEnable() {
        id = Random.Range(0, int.MaxValue);
        gameObject.name = "Bot" + id;
    }
}
