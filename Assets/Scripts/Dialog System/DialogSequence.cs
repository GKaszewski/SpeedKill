using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogSequence : MonoBehaviour {
    public Queue<Dialog> dialogs = new Queue<Dialog>();
    private Queue<string> currentSentences = new Queue<string>();

    private Dialog currentDialog;
    private GameObject dialogDialog;
    private TextMeshProUGUI characterName;
    private TextMeshProUGUI textArea;

    public float popupTime = 0.5f;
    public Vector3 originalScale ;

    public int dialogsCount;

    private void Awake() {
        dialogsCount = dialogs.Count;

        //dialogDialog = GameManager.instance.dialogManager.dialogDialog;
        //characterName = GameManager.instance.dialogManager.characterName;
        //textArea = GameManager.instance.dialogManager.textArea;

        dialogDialog.transform.localScale = originalScale;
        dialogDialog.transform.localScale = Vector3.zero;
    }

    public void Next() {
        if (dialogs.Count == 0) {
            CloseDialog();
            return;
        }

        currentDialog = dialogs.Dequeue();
        currentSentences.Clear();
        foreach (var sentence in currentDialog.sentences) currentSentences.Enqueue(sentence);
        SetDialogData();
        ShowDialog();
        StartCoroutine(WaitToEnd(currentDialog.duration));
        CloseDialog();
    }

    private IEnumerator WaitToEnd(float t) {
        yield return new WaitForSeconds(t);
    }

    private void SetDialogData() {
        characterName.text = currentDialog.characterName;
        characterName.color = currentDialog.characterColor;
        if (currentSentences.Count == 0) {
            return;
        }
        textArea.text = currentSentences.Dequeue();
    }

    private void ShowDialog() {
        LeanTween.scale(dialogDialog, originalScale, popupTime);
    }

    private void CloseDialog() {
        LeanTween.scale(dialogDialog, Vector3.zero, popupTime).setOnComplete(ResetDialog);
    }

    private void ResetDialog() {
        currentDialog = null;
        characterName.text = "";
        characterName.color = Color.white;
        textArea.text = "";
    }
}