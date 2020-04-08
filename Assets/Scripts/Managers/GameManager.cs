using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public EventsManager eventsManager;
    public EffectsManager effects;

    public Transform[] spawnpoints;
    public SpawnBots botSpawner;
    public PlayerSpawner playerSpawner;
    public float respawnTime = 5f;
    private void OnEnable() {
        if (instance == null) instance = this;
        //eventsManager.OnBotSpawn += AddTargets;
    }
}
