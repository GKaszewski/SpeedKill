using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public GameObject playerPrefab;
    public void Kill() {
        GameManager.instance.playerSpawner.StartCoroutine("Respawn");
        Destroy(gameObject);
    }

    public void Spawn() {
        var position = GameManager.instance.spawnpoints[Random.Range(0, GameManager.instance.spawnpoints.Length)].position;
        Instantiate(playerPrefab, position, Quaternion.identity);
    }
}
