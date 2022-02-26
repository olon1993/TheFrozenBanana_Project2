using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deparent : MonoBehaviour
{
	private void Awake() {
		gameObject.transform.SetParent(null);
	}
}
