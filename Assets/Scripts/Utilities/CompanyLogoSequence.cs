using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyLogoSequence : MonoBehaviour
{
    [SerializeField] private GameObject _toEnable;
    [SerializeField] private AudioSource _splat;
    [SerializeField] private float _destroyTimer = 4f;
    private float _startTime;


    // Start is called before the first frame update
    void Start()
    {
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= _startTime + _destroyTimer)
        {
            _toEnable.SetActive(true);
            Destroy(gameObject, 0.1f);
        }
    }

    public void PlaySplatSoundFx()
    {
        _splat.Play();
    }
}
