using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DialogManager : MonoBehaviour {
    private Dictionary<string, DialogSequence> sequences = new Dictionary<string, DialogSequence>();

    public GameObject dialogDialog;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI textArea;

    private void Start() {
        //Load all sequences

        GameManager.instance.eventsManager.OnStartDialogSequence += Play;
    }

    private void OnDisable() {
        GameManager.instance.eventsManager.OnStartDialogSequence -= Play;
    }
    private void Play(string sequenceName) {
        if (!sequences.ContainsKey(sequenceName)) return;
        var currentSequence = sequences[sequenceName];
        for (int i = 0; i < currentSequence.dialogsCount; i++) {
            currentSequence.Next();
        }

        RemoveSequence(sequenceName);
    }

    public void AddSequence(string sequenceName, DialogSequence sequence) {
        if (sequences.ContainsKey(sequenceName)) return;
        
        sequences.Add(sequenceName, sequence);
    }

    public void RemoveSequence(string sequenceName) {
        if (sequences.ContainsKey(sequenceName)) return;
        sequences.Remove(sequenceName);
    }
}