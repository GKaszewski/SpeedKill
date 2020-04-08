using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour {

    private bool fullscreen = false;
    public Toggle vsyncToogle;
    public void ExitGame() {
        Application.Quit();
    }

    public void SetVsync() {
        if(vsyncToogle.isOn) QualitySettings.vSyncCount = 1;
        else  QualitySettings.vSyncCount = 0;
    }

    public void FullHD() {
        Screen.SetResolution(1920, 1080, fullscreen);
    }

    public void HD() {
        Screen.SetResolution(1280, 720, fullscreen);
    }

    public void ScreenResolution2() {
        Screen.SetResolution(1600, 900, fullscreen);
    }

    public void SmallWindow() {
        Screen.SetResolution(800, 600, fullscreen);
    }

    public void Fullscreen() {
        fullscreen = true;
        FullHD();
    }

    public void Windowed() {
        fullscreen = false;
    }

    public void PlayGame() {
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
