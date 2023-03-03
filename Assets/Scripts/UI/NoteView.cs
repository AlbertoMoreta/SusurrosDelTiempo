using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteView : View {
    [SerializeField]
    private Button _exitBtn;
    public Note SelectedNote{
        set; private get;
    }

    public override void Initialize() {
        _exitBtn.onClick.AddListener(() => {
            Debug.Log("Exit Note Clicked");
            SelectedNote.StartNoteTransition(NoteDirection.TO_BACKGROUND, NoteFlip.FRONT);
        });
    }

    
}
