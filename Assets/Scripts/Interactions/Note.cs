
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NoteDirection {
    NONE = -1,
    TO_BACKGROUND = 0,
    TO_FOREGROUND = 1
}

public enum NoteFlip{
    FRONT = 0,
    BACK = 1
}

public class Note : MonoBehaviour, Interactable {

    public bool LockRotation = false;

    private float desiredDuration = 3f;
    private float elapsedTime;

    private NoteDirection _direction = NoteDirection.NONE;

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private Vector3 _startingScale;

    private NoteFlip _noteTurnet = NoteFlip.FRONT;

    // Start is called before the first frame update
    void Start() {
        _startingPosition = this.transform.position;
        _startingRotation = this.transform.rotation;
        _startingScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        switch(_direction) {
            case NoteDirection.TO_FOREGROUND: AnimateNoteToForeground(); break;
            case NoteDirection.TO_BACKGROUND: AnimateNoteToBackground(); break;
        }
    }


    public void Interact() {
        Debug.Log("Note interact");
        StartNoteTransition(NoteDirection.TO_FOREGROUND, NoteFlip.FRONT);
    }

    public void StartNoteTransition(NoteDirection direction, NoteFlip noteTurnet) {
        elapsedTime = 0;
        _direction = direction;
        _noteTurnet = noteTurnet;
    }


    private void AnimateNoteToForeground(){
        Debug.Log("Animate note to foreground.");
        elapsedTime += Time.deltaTime;
        UIManager.GetView<NoteView>().SelectedNote = this;
        UIManager.Show<NoteView>();
        
        float percentageComplete = elapsedTime / desiredDuration;
        var playerManager = PlayerManager.Instance;
        playerManager.SetFirstPersonModeActive(false);
        var playerCamera = playerManager.GetPlayerCamera();
        var endPosition = playerCamera.transform.position + playerCamera.transform.forward;
        var endRotation = playerCamera.transform.rotation * Quaternion.Euler(0,180,0);
        var endScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, _startingScale.z);
        this.transform.position = Vector3.Slerp(this.transform.position, endPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, endRotation, percentageComplete);
        this.transform.localScale = endScale;
        
        
        if(Vector3.Distance(this.transform.position, endPosition) < 0.1){
            _direction = NoteDirection.NONE;
        }
    }

    private void AnimateNoteToBackground(){
        Debug.Log("Animate note to background.");
        elapsedTime += Time.deltaTime;
        UIManager.GetView<NoteView>().SelectedNote = null;
        UIManager.Hide<NoteView>();

        float percentageComplete = elapsedTime / desiredDuration;
        this.transform.position = Vector3.Slerp(this.transform.position, _startingPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _startingRotation, percentageComplete);
        this.transform.localScale = _startingScale;

        var playerManager = PlayerManager.Instance;
        playerManager.SetFirstPersonModeActive(true);
        if(Vector3.Distance(this.transform.position, _startingPosition) < 0.1){
            _direction = NoteDirection.NONE;
        }
    }

    //invokes method to rotate note
    public void OnMouseDown(){
        if(LockRotation) {
            StartCoroutine(RotateNoteToBack());
        }
    }

    private IEnumerator RotateNoteToBack(){
        //if(_noteTurnet==NoteFlip.FRONT){
            for(float i = 0f; i<=180f; i+=10){
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return new WaitForSeconds(0.02f);
            }/*
        }
        else{
            for(float i = 180f; i>=0f; i-=10){
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return new WaitForSeconds(0.05f);
            }
        }
        var finalRotation = Quaternion.Euler(0f, 180f, 0f);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _startingRotation, 1);*/
        _noteTurnet = NoteFlip.BACK;
    }

    private IEnumerator RotateNoteToFront(){
        yield return new WaitForSeconds(0.2f);
        for (float i = 180f; i>=0f; i+=10){
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            yield return new WaitForSeconds(0.02f);
        }

        _noteTurnet = NoteFlip.FRONT;
    }
}
