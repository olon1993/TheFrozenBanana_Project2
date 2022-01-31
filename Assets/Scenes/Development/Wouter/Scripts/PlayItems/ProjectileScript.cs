using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileScript : MonoBehaviour
{

	/*******************************************************************
	 * PROJECTILES
	 * This is the common script for all projectiles. Although not very
	 * refined, it does the job of handling every single projectile 
	 * from both player and enemy. Every projectile fired is made so 
	 * that it is automatically destroyed if stays in game too long. 
	 * Otherwise it is destroyed when it hits a target. 
	 * NOTE: Projectiles do not trigger on their owner tags!
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added guns functionality for initial testing. 
	 *	0.2     WH  Added all weapons functionality, removed "SetTag"
	 *	            and replaced it with "SetProperties". Also added
	 *	            the strafe functionality for rockets. 
	 *	0.3     WH  Added enemy shooting functionality.
	 *******************************************************************/
	// editor setup variables
	public GameObject hitEffect;
	public float velocity, areaEffectSize;
	public int damage; 

	// private set variables
	private Transform trans;
	private bool active = false;
	private string ownerTag;

	// private variables
	private Vector2 moveVector;
	private float destroyTimer;
	private int id;

	void Awake() {
		trans = gameObject.transform;
    }

	// Set properties is called from the firing unit (player or enemy). 
	// By using these properties, all defining characteristics of the
	// projectile spawned are dynamic. 
	public void SetProperties(string owner, int shotId, float[] values, float offset) { // values of weapon array, [2];[3];[4] are velocity, dmg and AoE (only if id=1)
		ownerTag = owner;
		id = shotId;
		velocity = values[2];
		damage = (int) values[3];
		if (id == 1) {
			areaEffectSize = values[4];
		}

		if (ownerTag == "Player") {
			if (shotId == 2) {
				moveVector = new Vector2(offset, velocity);
				moveVector = moveVector.normalized * velocity;
			} else {
				moveVector = new Vector2(0, velocity);
			}
		}
		if (shotId == 3) {
			StartCoroutine(Strafe(offset));
		} else {
			active = true;
		}

		if (ownerTag == "Enemy") {
			moveVector = new Vector2(0, -velocity);
		}
		destroyTimer = 20 / velocity;
		Destroy(this.gameObject, destroyTimer);

	}

	// on trigger enter: only allow to work if object is active
	// object is only active after tag has been set
	private void OnTriggerEnter2D(Collider2D col) {
		if (!active) {
			return;
		}

		if (col.tag != ownerTag) {
			active = false;
			if (col.CompareTag("Player")) {
				col.GetComponent<PlayerShipScript>().TakeDamage(damage);
			}

			if (col.CompareTag("BulletBorder")) {
				Destroy(this.gameObject, 0.4f);
				return;
			}

			if (col.CompareTag("Enemy") && id == 1 && areaEffectSize > 0) {
				Explode();
			} else if (col.CompareTag("Enemy")) {
				try {
					col.GetComponent<EnemyShipScript>().TakeDamage(damage);
				} catch (Exception e) {
					Debug.Log("Projectile Collider Error: " + e);
				}
			}

			GameObject hit = Instantiate(hitEffect, trans.position, Quaternion.identity, null) as GameObject;
			Destroy(hit, 2f);
			Destroy(this.gameObject,0.4f);
		} 
	}

	// Explode is made solely for the Cannon type weapon
	private void Explode() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies) {
			if (Vector2.Distance(trans.position, enemy.transform.position) < areaEffectSize) {
				try {
					enemy.GetComponent<EnemyShipScript>().TakeDamage(damage);
				} catch (Exception e) {
					Debug.Log("Explosive Error: " + e);
				}
			}
		}
	}

	// Only movement is done here.
	void FixedUpdate() {
        if (!active) {
			return;
		}
		trans.Translate(moveVector * Time.fixedDeltaTime);
    }

	// Purely made for rockets, strafe makes a rocket
	// go sideways before going forwards. 
	IEnumerator Strafe(float strafeDir) {
		Vector2 tmpMoveVector = new Vector2(Mathf.Sign(strafeDir) * 2,0);
		float t = 0;
		while (t < 1) {
			trans.Translate(tmpMoveVector * Time.deltaTime);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		active = true;
	}
}
