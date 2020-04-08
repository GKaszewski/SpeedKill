using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenu;

    private bool isPaused = false;

    private void Start() {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        pauseMenu.SetActive(isPaused);
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1.0f;
                isPaused = false;
            }
            else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = .0f;
                isPaused = true;
            }
        }
    }

    public void Leave() {  
        SceneManager.LoadScene("Menu");
    }
}
