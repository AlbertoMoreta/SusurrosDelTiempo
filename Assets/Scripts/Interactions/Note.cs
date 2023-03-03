
using UnityEngine;


public enum NoteDirection {
    NONE = -1,
    TO_BACKGROUND = 0,
    TO_FOREGROUND = 1
}

public class Note : MonoBehaviour, Interactable {

    private float desiredDuration = 3f;
    private float elapsedTime;

    private NoteDirection _direction = NoteDirection.NONE;

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private Vector3 _startingScale;

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
        StartNoteTransition(NoteDirection.TO_FOREGROUND);
    }

    public void StartNoteTransition(NoteDirection direction) {
        elapsedTime = 0;
        _direction = direction;
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
        var endPosition = playerCamera.transform.position + playerCamera.transform.forward * 2;
        var endRotation = playerCamera.transform.rotation * Quaternion.Euler(90,0,0);
        var endScale = new Vector3(2f, _startingScale.y, 2f);
        this.transform.position = Vector3.Slerp(this.transform.position, endPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, endRotation, percentageComplete);
        this.transform.localScale = endScale;
        if(this.transform.position == endPosition && 
            this.transform.rotation == endRotation &&
            this.transform.localScale == endScale){
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
        if(this.transform.position == _startingPosition && 
            this.transform.rotation == _startingRotation &&
            this.transform.localScale == _startingScale){
            _direction = NoteDirection.NONE;
        }
    }
}
