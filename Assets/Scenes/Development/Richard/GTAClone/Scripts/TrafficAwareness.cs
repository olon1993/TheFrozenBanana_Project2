using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAwareness : MonoBehaviour
{
    NavigationController navigationController;
    [SerializeField] float rayDistance;
    [SerializeField] Transform raycastOrigin;

    private void Start()
    {
        navigationController = GetComponent<NavigationController>();
    }

    private void Update()
    {
        
        if(Physics2D.Raycast(raycastOrigin.position, transform.TransformDirection(Vector2.up), rayDistance))
        {
            navigationController.StopMoving();
        }
        else
        {
            navigationController.StartMoving();
        }
       
    }
}
