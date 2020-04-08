using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {
    public string dialogID;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) GameManager.instance.eventsManager.StartDialogSequence(dialogID);
    }
}
