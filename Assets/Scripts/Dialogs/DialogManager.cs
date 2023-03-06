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
    private TextMeshProUGUI _subsTextBox;
    
    private Transform _originalParent;

    private DialogCollection _dialogCollection;
    private AudioSource audioSource;
    private GameObject _background;

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
        if(!audioSource.isPlaying && !dialogBox.activeSelf){
            var selectedDialog = _dialogCollection.dialogLines.First(dialog => dialog.key == dialogKey);
            PlayAudio(selectedDialog.audioPath);
            StartCoroutine(DisplaySubtitles(selectedDialog.subtitles));
        }
    }

    private void PlayAudio(string audioName) {
        var audioPath = "Sounds/" + audioName;
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
                    _background.transform.localRotation = startingRotation;
                    break;
                }
                case DIALOG_BOX_POSITIONS.LEFT:{
                    xOffset = dialogBoxOffset;
                    _background.transform.localRotation *= Quaternion.AngleAxis(180, Vector3.forward);
                    break;
                }
            }
            dialogBox.transform.position = character.transform.position + new Vector3(xOffset, 0.7f, 0);
            _subsTextBox.text = sub.text;
            // if(sub.hint){
                diaryNotes.text += "- " + "hola\n";
            // }
            yield return new WaitForSeconds(sub.duration);
        }
        _subsTextBox.text = "";
        _background.transform.localRotation = startingRotation;
        dialogBox.transform.parent =_originalParent;
        dialogBox.SetActive(false);
    }

}

