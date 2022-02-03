using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    [SerializeField] float baseMovementSpeed;
    [SerializeField] float movementSpeedVariance;
    [SerializeField] float rotationSpeed;
    [SerializeField] float stopDistance;
    [SerializeField] Transform destinationTransform;
    [SerializeField] bool playerControlled = false;
    [SerializeField] bool AIControlled = true;

    GameObject player;

    Vector3 destination;

    float movementSpeed;

    public bool reachedDestination;

    private void Start()
    {
        baseMovementSpeed = baseMovementSpeed + Random.Range(-movementSpeedVariance, movementSpeedVariance);
        movementSpeed = baseMovementSpeed;
        if (destinationTransform != null)
        {
            destination = destinationTransform.position;
        }
    }

    private void Update()
    {
        if (AIControlled)
            AIMovement();
        else if (playerControlled)
        {
            PlayerMovement();
            ExitVehicleCheck();
        }
            
    }

    void PlayerMovement()
    {
        transform.Rotate(-Vector3.forward * Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
    }

    void ExitVehicleCheck()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerExitsVehicle();
        }
    }

    void AIMovement()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.z = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

        }
    }

    public void PlayerEntersVehicle(GameObject go)
    {
        movementSpeed = baseMovementSpeed;
        AIControlled = false;
        playerControlled = true;
        player = go;
        GetComponent<TrafficAwareness>().enabled = false;
    }

    public void PlayerExitsVehicle()
    {
        playerControlled = false;
        player.SetActive(true);
        player.transform.SetParent(null);
    }

    public void AITakesControl()
    {
        playerControlled = false;
        AIControlled = true;
        GetComponent<TrafficAwareness>().enabled = true;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

    public void StopMoving()
    {
        movementSpeed = 0f;
    }

    public void StartMoving()
    {
        movementSpeed = baseMovementSpeed;
    }

}
