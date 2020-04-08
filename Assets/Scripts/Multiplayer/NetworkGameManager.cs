using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class NetworkGameManager : MonoBehaviour {
    public static NetworkGameManager instance = null;
    public float respawnTime = 5.0f;

    public Transform[] spawnpoints;
    public EffectsManager effects;

    public Dictionary<string, ProfileData> players = new Dictionary<string, ProfileData>();
    public GameObject player;

    private void Awake() {
        if (instance == null) instance = this;
        CreatePlayerConnection();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            foreach (var item in players.Values)
            {
                Debug.LogError(item.nick);
            }
        }
    }

    public void Respawn() {
        StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine() {
        yield return new WaitForSeconds(respawnTime);
        player = PhotonNetwork.Instantiate("NetworkPlayer", spawnpoints[Random.Range(0, spawnpoints.Length)].position, Quaternion.identity);
        player.name = PhotonNetwork.LocalPlayer.NickName;
        var health = player.GetComponent<NetworkHealth>();
        health.RespawnEvent += Respawn;

    }

    public void CreatePlayerConnection() {
        PhotonNetwork.Instantiate("NetworkPlayerConnection", transform.position, Quaternion.identity);
        foreach (var item in players.Values) {
            Debug.LogError(item.nick);
        }
    }

    public ProfileData GetPlayer(Player player) {
        var profile = new ProfileData();
        profile.nick = player.NickName;
        profile.kills = (int)player.CustomProperties["kills"];
        profile.deaths = (int)player.CustomProperties["deaths"];
        profile.killDeathRatio = (float)player.CustomProperties["kdr"];
        return profile;
    }

    public void AddDeaths(Player player) {
        int current = GetPlayer(player).deaths;
        current++;
        var profileTable = new Hashtable();
        profileTable.Add("deaths", current);
        player.SetCustomProperties(profileTable);
        SetKDR(player);
        Debug.LogError($"{player.NickName} has {current} deaths!");
    }

    private void SetKDR(Player player) {
        var profileTable = new Hashtable();
        var kdr = GetPlayer(player).CalculateKDR();
        profileTable.Add("kdr", kdr);
        player.SetCustomProperties(profileTable);
        Debug.Log($"{player.NickName} has {kdr} KDR!");
    }

    public void AddKills(Player player) {
        int current = GetPlayer(player).kills;
        current++;
        var profileTable = new Hashtable();
        profileTable.Add("kills", current);
        player.SetCustomProperties(profileTable);
        SetKDR(player);
        Debug.LogError($"{player.NickName} has {current} kills!");
    }
}
