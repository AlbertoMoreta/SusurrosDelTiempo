using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void OnClickPlay() {
        SceneManager.LoadScene(1);
    }

    public void OnClickExit() {
        Application.Quit();
    }
}
