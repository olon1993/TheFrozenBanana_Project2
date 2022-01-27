using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    [TagSelector] public string[] TagFilterArray = new string[] { };
    [SerializeField] private bool _destroyAfterSoundFx = false;
    private AudioSource _audioSource;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("AudioSource not found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in TagFilterArray)
        {
            if (other.gameObject.CompareTag(tag))
            {
                if (_showDebugLog)
                {
                    Debug.Log("Collision with " + tag + " has caused DestroyOnCollision to trigger on " + name);
                }

                if (_audioSource.clip != null && _destroyAfterSoundFx)
                {
                    _audioSource.Play();
                    Destroy(gameObject, _audioSource.clip.length);
                }
                else
                {
                    if (_showDebugLog)
                    {
                        Debug.Log("Clilp was null");
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in TagFilterArray)
        {
            if (other.gameObject.CompareTag(tag))
            {
                if (_showDebugLog)
                {
                    Debug.Log("Collision with " + tag + " has caused DestroyOnCollision to trigger on " + name);
                }

                if(_audioSource.clip != null && _destroyAfterSoundFx)
                {
                    _audioSource.Play();
                    Destroy(gameObject, _audioSource.clip.length);
                }
                else
                {
                    if (_showDebugLog)
                    {
                        Debug.Log("Clilp was null");
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
