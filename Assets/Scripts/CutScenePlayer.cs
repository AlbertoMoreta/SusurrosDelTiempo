using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CutScenePlayer : MonoBehaviour
{

    public GameObject cabo1;
    public GameObject cabo2;
    public GameObject blackout;
    public GameObject message;

    public LevelLoader LevelLoader;

    // Start is called before the first frame update
    void Awake()
    {
        IEnumerator StartSecondDialog()
        {
            yield return new WaitForSeconds(30);// Wait a bit
            cabo1.SetActive(false);
            cabo2.SetActive(true);
            ShowSecondDialog();
        }
        //GameObject.Find("DialogTrigger").SetActive(true);
        StartCoroutine(StartSecondDialog());
    }

    private void ShowSecondDialog()
    {
        DialogManager.Instance.StartDialog("2_Final_GC_y_falangista");
        IEnumerator WaitForBlackout()
        {
            yield return new WaitForSeconds(9.2f);// Wait a bit
            Blackout();
        }
        StartCoroutine(WaitForBlackout());

    }


    void Blackout()
    {
        blackout.SetActive(true);
        IEnumerator WaitForMessage()
        {
            yield return new WaitForSeconds(3.2f);// Wait a bit
            showMessageAndEnd();
        }
        StartCoroutine(WaitForMessage());
    }

    void showMessageAndEnd()
    {
        message.SetActive(true);
        IEnumerator WaitForMessage()
        {
            yield return new WaitForSeconds(10f);// Wait a bit
            LevelLoader.FadeAndLoadScene("Credits");
        }
        StartCoroutine(WaitForMessage());
    }
}
