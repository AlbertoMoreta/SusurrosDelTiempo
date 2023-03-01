using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour {

    public TextMeshProUGUI subsTextBox;

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

    public void StartDialog(string dialogKey) {
        var selectedDialog = _dialogCollection.dialogLines.First(dialog => dialog.key == dialogKey);

        // Play audio
        var audioPath = "Sounds/" + selectedDialog.audioPath;
        var audioClip = Resources.Load<AudioClip>(audioPath);
        audioSource.PlayOneShot(audioClip);

        StartCoroutine(DisplaySubtitles(selectedDialog.subtitles));
    }

     private IEnumerator DisplaySubtitles(List<Subtitle> subtitles) {
        foreach (Subtitle sub in subtitles) {
            subsTextBox.text = sub.text;
            yield return new WaitForSeconds(sub.duration);
        }
        subsTextBox.text = "";
    }
}

