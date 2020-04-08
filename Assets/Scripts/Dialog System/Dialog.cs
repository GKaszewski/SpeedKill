using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog {
    public string characterName;
    public Color characterColor;
    [TextArea(3, 10)]
    public string[] sentences;
    public float duration;

    public Dialog(string name, string[] text, Color color) {
        characterName = name;
        characterColor = color;
        sentences = text;
    }

    public Dialog() {
        characterName = "";
        characterColor = Color.white;
        sentences = new string[] { };
        duration = 0.0f;
    }

    public void SetCharacterName(string name) => characterName = name;
    public void SetCharacterColor(Color color) => characterColor = color;
    public void SetText(string[] text) => sentences = text;
    public void SetDuration(float duration) => this.duration = duration;
}
