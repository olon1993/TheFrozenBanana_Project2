using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserButtonsScript : MonoBehaviour
{

	public bool userFire;

	void Awake() {
		userFire = false;
	}

	public void Fire(bool fire) {
		userFire = fire;
	}
}
