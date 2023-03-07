using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum DIALOG_BOX_POSITIONS {
    LEFT = -1,
    RIGHT = 1
}

public class DialogManager : MonoBehaviour {
    
    public GameObject dialogBox;
    public float dialogBoxOffset = 1f;
    public TextMeshProUGUI diaryNotes;
    public float dialogBoxHeight = 1f;

    private TextMeshProUGUI _subsTextBox;
    
    private Transform _originalParent;

    private DialogCollection _dialogCollection;
    private AudioSource audioSource;
    private GameObject _background;
    private Dialog _currentDialog;
    private Coroutine _subtitlesCoroutine;

    public static DialogManager Instance {
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
        _subsTextBox = GameObject.Find("Subs").GetComponent<TextMeshProUGUI>();
        _background = GameObject.Find("Background");
        _originalParent = dialogBox.transform.parent;
        dialogBox.SetActive(false);

        // Load dialogs from Resources/dialogs.json
        var dialogsFile = Resources.Load("dialogs") as TextAsset;
        Debug.Log("dialogsFile: " + dialogsFile);
        if (dialogsFile != null) {
            _dialogCollection = JsonUtility.FromJson<DialogCollection>(dialogsFile.text);
        } else {
            Debug.LogWarning(dialogsFile + " file does not exists.");
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Play dialog: DialogManager.Instance.SendMessage("StartDialog", "dialog_key");
    public void StartDialog(string dialogKey) {
        if(!audioSource.isPlaying && !dialogBox.activeSelf && !IsClockViewActive()){
            _currentDialog = _dialogCollection.dialogLines.First(dialog => dialog.key == dialogKey);
            PlayAudio(_currentDialog.audioPath);
            _subtitlesCoroutine = StartCoroutine(DisplaySubtitles(_currentDialog.subtitles));
        }
    }

    private void PlayAudio(string audioName) {
        var audioPath = "Sounds/Dialogs/" + audioName;
        var audioClip = Resources.Load<AudioClip>(audioPath);
        audioSource.PlayOneShot(audioClip);
    }

    private IEnumerator DisplaySubtitles(List<Subtitle> subtitles) {
        dialogBox.SetActive(true);
        Quaternion startingRotation = _background.transform.localRotation;

        foreach (Subtitle sub in subtitles) { 
            var character = GameObject.Find(sub.characterName);
            dialogBox.transform.parent = character.transform.parent;
            float xOffset = 0;
            Debug.Log("Position: " + sub.position);
            switch(sub.position) {
                case DIALOG_BOX_POSITIONS.RIGHT: {
                    xOffset = -dialogBoxOffset;
                    _background.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    break;
                }
                case DIALOG_BOX_POSITIONS.LEFT:{
                    xOffset = dialogBoxOffset;
                    _background.transform.localRotation *= Quaternion.AngleAxis(180, Vector3.forward);
                    break;
                }
            }
            dialogBox.transform.position = character.transform.position + new Vector3(xOffset, dialogBoxHeight, 0);
            _subsTextBox.text = sub.text;
            if(!string.IsNullOrEmpty(sub.hint) && !HintUnlocked(sub.hint)){
                diaryNotes.text += "- " + sub.hint + "\n";
            }
            yield return new WaitForSeconds(sub.duration);
        }
        StopSubtitles();
        
    }

    private void StopSubtitles(){
        Debug.Log("Stopping subtitles");
        if(_subsTextBox != null) { _subsTextBox.text = ""; }
        if(_background != null) {
            _background.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }   
        dialogBox.transform.parent = _originalParent;
        if(dialogBox != null){
            dialogBox.SetActive(false);
        }
    }

    public void StopDialog() {
        if(dialogBox != null && dialogBox.activeSelf) {
            if(audioSource != null && audioSource.isPlaying){
                audioSource.Stop();
            }
            if(_currentDialog != null){
                StopCoroutine(_subtitlesCoroutine);
                _currentDialog = null;
            }
            StopSubtitles();
        }
        
    }

    private bool IsClockViewActive(){
        var clockView =UIManager.GetView<ClockView>();
        return clockView != null && clockView.isActiveAndEnabled;
    }

    private bool HintUnlocked(string hint){
        return diaryNotes.text.ToUpper().Contains(hint.ToUpper());
    }
}

