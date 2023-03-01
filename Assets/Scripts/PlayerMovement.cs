using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject camera;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 12f;
    public DialogManager dm;

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = 9.82f;
    private float groundDistance = 0.2f;
    private bool isGrounded;
    private bool canMove = true;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        dm.SendMessage("StartDialog", "1_welcome");
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

         // Movement
        if (canMove) {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            // Gravity
            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }
}
