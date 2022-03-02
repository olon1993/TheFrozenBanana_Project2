using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOverview : MonoBehaviour
{
	[SerializeField] private float transformTime;
	[SerializeField] private Animator screenAC;
	[SerializeField] private GameObject[] markers;
	private Transform trans;
	private Transform focalPoint;

	private void Awake() {
		trans = this.gameObject.transform;
		foreach (GameObject marker in markers) {
			marker.SetActive(false);
		}
	}

	void OnEnable() {
		focalPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		trans.position = focalPoint.position;
		trans.localScale = new Vector3(0.1f, 0.1f, 1f);
		StartCoroutine(StartupScreen());
	}

	void OnDisable() {
		trans.localScale = new Vector3(0,0,0);
		foreach (GameObject marker in markers) {
			marker.SetActive(false);
		}
	}

	private IEnumerator StartupScreen() {
		float t = 0f;
		float x = 0.1f;
		float y = 0.1f;
		float z = 1f;
		yield return new WaitForEndOfFrame();
		// Widen
		while (t < transformTime) {
			t += Time.deltaTime;
			x = t / transformTime;
			trans.localScale = new Vector3(x,y,z);
			yield return new WaitForEndOfFrame();
		}
		x = 1f;
		t = 0f;
		// Heighten
		while (t < transformTime) {
			t += Time.deltaTime;
			y = t / transformTime;
			trans.localScale = new Vector3(x,y,z);
			yield return new WaitForEndOfFrame();
		}
		y = 1f;
		trans.localScale = new Vector3(x, y, z);
		// turn on screen
		screenAC.Play("screen_anim");
		yield return new WaitForSeconds(2);
		foreach (GameObject marker in markers) {
			marker.SetActive(true);
		}
	}
}
