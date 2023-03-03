using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Diary : MonoBehaviour {


    public Image icon;
    public Image fullScreen;


    // Start is called before the first frame update
    void Start()
    {
        fullScreen.enabled = false;
        icon.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown ("k"))
        {
            fullScreen.enabled = !fullScreen.enabled;
            icon.enabled = !fullScreen.enabled;
        }
    }
}
