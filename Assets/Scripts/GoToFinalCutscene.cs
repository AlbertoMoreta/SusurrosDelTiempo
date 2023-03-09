using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToFinalCutscene : MonoBehaviour
{
    public LevelLoader LevelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            LevelLoader.FadeAndLoadScene("CutScene_final");
        }
    }
}
