using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float turnSpeed;

    GameObject car;
    bool getInCar;

    private void Update()
    {
        Move();
        InteractWithCar();
    }


    void Move()
    {
        transform.Rotate(-Vector3.forward * Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
    }

    void InteractWithCar()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 1.0f, LayerMask.GetMask("Vehicle")))
        {
            car = hit.transform.gameObject;
            NavigationController navControler = car.GetComponent<NavigationController>();
            navControler.StopMoving();

            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.transform.position = car.transform.position;
                gameObject.transform.SetParent(car.transform);
                getInCar = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (getInCar)
        {
            car.GetComponent<NavigationController>().PlayerEntersVehicle(gameObject);
            getInCar = false;
            car = null;
            gameObject.SetActive(false);
        }
    }
}
