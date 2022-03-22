using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    [SerializeField] private float _timer = 5.2f;
    private float _startTime;

    private void Start()
    {
        Destroy(gameObject, _timer);
       // _startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //if(_timer >= _startTime)
        //{
        //}
    }
}
