
using UnityEngine;


public enum ClockDirection {
    NONE = -1,
    TO_BACKGROUND = 0,
    TO_FOREGROUND = 1
}

public class Clock : MonoBehaviour, Interactable {

    private float desiredDuration = 3f;
    private float elapsedTime;

    private ClockDirection _direction = ClockDirection.NONE;

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private Vector3 _startingScale;

    private SphereCollider _collider;
    private MeshRenderer _renderer;


    // Start is called before the first frame update
    void Start() {
        _startingPosition = this.transform.position;
        _startingRotation = this.transform.rotation;
        _startingScale = this.transform.localScale;
        _collider = gameObject.GetComponent<SphereCollider>();
        _renderer = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {
        switch(_direction) {
            case ClockDirection.TO_FOREGROUND: AnimateClockToForeground(); break;
            case ClockDirection.TO_BACKGROUND: AnimateClockToBackground(); break;
        }
    }


    public void Interact() {
        StartClockTransition(ClockDirection.TO_FOREGROUND);
    }

    public void Hover() {
        _renderer.material.EnableKeyword("_EMISSION");
    }

    public void UnHover() {
        _renderer.material.DisableKeyword("_EMISSION");
    }

    public void StartClockTransition(ClockDirection direction) {
        elapsedTime = 0;
        _direction = direction;
    }


    private void AnimateClockToForeground(){
        // Debug.Log("Animate clock to foreground.");
        _collider.enabled = false;
        elapsedTime += Time.deltaTime;
        UIManager.GetView<ClockView>().SelectedClock = this;
        UIManager.Show<ClockView>();
        
        float percentageComplete = elapsedTime / desiredDuration;
        var playerManager = PlayerManager.Instance;
        playerManager.SetFirstPersonModeActive(false);
        var playerCamera = playerManager.GetPlayerCamera();
        var endPosition = playerCamera.transform.position + playerCamera.transform.forward;
        var endRotation = playerCamera.transform.rotation * Quaternion.Euler(-90,-90,90);
        var endScale = new Vector3(1f, _startingScale.y, 1f);
        this.transform.position = Vector3.Slerp(this.transform.position, endPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, endRotation, percentageComplete);
        this.transform.localScale = endScale;
        if(this.transform.position == endPosition && 
            this.transform.rotation == endRotation){
            _direction = ClockDirection.NONE;
        }
    }

    private void AnimateClockToBackground(){
        // Debug.Log("Animate clock to background.");
        _collider.enabled = true;
        elapsedTime += Time.deltaTime;
        UIManager.GetView<ClockView>().SelectedClock = null;
        UIManager.Hide<ClockView>();

        float percentageComplete = elapsedTime / desiredDuration;
        this.transform.position = Vector3.Slerp(this.transform.position, _startingPosition, percentageComplete);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _startingRotation, percentageComplete);
        this.transform.localScale = _startingScale;

        var playerManager = PlayerManager.Instance;
        playerManager.SetFirstPersonModeActive(true);
        if(this.transform.position == _startingPosition && 
            this.transform.rotation == _startingRotation){
            _direction = ClockDirection.NONE;
        }
    }
}
