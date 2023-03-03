using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrack : MonoBehaviour {
    public GameObject player;

    void Start() { }

    void Update() {
        if(player){
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y,  player.transform.position.z));
        }
    }
}
