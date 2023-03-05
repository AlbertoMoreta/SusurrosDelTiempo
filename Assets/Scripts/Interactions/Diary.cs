using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Diary : MonoBehaviour {


    public Image icon;
    public Image fullScreen;
    public Text hints;



    // Start is called before the first frame update
    void Start()
    {
        fullScreen.enabled = false;
        icon.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var playerManager = PlayerManager.Instance;
        if (Input.GetKeyDown ("k"))
        {
            fullScreen.enabled = !fullScreen.enabled;
            icon.enabled = !fullScreen.enabled;
        }
        if (fullScreen.enabled == true)
        {
            hints.enabled = true;
            playerManager.SetFirstPersonModeActive(false);
        } else
        {
            hints.enabled = false;
            playerManager.SetFirstPersonModeActive(true);
        }
    }
}
