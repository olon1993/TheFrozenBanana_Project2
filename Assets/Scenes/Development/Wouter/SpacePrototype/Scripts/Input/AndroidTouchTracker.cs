using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class AndroidTouchTracker : MonoBehaviour
{
	public Text testText;
	public Touch[] touchTracker;
	public int countTracker = 0;

    void Awake()
    {
        
    }

    void Update()
    {
		if (Input.GetMouseButtonDown(0)) {
			countTracker++;
		}
		if (Input.GetMouseButtonUp(0)) {
			countTracker--;
		}
		for (int i = 0; i < countTracker; i++) {
		//	Input.GetTouch(i);
		}

		testText.text = countTracker.ToString();
    }
}
