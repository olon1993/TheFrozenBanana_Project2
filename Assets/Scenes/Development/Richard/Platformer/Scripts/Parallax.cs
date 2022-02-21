using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Camera cam;
    private Vector3 previousCamPos;
    [SerializeField] float parallaxEffect;

    private void Start()
    {
        cam = Camera.main;
        previousCamPos = cam.transform.position;
    }

    private void LateUpdate()
    {
        float dist = (cam.transform.position.x - previousCamPos.x) * parallaxEffect;

        transform.position = new Vector3(transform.position.x + dist, transform.position.y, transform.position.z);

        previousCamPos = cam.transform.position;
    }
}
