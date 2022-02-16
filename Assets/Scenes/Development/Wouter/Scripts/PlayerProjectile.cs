using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

public class PlayerProjectile : MonoBehaviour {

	[SerializeField] private GameObject projectileImage;
	[SerializeField] private float velocity;
	private bool active;
	private Vector3 direction;
	private Damage dmg;

	private void Awake() {
		active = false;
		dmg = gameObject.GetComponentInChildren<Damage>();
		if (dmg == null) {
			Debug.LogError("Damage could not be found");
		}
	}

	// This is called from the child, as the child contains the collider
	// this has to do with the transform rotation and influence on 
	// the vector direction if done on the parent object. 
	public void TriggerCollision(Collider2D col) {
		RunCollisionCheck(col);
	}

	// This is the actual OnTriggerEnter2D, but it's called from the child
	private void RunCollisionCheck(Collider2D col) {
		if (!active) {
			return;
		}
		if (col.CompareTag("Player")) {
			return;
		}
		if (active) {
			Deactivate();
		}
		Health hpScript = col.GetComponent<Health>();
		if (hpScript != null) {
			Debug.Log("I actually Hit something!!");
			// Do damage
			hpScript.TakeDamage(dmg);
		} else {
			Debug.Log("Damn! Nothing special hit...");
		}
	}

	// only handles movement if active
	private void FixedUpdate() {
		if (active) {
			gameObject.transform.Translate(direction.normalized * velocity * Time.fixedDeltaTime);
		}
	}

	// Setup gives the target to go to and the rotation of the projectile renderer
	public void Setup(Vector3 start, Vector3 target, Quaternion projectileRotation) {
		direction = target - start;
		projectileImage.transform.rotation = projectileRotation;
		active = true;
	}

	// turns off the projectile. Not destroyed here, as it is destroyed in main 
	// should it miss. calling destroy here as well will make destroy be called 
	// twice and creating null reference exception. hence, deactivate.
	private void Deactivate() {
		active = false;
		gameObject.SetActive(false);
	}
}
