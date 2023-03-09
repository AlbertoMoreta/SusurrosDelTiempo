
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    public string dialogKey;

    private SphereCollider _collider;

    // Start is called before the first frame update
    void Start() {
        _collider = gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Player") && dialogKey != null) {
            DialogManager.Instance.StartDialog(dialogKey);
        }
    }
    
}
