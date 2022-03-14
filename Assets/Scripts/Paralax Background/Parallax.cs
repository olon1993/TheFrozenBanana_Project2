using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform _camTransform;
    private Vector3 _previousCamPos;
    [SerializeField] Vector2 _parallaxEffect;

    private void Start()
    {
        _camTransform = Camera.main.transform;
        _previousCamPos = _camTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = (_camTransform.position - _previousCamPos);

        transform.position += new Vector3(deltaMovement.x * _parallaxEffect.x, deltaMovement.y * _parallaxEffect.y, 0);

        _previousCamPos = _camTransform.transform.position;
    }
}
