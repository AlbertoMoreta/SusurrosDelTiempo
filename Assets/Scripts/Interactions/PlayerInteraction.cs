
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {

    public float distance = 3f;
    public LayerMask interactableLayers;
    public Transform playerCamera;
    public Image crosshair;

    private Interactable currentInteractable;
    private Sprite activeCrosshair;
    private Sprite defaultCrosshair;

    private bool _mouseMode = false;

    // Start is called before the first frame update
    void Start() {
        activeCrosshair = Resources.Load<Sprite>("Sprites/crosshair_active");
        defaultCrosshair = Resources.Load<Sprite>("Sprites/crosshair_default");
    }

    // Update is called once per frame
    void Update() {
        if(!_mouseMode){
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, distance)) {
                var newInteractable = hit.collider.GetComponent<Interactable>();
                if(newInteractable != null){
                    currentInteractable = newInteractable;
                    if(crosshair != null) { crosshair.sprite = activeCrosshair; }
                    currentInteractable.Hover();
                } else {
                    if(currentInteractable != null){
                        currentInteractable.UnHover();
                        currentInteractable = null;
                    }
                    if(crosshair != null) { crosshair.sprite = defaultCrosshair; }
                }
            } 
            else {
                if(currentInteractable != null){
                    currentInteractable.UnHover();
                    currentInteractable = null;
                }
                if(crosshair != null) { crosshair.sprite = defaultCrosshair; }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (currentInteractable != null) {
                    Debug.Log("CurrentInteractable = " + currentInteractable);
                    currentInteractable.Interact();
                }
            }
        }
    }

    public void SetMouseModeActive(bool active) {
        _mouseMode = active;
    }
}
