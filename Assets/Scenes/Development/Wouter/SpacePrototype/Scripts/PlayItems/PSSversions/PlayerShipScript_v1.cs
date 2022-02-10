using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/* v1 of this class that works
 * this version is the primary test version.
 * next editions will change in order to accomodate
 * save files, upgrades, improved firing methods.

public class PlayerShipScript : MonoBehaviour {
 
 */
public class PlayerShipScript_v1 : MonoBehaviour {

	// editor setup variables
	public bool androidInput;
	public float maxSpeed, rateOfFire;
	public GameObject deathExplosion;
	public GameObject projectile;
	public Transform ship;
	public Transform[] Guns;

	// private set variables
	private Rigidbody2D rb;
	private MeshCollider mc;
	private Transform trans;
	private Vector3 shipEuler;
	private MainModule mpps; // player particle system
	private JoystickScript jss;
	private UserButtonsScript ubs;

	// private variables
	private Vector3 tiltEuler;
	private Vector2 moveVector;
	private bool firing, refireLock, dead;
	private float speedX, speedY, ppsSize;
	private int gunIndex = 0;

	private Bounds bound;

	// Set private set variables
	void Awake() {
		dead = false;
		if (rb == null) {
			rb = gameObject.GetComponent<Rigidbody2D>();
		}
		if (mc == null) {
			mc = gameObject.GetComponent<MeshCollider>();
		}
		if (trans == null) {
			trans = gameObject.transform;
		}

		mpps = gameObject.GetComponentInChildren<ParticleSystem>().main;
		/* TODO: Android Input Disabled
			if (androidInput) {
				StartCoroutine(RetrieveJoystick());
				StartCoroutine(RetrieveUIButtons());
			}
			*/

		// This gay piece of shit all of a sudden no longer works, override with hard code...
		// shipEuler = ship.rotation.eulerAngles;
		shipEuler = new Vector3(270, 0, 0);
		ship.rotation = new Quaternion(0, 0, 0, 1);

		firing = false;
		refireLock = false;
	}

	/* Android Input Disabled
	IEnumerator RetrieveJoystick() {
		while (jss == null) {
			yield return new WaitForEndOfFrame();
			GameObject js = GameObject.FindGameObjectWithTag("Joystick");
			if (js != null) {
				jss = js.GetComponentInChildren<JoystickScript>();
				Debug.Log("JSS Found");
			}
		}
	}

	IEnumerator RetrieveUIButtons() {
		while (ubs == null) {
			yield return new WaitForEndOfFrame();
			GameObject ub = GameObject.FindGameObjectWithTag("UserButtons");
			if (ub != null) {
				ubs = ub.GetComponent<UserButtonsScript>();
				Debug.Log("UBS Found");
			}
		}
	}
	*/

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Enemy")) {
			Death();
		}
	}

	// Updates. Do not fill anything here but method callers.
	void Update() {
		if (!dead) {
			CheckInput();
			/* Android Input Disabled
					if (jss != null && ubs != null) {
						CheckJoystickInput();
						CheckButtonsInput();
					} else { 
						CheckInput();
					}*/
		}
	}

	void FixedUpdate() {
		if (!dead) {
			Move();
			Tilt();
		}
	}

	// Methods. 

	/* Android Input Disabled
	void CheckJoystickInput() {
		float spd = maxSpeed * (jss.magnitude / jss.maxSpeed);
		moveVector = jss.direction.normalized * spd;
		speedX = moveVector.x;
		speedY = moveVector.y;
		// Replace keys with button on UI
		CheckFiringInput();
	}

	void CheckButtonsInput() {
		if (ubs.userFire && !refireLock) {
			firing = true;
			StartCoroutine(Shoot());
		}
		if (!ubs.userFire) {
			firing = false;
		}
	}
	*/
	void CheckInput() {
		CheckMovementInput();
		CheckFiringInput();
	}

	void CheckMovementInput() {
		speedX = Input.GetAxis("Horizontal") * 5; // multiply by 5 to max out speed
		speedY = Input.GetAxis("Vertical") * 5; // multiply by 5 to max out speed
		moveVector = new Vector2(speedX, speedY);
		if (moveVector.magnitude > maxSpeed) {
			moveVector = moveVector.normalized * maxSpeed;
		}
	}

	void CheckFiringInput() {
		if (Input.GetKeyDown(KeyCode.Space) && !refireLock) {
			firing = true;
			StartCoroutine(Shoot());
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			firing = false;
		}
	}

	void Move() {

		trans.Translate(moveVector * Time.fixedDeltaTime);

		// pps exhaust size
		if (speedY > 0) {
			ppsSize = 0.8f;
		} else if (speedY == 0) {
			ppsSize = 0.5f;
		} else {
			ppsSize = 0.2f;
		}

		// bind the player to the bottom half of the screen, so: x->(-2.5f:2.5f); y->(-4.0f:0.5f)
		float vX = 0;
		float vY = 0;
		if (trans.position.x > 2.5f) {
			vX = 2.5f - trans.position.x;
		} else if (trans.position.x < -2.5f) {
			vX = -2.5f - trans.position.x;
		}
		if (trans.position.y > 0.5f) {
			vY = 0.5f - trans.position.y;
			ppsSize = 0.5f;
		} else if (trans.position.y < -4.0f) {
			vY = -4.0f - trans.position.y;
			ppsSize = 0.5f;
		}
		if (vY != 0 || vX != 0) {
			Vector2 v2 = new Vector2(vX, vY);
			trans.Translate(v2);
		}

		mpps.startSize = ppsSize;
	}

	//left +, right -. max 20 degrees Y angle rotation. move Vector of X is horizontal, rotates ship on Y axis.
	void Tilt() {
		float yDegrees = -(moveVector.x / maxSpeed) * 20;
		tiltEuler = new Vector3(shipEuler.x, yDegrees, shipEuler.z);
		ship.rotation = Quaternion.Euler(tiltEuler);
	}

	IEnumerator Shoot() {
		float t = 1 / rateOfFire;
		refireLock = true;
		while (firing) {
			gunIndex++;
			if (gunIndex >= Guns.Length) {
				gunIndex = 0;
			}
			FireGun(Guns[gunIndex]);
			yield return new WaitForSeconds(t);
		}
		refireLock = false;
	}

	void FireGun(Transform spawner) {
		GameObject prj = Instantiate(projectile, spawner.position, Quaternion.identity, null) as GameObject;
//		prj.GetComponent<ProjectileScript>().SetTag(gameObject.tag);
	}

	public void TakeDamage() {
		// Debug.Log("Player ship takes damage");
	}

	void Death() {
		dead = true;
		GameObject exp = Instantiate(deathExplosion, trans.position, Quaternion.identity, null) as GameObject;
		Destroy(exp, 2f);
		Destroy(this.gameObject, 0.1f);
	}
}
