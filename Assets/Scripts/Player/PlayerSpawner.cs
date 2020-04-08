using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public Health player;
    public IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.respawnTime);
        player.Spawn();
    }
}
