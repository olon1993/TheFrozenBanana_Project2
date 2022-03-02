using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScript : MonoBehaviour
{


	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			SelectionRay();
		}
	}

	
	void SelectionRay() { 
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		// requires 3D collider to hit unfortunately...
		if (Physics.Raycast(ray, out hit)) {
			LevelInformation li = hit.transform.GetComponent<LevelInformation>();
			Debug.Log("Level " + li.GetLevelId() + " has been selected.");
		}
	}
}
