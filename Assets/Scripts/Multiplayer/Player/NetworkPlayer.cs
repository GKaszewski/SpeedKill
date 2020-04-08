using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class NetworkPlayer : MonoBehaviourPunCallbacks {
    public int id;
    public ProfileData profile;
    public GameObject playerUnit;

    private void Start() {
        if (photonView.IsMine) { 
            profile = NetworkManager.profileData;
            id = Random.Range(0, int.MaxValue);
            SyncProfile(profile.kills, profile.deaths, profile.killDeathRatio);
            transform.name = PhotonNetwork.LocalPlayer.NickName;
            Spawn();
        }
    }

    private void OnDestroy() {
    }

    public void Spawn() {
        NetworkGameManager.instance.Respawn();
    }

    [PunRPC]
    public IEnumerator Respawn() {
        yield return new WaitForSeconds(NetworkGameManager.instance.respawnTime);
        Spawn();
    }

    //[PunRPC]
    //private void SetID(int id) {
    //    this.id = id;
    //}

    private void SyncProfile(int kills, int deaths, float kdr) {
        var profileTable = new Hashtable();
        profileTable.Add("kills", kills);
        profileTable.Add("deaths", deaths);
        profileTable.Add("kdr", kdr);
        PhotonNetwork.LocalPlayer.SetCustomProperties(profileTable);
        Debug.Log(PhotonNetwork.LocalPlayer.ToString());
        Debug.Log("PROFILE TABLE");
        foreach (var item in profileTable) {
            Debug.Log($"{item.Key.ToString()} : {item.Value.ToString()}");
        } 
    }
}
