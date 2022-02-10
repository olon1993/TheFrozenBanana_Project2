using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerShipScript : MonoBehaviour
{

	/*******************************************************************
	 * PLAYER SHIP
	 * This script is what controls the ship of the player during
	 * playing the levels. With input being very simple, the script 
	 * itself does a lot to accomodate all player options. From the 
	 * input, player controls movement and firing. The rest is done
	 * within this script.
	 *		- Setup weapons
	 *		- Setup systems
	 *		- Tracks all stats
	 *		- Handles death
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *	0.2     WH  Attempt of Android Input. 
	 *	0.3     WH  Reiteration of entire script to add all ship
	 *	            statistics (weapons and systems). Removed Android
	 *	            input options and saved this in a different script
	 *	            as a backup.
	 *******************************************************************/
	// editor setup variables
	public bool androidInput;
	public GameObject deathExplosion;
	public GameObject[] projectiles;
	public Transform ship;
	public Transform[] Guns, Cannon, Lasers, Rockets;

	// private set variables
	private PlayManagementScript pms;
	private ShipUpgradeLibrary sulRef;
	private Rigidbody2D rb;
	private MeshCollider mc;
	private Transform trans;
	private Vector3 shipEuler;
	private MainModule mpps; // player particle system
	private PlayUIScript puis;

	// private variables
	private Vector3 tiltEuler;
	private Vector2 moveVector;
	private bool firing, refireLock, dead, shieldRegenerating;
	private float speedX, speedY, ppsSize, dmgTimer;
	private float currentHp, maxHp, currentShield, maxShield, shieldRegenRate, armor, maxSpeed, currentTime, maxTime, maxHeat, currentHeat, heatRegen;
	private float[][] weapons;

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
		// This gay piece of shit all of a sudden no longer works, override with hard code...
		// shipEuler = ship.rotation.eulerAngles;
		shipEuler = new Vector3(270,0,0);
		ship.rotation = new Quaternion(0,0,0,1);
		
		firing = false;
		refireLock = false;
	}

	// SETUP PART
	// Sets up the initial stats for guns and systems, then connects
	// to the in game UI
	public void SetupShipProfile(PlayManagementScript pmst, int[][] shipProfile, ShipUpgradeLibrary sul, PlayUIScript pscript) {
		pms = pmst;
		sulRef = sul;
		SetupShipGuns(shipProfile);
		SetupShipSystems(shipProfile);
		puis = pscript;
		puis.SetupInitialValues(maxHp, maxShield, maxHeat, maxTime);
	}

	// ShipGuns
	// The profile only contains int references. Here the
	// translation between index and values is done for play.
	private void SetupShipGuns(int[][] profile) {

		float[][] sulProfile = new float[4][];
		float[] gunProfile = new float[5];
		gunProfile[0] = profile[0][0];
		gunProfile[1] = sulRef.gunFireRate[profile[0][1]];
		gunProfile[2] = sulRef.gunProjectileSpeed[profile[0][2]];
		gunProfile[3] = sulRef.gunDamage[profile[0][3]];
		gunProfile[4] = sulRef.gunSpecial[profile[0][4]];

		sulProfile[0] = gunProfile;

		float[] cannonProfile = new float[5];
		cannonProfile[0] = profile[1][0];
		cannonProfile[1] = sulRef.cannonFireRate[profile[1][1]];
		cannonProfile[2] = sulRef.cannonProjectileSpeed[profile[1][2]];
		cannonProfile[3] = sulRef.cannonDamage[profile[1][3]];
		cannonProfile[4] = sulRef.cannonSpecial[profile[1][4]];

		sulProfile[1] = cannonProfile;

		float[] laserProfile = new float[5];
		laserProfile[0] = profile[2][0];
		laserProfile[1] = sulRef.laserFireRate[profile[2][1]];
		laserProfile[2] = sulRef.laserProjectileSpeed[profile[2][2]];
		laserProfile[3] = sulRef.laserDamage[profile[2][3]];
		laserProfile[4] = sulRef.laserSpecial[profile[2][4]];

		sulProfile[2] = laserProfile;

		float[] rocketProfile = new float[5];
		rocketProfile[0] = profile[3][0];
		rocketProfile[1] = sulRef.rocketFireRate[profile[3][1]];
		rocketProfile[2] = sulRef.rocketProjectileSpeed[profile[3][2]];
		rocketProfile[3] = sulRef.rocketDamage[profile[3][3]];
		rocketProfile[4] = sulRef.rocketSpecial[profile[3][4]];

		sulProfile[3] = rocketProfile;

		weapons = sulProfile;
	}

	// ShipSystems
	// The profile only contains int references. Here the
	// translation between index and values is done for play.
	private void SetupShipSystems(int[][] profile) {
		armor = sulRef.shipArmor[profile[4][1]];
		maxHp = sulRef.shipStructure[profile[4][2]];
		currentHp = maxHp;
		maxShield = sulRef.shieldGenerator[profile[5][1]];
		currentShield = maxShield;
		shieldRegenRate = sulRef.shieldCapacitor[profile[5][2]];
		maxTime = sulRef.engineEnergy[profile[6][1]];
		currentTime = maxTime;
		maxSpeed = sulRef.engineOutput[profile[6][2]];
		maxHeat = sulRef.coolerTanks[profile[7][1]];
		currentHeat = 0;
		heatRegen = sulRef.coolerTurbines[profile[7][2]];
	}

	// On collision with an enemy, player dies!
	// NOTE: projectiles are not tagged for this purpose
	private void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Enemy")) {
			Death();
		}
	}

	// UPDATES PART
	// Do not fill anything here but method callers.
	// The stats bars are updated here because they can change
	// per deltaTime. Only health does not, so this is changed
	// only when taking damage. 
	void Update() {
		if (!dead) {
			CheckInput();
		}
		if (pms.victory) {
			return;
		}
		currentTime -= Time.deltaTime;
		puis.UpdateTimeValue(currentTime);
		puis.UpdateHeatValue(currentHeat);
		puis.UpdateShieldValue(currentShield);
	}

	// Movement done in fixed updates. 
	void FixedUpdate() {
		if (!dead) {
			Move();
			Tilt();
		}
	}

	// METHODS PART
	
	// CheckInput
	// does what it says
	void CheckInput() {
		CheckMovementInput();
		CheckFiringInput();
	}

	// MovementInput is based on horizontal/vertical input
	void CheckMovementInput() {
		speedX = Input.GetAxis("Horizontal") * 10; // multiply by 5 to max out speed
		speedY = Input.GetAxis("Vertical") * 10; // multiply by 5 to max out speed
		moveVector = new Vector2(speedX, speedY);
		if (moveVector.magnitude > maxSpeed) {
			moveVector = moveVector.normalized * maxSpeed;
		}
	}

	// Firing input is based on SPACE bar
	// when firing, checks which weapons are active
	// and triggers all these to separately run their coroutine
	void CheckFiringInput() {
		if (Input.GetKeyDown(KeyCode.Space) && !refireLock) {
			firing = true;
//			StartCoroutine(Shoot());
			for (int i = 0; i < weapons.Length; i++) {
				if (weapons[i][0] == 1) {
					if (i == 0) {
						StartCoroutine(FireWeapon(i, weapons[i], Guns));
					} else if (i == 1) {
						StartCoroutine(FireWeapon(i, weapons[i], Cannon));
					} else if (i == 2) {
						StartCoroutine(FireWeapon(i, weapons[i], Lasers));
					} else if (i == 3) {
						StartCoroutine(FireWeapon(i, weapons[i], Rockets));
					}
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			firing = false;
			StartCoroutine(Cooling());
		}
	}

	// Move
	// Handles the actual movement. Also binds the player ship into
	// a border so the player does not go off screen or too far up.
	// ppsSize is the size of the exhaust.
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
		if (trans.position.x > 3f) {
			vX = 3f - trans.position.x;
		} else if (trans.position.x < -3f) {
			vX = -3f - trans.position.x;
		}
		if (trans.position.y > 0.5f) {
			vY = 0.5f - trans.position.y;
			ppsSize = 0.5f;
		} else if (trans.position.y < -4.5f) {
			vY = -4.5f - trans.position.y;
			ppsSize = 0.5f;
		}
		if (vY != 0 || vX != 0) {
			Vector2 v2 = new Vector2(vX,vY);
			trans.Translate(v2);
		}

		mpps.startSize = ppsSize;
	}

	// Tilt
	// left +, right -. max 20 degrees Y angle rotation. 
	// move Vector of X is horizontal, rotates ship on Y axis.
	void Tilt() {
		float yDegrees = - (moveVector.x / maxSpeed) * 20;
		tiltEuler = new Vector3(shipEuler.x, yDegrees, shipEuler.z);
		ship.rotation = Quaternion.Euler(tiltEuler);
	}

	// FireWeapon
	// Seems large, but that's because there are 4 different weapons
	// that are each handled differently.
	// Each weapon also separately calls this CoRoutine, meaning that
	// this can be running up to 4 times simultaneously.
	IEnumerator FireWeapon(int weaponId, float[] weaponInfo, Transform[] spawners) {
		float shotsPerSecond = weaponInfo[1];
		if (weaponId == 0 || weaponId == 3) {// guns or rockets
			shotsPerSecond *= weaponInfo[4];
		}
		int spawnIndex = 0;
		float rateOfFire = 1 / shotsPerSecond;
		while (firing && currentHeat < maxHeat) {
			if (weaponId == 0) {
				// GUNS
				SpawnProjectile(weaponId, weaponInfo, spawners[spawnIndex], 0);
				spawnIndex++;
				if (spawnIndex == weaponInfo[4]) {
					spawnIndex = 0;
				}
			}
			if (weaponId == 1) {
				// CANNON
				SpawnProjectile(weaponId, weaponInfo, spawners[0] , 0);
			}
			if (weaponId == 2) {
				// LASER - can split into multiple, so spawn multiple with offset if needed
				int countLasers = (int) weaponInfo[4] + 1;
				float offset = 0;
				if (countLasers == 1) {
					SpawnProjectile(weaponId, weaponInfo, Lasers[0], offset);
				} else if (countLasers == 2 || countLasers == 4) {
					offset = .4f;
				} else if (countLasers == 3 || countLasers == 5) {
					offset = 0.6f;
				}
				int sign = 1;
				if (countLasers != 1) {
					int startCount = 0;
					if (countLasers == 3 || countLasers == 5) {
						// center shot
						SpawnProjectile(weaponId, weaponInfo, Lasers[0], 0);
						startCount = 1;
					}
					for (int i = startCount; i < countLasers; i++) {
						// 0, 1, 2, 3, 4;
						int factor = 1;
						if (i > 2) {
							factor = 2;
						}
						SpawnProjectile(weaponId, weaponInfo, Lasers[0], factor * offset * sign);
						sign = -sign;
					}
				}
			}
			if (weaponId == 3) {
				// ROCKETS
				float offset = 1;
				if (spawnIndex == 1 || spawnIndex == 3) {
					offset = -1;
				}
				SpawnProjectile(weaponId, weaponInfo, spawners[spawnIndex], offset); // rockets
				spawnIndex++;
				if (spawnIndex == weaponInfo[4]) {
					spawnIndex = 0;
				}
			}
			yield return new WaitForSeconds(rateOfFire);
		}
	}

	// SpawnProjectile
	void SpawnProjectile(int id, float[] info, Transform spawner, float fireOffset) {
		GameObject prj = Instantiate(projectiles[id], spawner.position, Quaternion.identity, null) as GameObject;
		prj.GetComponent<ProjectileScript>().SetProperties(gameObject.tag, id, info, fireOffset);
		currentHeat += 0.1f;
	}

	// TakeDamage
	public void TakeDamage(float dmg) {
		if (dead) {
			return;
		}
		dmgTimer = 2f;
		float hpDmg = 0;
		if (currentShield > dmg) {
			currentShield -= dmg;
		} else {
			hpDmg = dmg - currentShield;
			hpDmg -= armor;
			if (hpDmg < 1) {
				hpDmg = 1;
			}
			currentShield = 0;
			currentHp -= hpDmg;
		}

		puis.UpdateHpValue(currentHp);
		if (currentHp < 0) {
			Death();
		} else if (maxShield > 0) {
			if (!shieldRegenerating) {
				StartCoroutine(ShieldRegen());
			}
		}
	}

	// Cooling
	// this runs when heat > 0 and firing has stopped
	IEnumerator Cooling() {
		float t = 1;
		while (t > 0 && !firing) {
			t -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (!firing && currentHeat > 0) {
			currentHeat -= heatRegen * Time.deltaTime;
			if (currentHeat < 0) {
				currentHeat = 0;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	// ShieldRegen
	// When shield is available and not max, this runs
	// when shield takes a hit, a pause is done before
	// recharging. 
	IEnumerator ShieldRegen() {
		shieldRegenerating = true;
		while (currentShield < maxShield) {
			if (dmgTimer > 0) {
				dmgTimer -= Time.deltaTime;
			} else {
				currentShield += shieldRegenRate * Time.deltaTime;
			}
			yield return new WaitForEndOfFrame();
		}
		currentShield = maxShield;
		shieldRegenerating = false;
	}

	// Death
	void Death() {
		dead = true;
		GameObject exp = Instantiate(deathExplosion, trans.position, Quaternion.identity, null) as GameObject;
		pms.ReturnToMenu();
		Destroy(exp, 2f);
		Destroy(this.gameObject, 0.1f);
	}
}
