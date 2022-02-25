using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\
    [SerializeField] protected GameObject interactTextBox;

    protected bool interactTextIsActive;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && interactTextIsActive)
        {
            interactTextBox.SetActive(false);
            interactTextIsActive = false;
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !interactTextIsActive)
        {
            interactTextBox.SetActive(true);
            interactTextIsActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactTextBox.SetActive(false);
            interactTextIsActive = false;
        }
    }


    protected virtual void Interact()
    { 

    }
}
