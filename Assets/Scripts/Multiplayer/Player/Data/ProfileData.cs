using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfileData {
    public string nick;
    public int kills = 0;
    public int deaths = 0;
    public float killDeathRatio = 0.0f;

    public float CalculateKDR() {
        if(deaths != 0)
            return killDeathRatio = kills / deaths;
        return 0;
    }
}
