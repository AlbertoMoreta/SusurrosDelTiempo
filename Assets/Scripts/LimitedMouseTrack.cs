using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedMouseTrack : MonoBehaviour {

    public float mouseSensitivity = 100f;
    public Transform player;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (canMove) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            yRotation += mouseX;
            xRotation = Mathf.Clamp(xRotation, -10f, 10f);
            yRotation = Mathf.Clamp(yRotation, -10f, 10f);

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }
}
