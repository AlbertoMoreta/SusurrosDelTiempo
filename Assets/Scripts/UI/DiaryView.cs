using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryView : View {
    [SerializeField]
    private Button _exitBtn;

    public override void Initialize() {
        _exitBtn.onClick.AddListener(() => {
            Debug.Log("Exit Diary Clicked");
            UIManager.Hide<DiaryView>();
        });
    }

    
}
