using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockView : View {
    [SerializeField]
    private Button _exitBtn;
    public Clock SelectedClock{
        set; private get;
    }

    public override void Initialize() {
        _exitBtn.onClick.AddListener(() => {
            Debug.Log("Clicked");
            SelectedClock.StartClockTransition(ClockDirection.TO_BACKGROUND);
        });
    }

    
}
