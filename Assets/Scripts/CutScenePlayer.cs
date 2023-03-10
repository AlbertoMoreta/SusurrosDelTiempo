using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutScenePlayer : MonoBehaviour
{

    public GameObject cabo1;
    public GameObject cabo2;
    public GameObject blackout;
    public LevelLoader LevelLoader;
    public GameObject message;

    void Start()
    {
        Debug.Log("start");
        Cursor.visible = false;
        StartCoroutine(StartFirstDialog());
        StartCoroutine(StartSecondDialog());
        StartCoroutine(Blackout());
    }

    IEnumerator StartFirstDialog() {
        yield return new WaitForSeconds(0.5f);
        cabo1.SetActive(true);
        cabo2.SetActive(false);
        DialogManager.Instance.StartDialog("1_Final_GC_y_falangista");
    }

    IEnumerator StartSecondDialog() {
        yield return new WaitForSeconds(32f);
        cabo1.SetActive(false);
        cabo2.SetActive(true);
        DialogManager.Instance.StartDialog("2_Final_GC_y_falangista");
    }


    IEnumerator Blackout() {
        yield return new WaitForSeconds(43f);
        blackout.SetActive(true);
        yield return new WaitForSeconds(3f);
        showMessageAndEnd();
    }

    void showMessageAndEnd()
    {
        message.SetActive(true);
        IEnumerator WaitForMessage()
        {
            yield return new WaitForSeconds(30f);// Wait a bit
            SceneManager.LoadScene(3);
        }
        StartCoroutine(WaitForMessage());
    }
}
