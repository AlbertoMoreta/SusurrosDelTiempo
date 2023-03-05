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
    private int _previousHour;
    

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

             if (angle == 360){
                angle = 0;
            }

            var hour = (int) angle / 30;
            if (hour == 0) { 
                hour += 12;

                if (timeFormat == MIDNIGHT_ENUM.AM) {
                    if (_previousHour != 11) {
                        hour += 12;
                    }
                }
            } 

            if(((timeFormat == MIDNIGHT_ENUM.PM) && (hour != 12) && !((_previousHour <= 12) && (hour == 11)))){
                hour += 12;     
            }

            if(((timeFormat == MIDNIGHT_ENUM.PM) && (_previousHour == 23) && (hour == 12))){
                hour += 12;     
            }

            if(((timeFormat == MIDNIGHT_ENUM.AM) && (_previousHour == 24) && (hour == 11))){
                hour += 12;     
            }
            
            if (_previousHour == 0) {
                _previousHour = hour;
            }

            var from = transform.localRotation;
            var to = Quaternion.Euler(0, angle, 0);
            // Debug.Log("PEVIOUS HOUR: " + _previousHour + "HOUR: " + hour + " ANGLE: " + angle + " FORMAT: " + timeFormat);
            // Debug.Log("ABS: " + Math.Abs(_previousHour - hour));

            if (Math.Abs(_previousHour - hour) <= 1) {
                transform.localRotation = Quaternion.Lerp(from, to, 0.4f);
                TimeTravelManager.Instance.SetTimeTravelHour(hour);
                _previousHour = hour;
            } else {
                Debug.Log("WRONG!");
            }
            
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
