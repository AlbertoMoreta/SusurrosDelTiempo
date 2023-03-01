using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour {

    public TextMeshProUGUI subsTextBox;

    private DialogList _dialogs;

    // Start is called before the first frame update
    void Start() {
        // Load dialogs from Resources/dialogs.json
        var dialogsFile = Resources.Load("dialogs") as TextAsset;
        Debug.Log("dialogsFile: " + dialogsFile);
        if (dialogsFile != null) {
            _dialogs = JsonUtility.FromJson<DialogList>(dialogsFile.text);
            Debug.Log("Dialogs: " + _dialogs.dialogLines.ToString());
        } else {
            Debug.LogWarning(dialogsFile + " file does not exists.");
        }
    }

    public void StartDialog(string dialogKey) {
        var selectedDialog = _dialogs.dialogLines.First(dialog => dialog.key == dialogKey);

        //ToDo: PlayAudio

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

[Serializable]
public class DialogList {
    public List<Dialog> dialogLines = new List<Dialog>();
}

[Serializable]
public class Dialog {
    public string key;
    public string audioPath;
    public List<Subtitle> subtitles;
}

[Serializable]
public class Subtitle  {
    public string text;
    public float duration;
}
