using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyShipScript : MonoBehaviour
{
	/*******************************************************************
	 * ENEMY SHIP
	 * This is the main script for ALL enemies. The parameters per 
	 * enemy type can be given in the editor. 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *	0.2		WH	Added firing functionality
	 *	0.3     WH  Added boss functionality
	 *******************************************************************/
	// editor setup variables
	public GameObject projectile;
	public Transform ship;
	public Transform[] projectileSpawner;
	public float[] projectileInfo;
	public int enemyIndex; // this is the same ID number as the array value in the enemy ships array of the play management script
	public int maxHp, enemyCashValue;
	public float maxSpeed, rateOfFire, flightPause;
	public bool bossEnemy;
	public GameObject deathExplosion;

	// private set variables
	private FlightPathLibrary fpl;
	private PlayManagementScript pms;
	private Transform[] flightPath;
	private Rigidbody2D rb;
	private MeshCollider mc;
	private Transform trans;
	private Vector3 shipEuler;
	private MainModule[] meps; // enemy particle system

	// private variables
	private bool dead = false;
	private Vector3 tiltEuler;
	private Vector2 moveVector = new Vector2(0,0);
	public float epsSize;
	public int currentHp;
	private int flightPathIndex = 0;
	private int flightPathIterator = 0;
	private int shipId; // needed at runtime for respawn options
	private bool readyToFly = false;


	// SETUP
	void Awake() {
		if (rb == null) {
			rb = gameObject.GetComponent<Rigidbody2D>();
		}
		if (mc == null) {
			mc = gameObject.GetComponent<MeshCollider>();
		}
		if (trans == null) {
			trans = gameObject.transform;
		}
		if (pms == null) {
			pms = GameObject.FindGameObjectWithTag("PlayManager").GetComponent<PlayManagementScript>();
		}
		if (fpl == null) {
			fpl = GameObject.FindGameObjectWithTag("PlayManager").GetComponent<FlightPathLibrary>();
			flightPath = null;
		}
		meps = new MainModule[0];
		ParticleSystem[] ps = gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem pss in ps) {
			AddMainModule(ref meps, pss.main);
		}
		// This gay piece of shit all of a sudden no longer works, override with hard code...
		// shipEuler = ship.rotation.eulerAngles;
		shipEuler = new Vector3(90, 180, 0);
		ship.rotation = new Quaternion(0, 0, 0, 1);

		currentHp = maxHp;
		readyToFly = true;
		StartCoroutine(AutoFire());
	}

	private void AddMainModule(ref MainModule[] array, MainModule add) {
		int newLength = array.Length + 1;
		MainModule[] tmpArray = new MainModule[newLength];
		for (int i = 0; i < array.Length; i++) {
			tmpArray[i] = array[i];
		}
		tmpArray[newLength - 1] = add;
		array = tmpArray;
	}

	private void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player")) {
		//	Death(false);
		}
	}

	// UPDATE
	// Only call methods from here.
	void FixedUpdate() {
		if (!dead) {
			Move();
			Tilt();
		}
	}

	// METHODS

	// Move
	// Handles the movement and exhaust size.
	void Move() {
		trans.Translate(moveVector * Time.fixedDeltaTime);
		// eps exhaust size
		if (moveVector.y < 0) {
			epsSize = 0.8f;
		} else if (moveVector.y == 0) {
			epsSize = 0.5f;
		} else {
			epsSize = 0.2f;
		}
		for (int i = 0; i < meps.Length; i++) {
			meps[i].startSize = epsSize;
		}
	}

	// Tilt
	//left +, right -. max 20 degrees Y angle rotation. 
	// move Vector of X is horizontal, rotates ship on Y axis.
	void Tilt() {
		float yDegrees = -(moveVector.x / maxSpeed) * 40;
		tiltEuler = new Vector3(shipEuler.x, yDegrees, shipEuler.z);
		ship.rotation = Quaternion.Euler(tiltEuler);
	}

	// PushFlightPath 
	// is called when spawning enemy from PlayManagementScript. 
	public void PushFlightInfo(int i, int id, int startHp) {
		flightPathIndex = i;
		shipId = id;
		if (startHp != 0) {
			currentHp = startHp;
		}
		StartCoroutine(RunFlightPath());
	}

	// RunFlightPath
	// Makes the enemy go from marker to marker, with a pause between each marker. 
	// v0.2: Added bossEnemy bool so that the boss does not despawn like regulars.
	IEnumerator RunFlightPath() {
		while (!readyToFly) {
			yield return new WaitForEndOfFrame();
		}

		Vector2 direction = new Vector2(0, 0);
		if (bossEnemy) {
			flightPath = fpl.FlightPaths[flightPathIndex];
			yield return new WaitForSeconds(flightPause);
			while (!dead) {
				direction = flightPath[flightPathIterator].position - trans.position;
				AlterMoveVector(direction);
				if (Vector2.Distance(trans.position, flightPath[flightPathIterator].position) < 0.1f) {
					flightPathIterator++;
					moveVector = Vector2.zero;
					yield return new WaitForSeconds(flightPause);
					if (flightPathIterator == flightPath.Length) {
						flightPathIterator = 1;
					}
				}
				yield return new WaitForEndOfFrame();
			}
		} else {
			flightPath = fpl.FlightPaths[flightPathIndex];
			yield return new WaitForSeconds(flightPause);
			while (flightPathIterator < flightPath.Length) {
				direction = flightPath[flightPathIterator].position - trans.position;
				AlterMoveVector(direction);
				if (Vector2.Distance(trans.position, flightPath[flightPathIterator].position) < 0.1f) {
					flightPathIterator++;
					moveVector = Vector2.zero;
					yield return new WaitForSeconds(flightPause);
				}
				yield return new WaitForEndOfFrame();
			}
		}
		Death(true);
	}

	// AlterMoveVector
	// Keeps the move Vector up to date so the enemy always
	// goes towards their target. 
	void AlterMoveVector(Vector2 dir) {
		moveVector = dir.normalized * maxSpeed;
	}

	// AutoFire
	// Called on start. As long as the enemy is not dead,
	// it keeps firing in intervals. 
	IEnumerator AutoFire() {
		float fireTimer = 1 / rateOfFire;
		// pause before firing
		yield return new WaitForSeconds(fireTimer);
		while (!dead) {
			for (int i = 0; i < projectileSpawner.Length; i++) {
				SpawnProjectile(projectileSpawner[i]);
				yield return new WaitForSeconds(fireTimer);
			}
			// this may look redundant, but this gives an extra pause for larger enemies
			yield return new WaitForSeconds(fireTimer);
		}
	}

	// SpawnProjectile
	private void SpawnProjectile(Transform spawnPoint) {
		GameObject prj = Instantiate(projectile, spawnPoint.position, Quaternion.identity, null) as GameObject;
		prj.GetComponent<ProjectileScript>().SetProperties(gameObject.tag, (int) projectileInfo[0], projectileInfo, 0);
	}

	// TakeDamage
	public void TakeDamage(int dmg) {
		if (dead) {
			return;
		}
		currentHp -= dmg;
		if (currentHp <= 0f) {
			Death(false);
		}
	}

	// Death
	void Death(bool respawn) {
		if (dead) {
			return;
		}
		dead = true;
		if (respawn) {
			pms.AddToRespawnEnemies(enemyIndex, flightPathIndex, shipId, currentHp);
		} else {
			GameObject exp = Instantiate(deathExplosion, trans.position, Quaternion.identity, null) as GameObject;
			pms.AddEarnedCash(enemyCashValue);
			Destroy(exp, 2f);
		}
		pms.RemoveEnemyFromCurrentEnemies(this.gameObject, shipId);
	}
}
