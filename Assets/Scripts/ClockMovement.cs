using System;
using UnityEngine;

public enum MIDNIGHT_ENUM {
    PM = 0,
    AM = 1,
}

public class ClockMovement : MonoBehaviour {

    public BoxCollider _midnightCollider;
    public MIDNIGHT_ENUM timeFormat = MIDNIGHT_ENUM.PM;
    private bool _isRotating;

    // Update is called once per frame
    void Update() {
        if (_isRotating) {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            
            mousePos.x -= objectPos.x;
            mousePos.y -=  objectPos.y;

            float angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg % 360;
            if (angle < 0) {
                angle += 360;
            }
            angle = (int) Math.Round(angle / 30) * 30;
  
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, angle, 0), 10 * Time.deltaTime);
            var hour = (int) angle / 30;
            if (hour == 0){
                hour = 12;
            }
            if(timeFormat == MIDNIGHT_ENUM.PM){
                hour += 12;
            }
            // Debug.Log("HOUR: " + hour + " FORMAT: " + timeFormat);
            TimeTravelManager.Instance.SetTimeTravelHour(hour);
        }
    }

    void OnMouseDown() {
        _isRotating = true;
    }

    void OnMouseUp() {
        _isRotating = false;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger");
        if(other.tag.Equals("MidnightTrigger")) {  
            if(timeFormat == MIDNIGHT_ENUM.PM) {
                timeFormat = MIDNIGHT_ENUM.AM;
            } else {
                timeFormat = MIDNIGHT_ENUM.PM;
            }
        }
    }
}
