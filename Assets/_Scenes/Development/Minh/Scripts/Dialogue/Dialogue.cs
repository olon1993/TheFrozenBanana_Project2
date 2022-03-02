using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
	public TextMeshProUGUI textComponentTMPRO;
	public Text textComponent;
    public string[] lines;
    public float textSpeed;

    public int index;

    void OnEnable()
    {
		ToggleTextBox(true);
		WriteText(string.Empty);
		StartDialogue();
    }

	void OnDisable() 
	{
		WriteText(string.Empty);
		ToggleTextBox(false);
	}

    void Update()
    {
        if(Input.GetButtonDown("Interact"))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
		string lineText = "";
        foreach (char c in lines[index].ToCharArray())
        {
			lineText += c;
			WriteText(lineText);
			yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
			WriteText(string.Empty);
            StartCoroutine(TypeLine());
        }
        else

        {
            index = 0;
			WriteText(string.Empty);
            gameObject.SetActive(false);
        }
    }

	private void ToggleTextBox(bool newState) {
		if (textComponentTMPRO != null) {
			textComponentTMPRO.gameObject.SetActive(newState);
		} else if (textComponent != null) {
			textComponent.gameObject.SetActive(newState);
		}
	}

	private void WriteText(string str) {
		if (textComponentTMPRO != null) {
			textComponentTMPRO.text = str;
		} else if (textComponent != null) {
			textComponent.text = str;
		}
	}
}


