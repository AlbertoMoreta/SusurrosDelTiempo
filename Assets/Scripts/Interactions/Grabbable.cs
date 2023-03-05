using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MODE {
    RELEASED = 0,
    GRAB = 1,
}

public class Grabbable : MonoBehaviour, Interactable {

    public GameObject grabber;
    public float force = 5f;
    private Transform _originalParent;
    private MODE _currentMode = MODE.RELEASED;
    private float _elapsedTime = 0f;
    private float _desiredDuration = 1f;
    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start() {
        _originalParent = transform.parent;
         _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if(_currentMode != MODE.RELEASED){
            Grab(); 

            if (Input.GetMouseButtonDown(1)) {
                Throw();
            }
        }
    }

    public void Interact() {
        Debug.Log("Mode: " + _currentMode);
        _elapsedTime = 0;
        switch(_currentMode){
            case MODE.GRAB: Release(); break;
            case MODE.RELEASED: _currentMode = MODE.GRAB; break;
        }
    }

    void Grab() {
        this.transform.parent = grabber.transform;
        
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        var playerManager = PlayerManager.Instance;
        var playerCamera = playerManager.GetPlayerCamera();
        var endPosition = playerCamera.transform.position + playerCamera.transform.forward * 2;

        _elapsedTime += Time.deltaTime;
        float percentageComplete = _elapsedTime / _desiredDuration;

        var desiredPosition = Vector3.Slerp(this.transform.position, endPosition, percentageComplete);

        _rigidbody.MovePosition(desiredPosition);

    }

    void Release() {
        Debug.Log("Release");
        _rigidbody.velocity = grabber.GetComponentInParent<CharacterController>().velocity;
        this.transform.parent = _originalParent;
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _currentMode = MODE.RELEASED;
    }

    void Throw() {
        Debug.Log("Throw");
        Release();
        _rigidbody.AddForce(grabber.transform.forward * force, ForceMode.Impulse);
    }
}
