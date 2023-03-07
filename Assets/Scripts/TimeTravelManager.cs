using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TimeTravelManager : MonoBehaviour {

    public int hour;
    public GameObject defaultTime;
    public GameObject[] times;

    private int _currentHour;    
    private GameObject _currentTime;

    private GameObject _sun;
    private bool _sunRotation = false;
    private float desiredDuration = 3f;
    private float elapsedTime;

    public static TimeTravelManager Instance {
        get; private set;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    // Start is called before the first frame update
    void Start() {
        _sun = GameObject.Find("Sky");
        _currentTime = defaultTime;
        hour = _currentHour;
    }

    // Update is called once per frame
    void Update() {
        if(hour != _currentHour){
            TimeTravel(hour);
            elapsedTime = 0;
            _sunRotation = true;
        }

        if (_sunRotation){
            RotateSun(hour);
        }
    }

    public void SetTimeTravelHour(int hour) {
        this.hour = hour;
    }

    private void TimeTravel(int hour){
        Debug.Log("Time traveling to: " + hour);
        DialogManager.Instance.StopDialog(); // Stop ongoing dialog
        _currentTime.SetActive(false);

        var time = times.FirstOrDefault(t => t.name == hour.ToString());
        if(!time) {
            time = defaultTime;
        } 
        
        time.SetActive(true);

        _currentTime = time;
        _currentHour = hour;
    }

    private void RotateSun(int hour){
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        var endRotation = Quaternion.Euler((hour - 6) * 15 ,0,0);
        _sun.transform.rotation = Quaternion.Lerp(_sun.transform.rotation, endRotation, percentageComplete);
        if(_sun.transform.rotation == endRotation) {
            _sunRotation = false;
        }
    }
}
