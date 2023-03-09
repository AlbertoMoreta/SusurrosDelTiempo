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

    private GameObject _sky;
    private GameObject _stars;
    private bool _skyRotation = false;
    private float desiredDuration = 3f;
    private float elapsedTime;

    private int[] _dayHours = {7,8,9,10,11,12,13,14,15,16,17,18};
    private int[] _nightHours = {19,20,21,22,23,24,0,1,2,3,4,5,6};

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
        _sky = GameObject.Find("Sky");
        _stars = _sky.transform.Find("Stars").gameObject;
        _currentTime = defaultTime;
        hour = _currentHour;
    }

    // Update is called once per frame
    void Update() {
        if(hour != _currentHour){
            TimeTravel(hour);
            elapsedTime = 0;
            _skyRotation = true;
        }

        if (_skyRotation){
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

        if (_dayHours.Contains(_currentHour)){ SwitchToDay(); }
        if (_nightHours.Contains(_currentHour)){ SwitchToNight(); }

        if(_currentHour == 12){ 
            SoundEffectManager.Instance.PlayClip("campanas"); 
        }else {
            SoundEffectManager.Instance.StopClip("campanas"); 
        }
    }

    private void RotateSun(int hour){
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;
        var endRotation = Quaternion.Euler((hour - 6) * 15 ,0,0);
        _sky.transform.rotation = Quaternion.Lerp(_sky.transform.rotation, endRotation, percentageComplete);
        if(_sky.transform.rotation == endRotation) {
            _skyRotation = false;
        }
    }

    
    private void SwitchToDay() {
        if(_stars != null) {
            _stars.SetActive(false);
        }
        SoundEffectManager.Instance.StopClip("grillos");
        SoundEffectManager.Instance.PlayClip("pájaros");
    }

    private void SwitchToNight() {
        if(_stars != null) {
            _stars.SetActive(true);
        }
        SoundEffectManager.Instance.PlayClip("grillos");
        SoundEffectManager.Instance.StopClip("pájaros");
    }
}
