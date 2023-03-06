
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField]
    public View[] views;
    private bool _isDiaryDisplaying = false;
    
    public static UIManager Instance {
        get; private set;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
       for (int i = 0; i < Instance.views.Length; i++) {
            Instance.views[i].Initialize();
        } 
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if(_isDiaryDisplaying) {
                UIManager.Show<DiaryView>();
            } else {
                UIManager.Hide<DiaryView>();
            }
            _isDiaryDisplaying = !_isDiaryDisplaying;
        }
    }


    public static T GetView<T>() where T : View {
        for (int i = 0; i < Instance.views.Length; i++) {
            if (Instance.views[i] is T view) {
                return view;
            }
        }

        return null;
    }

    public static void Show<T>(bool remember = true) where T : View {
        GetView<T>().Show();
    }

    public static void Hide<T>(bool remember = true) where T : View {
        GetView<T>().Hide();
    }

}
