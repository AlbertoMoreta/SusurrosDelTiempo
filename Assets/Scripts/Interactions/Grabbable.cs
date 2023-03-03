using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MODE {
    RELEASED = 0,
    GRAB = 1,
    GRABBED = 2,
}

public class Grabbable : MonoBehaviour, Interactable {

    public GameObject grabber;
    public float force = 5f;
    private Transform _originalParent;
    private MODE _currentMode = MODE.RELEASED;
    private float _elapsedTime = 0f;
    private float _desiredDuration = 3f;
    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start() {
        _originalParent = transform.parent;
         _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if(_currentMode == MODE.GRAB){
            Grab(); 
        }
    }

    public void Interact() {
        Debug.Log("Mode: " + _currentMode);
        switch(_currentMode){
            case MODE.GRABBED:
            case MODE.GRAB: Release(); break;
            case MODE.RELEASED: _currentMode = MODE.GRAB; break;
        }
    }

    void Grab() {
        Debug.Log("Grab");
        this.transform.parent = grabber.transform;
        _rigidbody.isKinematic = true;

        _elapsedTime += Time.deltaTime;
        float percentageComplete = _elapsedTime / _desiredDuration;

        var playerManager = PlayerManager.Instance;
        var playerCamera = playerManager.GetPlayerCamera();
        var endPosition = playerCamera.transform.position + playerCamera.transform.forward * 2;
        var endRotation = playerCamera.transform.rotation * Quaternion.Euler(-90,-90,90);

        this.transform.position = Vector3.Slerp(this.transform.position, endPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, endRotation, percentageComplete);

        if(this.transform.position == endPosition &&
            this.transform.rotation == endRotation){
           _currentMode = MODE.GRABBED; 
        }

    }

    void Release() {
        Debug.Log("Release");
        this.transform.parent = _originalParent;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(grabber.transform.forward * force, ForceMode.Impulse);
        _currentMode = MODE.RELEASED;
    }
}
