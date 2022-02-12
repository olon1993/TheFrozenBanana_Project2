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

    protected bool interactTextActive;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && interactTextActive)
        {
            interactTextBox.SetActive(false);
            interactTextActive = false;
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !interactTextActive)
        {
            interactTextBox.SetActive(true);
            interactTextActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactTextBox.SetActive(false);
            interactTextActive = false;
        }
    }


    protected virtual void Interact()
    { 

    }
}
