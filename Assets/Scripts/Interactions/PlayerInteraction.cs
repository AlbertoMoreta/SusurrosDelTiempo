
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
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, distance, interactableLayers)) {
                currentInteractable = hit.collider.GetComponent<Interactable>();
                crosshair.sprite = activeCrosshair; 
                if(currentInteractable != null){
                    currentInteractable.Hover();
                }
            } else {
                if(currentInteractable != null){
                    currentInteractable.UnHover();
                }
                currentInteractable = null;
                crosshair.sprite = defaultCrosshair;
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
