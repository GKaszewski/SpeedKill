using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBots : MonoBehaviour {
    private int lastIndex;
    
    public int bots = 4;
    public GameObject botPrefab;
    private void Start() {
        for (int i = 0; i < bots; i++) {
            Spawn();
        }
    }

    public void Spawn() {
        var index = Random.Range(0, GameManager.instance.spawnpoints.Length);
        while (index == lastIndex)
            index = Random.Range(0, GameManager.instance.spawnpoints.Length);
        var position = GameManager.instance.spawnpoints[index].position;
        lastIndex = index;
        Instantiate(botPrefab, position, Quaternion.identity);
    }

    public IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.respawnTime);
        Spawn();
    }
}
