using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject player;

    public static PlayerManager Instance {
        get; private set;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void SetFirstPersonModeActive(bool active) {
        GetMovementComponent().SetCanMove(active);
        GetCameraMovementComponent().SetCanMove(active);
        GetInteractionComponent().SetMouseModeActive(!active);
    }

    public PlayerMovement GetMovementComponent() {
        return player.GetComponent<PlayerMovement>();
    }

    public GameObject GetPlayerCamera() {
        return GameObject.Find("PlayerCamera");
    }

    public MouseTrack GetCameraMovementComponent() {
        return GetPlayerCamera().GetComponent<MouseTrack>();
    }

    public PlayerInteraction GetInteractionComponent() {
        return player.GetComponent<PlayerInteraction>();
    }
}
