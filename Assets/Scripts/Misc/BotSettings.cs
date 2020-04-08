using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotSettings : MonoBehaviour {
    public int bots;
    public TMP_InputField botField;
    public TextMeshProUGUI warning;

    private void Awake() {
        DontDestroyOnLoad(this);        
    }

    public void SetBots() {
        bots = Convert.ToInt32(botField.text);
        if (bots == 0) {
            warning.SetText("WRONG NUMBER!");
        } else {
            warning.SetText("");
        }
    }
}
