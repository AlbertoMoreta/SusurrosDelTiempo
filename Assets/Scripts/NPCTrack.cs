using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrack : MonoBehaviour {
    public GameObject playerCamera;

    public bool trackXAxis;
    public bool trackYAxis;
    public bool trackZAxis;

    void Start() { }

    void Update() {
        if(playerCamera){

            var x = trackXAxis ? playerCamera.transform.position.x : transform.position.x;
            var y = trackYAxis ? playerCamera.transform.position.y : transform.position.y;
            var z = trackZAxis ? playerCamera.transform.position.z : transform.position.z;
            transform.LookAt(new Vector3(x, y, z));

            // var direction = new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z) - transform.position;
            // var rotation = Quaternion.LookRotation(direction);
            // rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, Mathf.Clamp(rotation.eulerAngles.y, -180, 180), rotation.eulerAngles.z);
            // transform.rotation = rotation;
        }
    }
}