using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dialogue
    [SerializeField] GameObject helperTextBox;
    [SerializeField] GameObject dialogueBox;
    bool helperTextActive;
    bool dialogueActive;
    RaycastController raycastController;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !helperTextActive)
        {
            helperTextBox.SetActive(true);
            helperTextActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            helperTextBox.SetActive(false);
            helperTextActive = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && helperTextActive)
        {
            helperTextBox.SetActive(false);
            helperTextActive = false;
            dialogueBox.SetActive(true);
        }
    }
}
