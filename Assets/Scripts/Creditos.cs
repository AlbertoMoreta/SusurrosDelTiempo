using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creditos : MonoBehaviour {
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Awake() {
        var audioPath = "Sounds/Dialogs/1_creditos";
        var audioClip = Resources.Load<AudioClip>(audioPath);
        audioSource.PlayOneShot(audioClip);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
