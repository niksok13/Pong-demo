using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSTools;
using UnityEngine;

public class ZBindAudioSource : MonoBehaviour
{
    public AudioClip[] clips;

    private Dictionary<string, AudioClip> _clips;
    private AudioSource source;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
        EventManager.Bind("sound_play",Play);
        Model.Bind("sound_volume",UpdateVolume);
        _clips = clips.ToDictionary(a => a.name, b => b);
    }

    private void UpdateVolume(object obj)
    {
        source.volume = Model.Get("sound_volume", 1f);
    }

    private void Play(object arg)
    {
        if (source.volume>0)
            source.PlayOneShot(_clips[(string)arg]);
    }
}
