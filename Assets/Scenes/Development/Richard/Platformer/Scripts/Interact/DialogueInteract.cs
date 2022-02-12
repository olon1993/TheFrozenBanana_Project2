using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteract : Interactable
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    [SerializeField] GameObject dialogueBox;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected override void Interact()
    {
        interactTextBox.SetActive(false);
        interactTextActive = false;
        dialogueBox.SetActive(true);
    }
}
