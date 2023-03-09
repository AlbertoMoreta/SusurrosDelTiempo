using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public LevelLoader LevelLoader;

    public void OnClickPlay() {
        LevelLoader.FadeAndLoadScene("Gameplay");
    }

    public void OnClickExit() {
        LevelLoader.FadeAnExit();
    }
}
