using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class JoystickScript : MonoBehaviour
{

	// public setup variables

	public Canvas myCanvas;
	public RectTransform joystick, joystickContainer;
	public float jsDistance, sizeFixer;

	// private set variables
	private Vector2 center;

	// private variables
	public bool selected;

	// public variables
	public Vector2 direction;
	public float magnitude, maxSpeed;

	void Awake() {

		center = joystick.transform.position;
		maxSpeed = 30f; // can be automated later. 
	}

	public void ForceRestart() {
		selected = true;
		Click(false);
	}

	public void Click(bool act) {
		if (selected && !act) {
			Centralize();
		}
		selected = act;
	}

	void Update() {
		if (selected) {
			Follow();
		}
		direction = new Vector2(transform.position.x, transform.position.y) - center;
		magnitude = direction.magnitude;
	}

	private void Centralize() {
		joystick.offsetMin = new Vector2(-sizeFixer, -sizeFixer);
		joystick.offsetMax = new Vector2(sizeFixer, sizeFixer);
	}

	private void Follow() {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickContainer, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = joystickContainer.transform.TransformPoint(Vector2.ClampMagnitude(pos, jsDistance));
	}

}

