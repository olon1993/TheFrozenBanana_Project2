using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        AudioEvents.PlaySoundClip += OnPlaySoundClip;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AudioEvents.PlaySoundClip -= OnPlaySoundClip;
    }

    public void OnPlaySoundClip(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}
