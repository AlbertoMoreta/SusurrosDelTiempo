using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogManager : MonoBehaviour {
    
    public GameObject dialogBox;
    private TextMeshProUGUI _characterNameTextBox;
    private TextMeshProUGUI _subsTextBox;

    private DialogCollection _dialogCollection;
    private AudioSource audioSource;

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
        _characterNameTextBox = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
        _subsTextBox = GameObject.Find("Subs").GetComponent<TextMeshProUGUI>();
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
        if(!audioSource.isPlaying){
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
        foreach (Subtitle sub in subtitles) {
            _characterNameTextBox.text = sub.characterName;
            _subsTextBox.text = sub.text;
            yield return new WaitForSeconds(sub.duration);
        }
        _characterNameTextBox.text = _subsTextBox.text = "";
        dialogBox.SetActive(false);
    }

}

