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
	[SerializeField] protected GameObject interactibleObject;

	protected bool interactible, interactTextIsActive, interactibleActive;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && interactTextIsActive)
        {
            interactTextBox.SetActive(false);
            interactTextIsActive = false;
		} 

		if (Input.GetButtonDown("Interact") && interactible) {
			Interact();
		}

	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !interactTextIsActive)
        {
            interactTextBox.SetActive(true);
            interactTextIsActive = true;
			interactible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            interactTextBox.SetActive(false);
            interactTextIsActive = false;
			interactibleActive = false;
			interactible = false;
			if (interactibleObject != null) {
				interactibleObject.SetActive(false);
			}
		}
    }


    protected virtual void Interact()
    { 
		interactibleActive = !interactibleActive;
		if (interactibleObject != null) {
			interactibleObject.SetActive(interactibleActive);
		}
    }
}
