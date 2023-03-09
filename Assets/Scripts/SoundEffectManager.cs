using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundEffectManager : MonoBehaviour {

    public AudioClip[] sounds;
    private List<AudioSource> _audioSources;

    public static SoundEffectManager Instance {
        get; private set;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start() {
        _audioSources = new List<AudioSource>();
        for (int sound=0; sound < sounds.Length; sound++){
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.1f;
            audioSource.clip = sounds[sound];
            _audioSources.Add(audioSource);    
        }
    }

    public void PlayClip(string name) {
        Debug.Log("Playing clip: " + name);
        var audioSource = _audioSources.First(sound => sound.clip.name == name);
        if (audioSource != null && !audioSource.isPlaying){
            audioSource.Play();
        }

    }

    public void StopClip(string name) {
        Debug.Log("Stopping clip: " + name);
        var audioSource = _audioSources.First(sound => sound.clip.name == name);
        if (audioSource != null && audioSource.isPlaying){
            audioSource.Stop();
        }
    }


}
