using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayManagementScript : MonoBehaviour
{

	/*******************************************************************
	 * PLAY MANAGER
	 * This is the script that does the work in a level. Before starting
	 * it checks to see if everything is ready. Once ready and the
	 * parameters have been given, the actual CoRoutine to run a level
	 * is run. Due to it being vital, there's also detailed commenting
	 * within the CoRoutine. 
	 * In short: spawns enemies, despawns/respawns where necessary,
	 * closes off the level once complete. 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	// editor setup variables
	public Transform[] spawnLocations;
	public GameObject[] enemyUnits;
	public float spawnTimer;
	public GameObject playerPrefab, lighting, playUIprefab;
	public Transform playerSpawnPoint;

	// private set variables
	private FlightPathLibrary fpl;
	private LevelLibrary ll;

	// private variables
	private PlayUIScript puis;
	private bool setupComplete, spawnEnemies; //, playerEndInput;
	private int level, cashEarned; 

	// private runtime level variables
	private int[][] currentEnemies, respawnEnemies;
	private int[][][] levelWaves;

	// public variables
	public bool victory;

	void Awake() {
		if (fpl == null) {
			fpl = gameObject.GetComponent<FlightPathLibrary>();
		}
		if (ll == null) {
			ll = gameObject.GetComponent<LevelLibrary>();
		}
	//	spawnEnemies = true;
		StartCoroutine(CheckLibraryComplete());
	}

	// SetupLevel - Spawn necessary components for the level
	//              setup variables
	public void SetupLevel(int lvl, int[][] shipProfile, ShipUpgradeLibrary sulRef, GameObject can) {
		level = lvl;
		Instantiate(lighting);
		GameObject playerShip = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
		GameObject ui = Instantiate(playUIprefab, can.transform) as GameObject;
		puis = ui.GetComponent<PlayUIScript>();
		playerShip.GetComponent<PlayerShipScript>().SetupShipProfile(this, shipProfile, sulRef, puis);

		setupComplete = true;
	}

	// As the libraries are important, only start the level once the 
	// libraries are guaranteed complete.
	// NOTE: might be redundant but to be sure, leave it like this (for now).
	IEnumerator CheckLibraryComplete() {
		while (!ll.libraryReady || !fpl.libraryReady) {
			yield return new WaitForEndOfFrame();
		}
		while (!setupComplete) {
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine(RunLevel(level)); 
	}

	// RunLevel - the brain of the level
	// Makes waves, spawns enemies, despawns enemies, respawns enemies, etc.
	IEnumerator RunLevel(int lvl) {
		victory = false;
		levelWaves = ll.levels[lvl];
		// levelWaves is as follows:
		//     - [i][j][k]
		//     - where 
		//     -    i -> wave index
		//          j -> ship-enemy index. NOTE: when j=0 -> its the timers:
		//               k =0 -> pause between waves, k=1 -> pause between individual spawn
		//          k -> when j!=0 -> k=0 -> enemy Ship, k=1 -> flight path
		int totalWaves = levelWaves.Length;
		float waveInterval = 0;  // wave interval is the time between waves, j=0; k=0
		float enemyInterval = 0; // enemy interval is the time between enemy units, j=0; k=1
		int enemyId = 0;         // is a random Id, but needed for respawn to work properly
		int enemyCount = 0;

		yield return new WaitForSeconds(5f);
		
		// Per wave, until all waves are complete: 
		for (int i = 0; i < totalWaves; i++) {
			int[][] wave = levelWaves[i];
			enemyCount = 0;
			waveInterval = ll.waveTimers[wave[0][0]];  // wave interval timer
			enemyInterval = ll.waveTimers[wave[0][1]]; // enemy unit interval timer
			currentEnemies = new int[0][];
			respawnEnemies = new int[0][];

			// initial spawn of enemies per wave
			for (int j = 1; j < wave.Length; j++) {
				EnemySpawner(wave[j][0], wave[j][1], enemyId, 0);
				enemyId++;
				enemyCount++;
				yield return new WaitForSeconds(enemyInterval);
			}

			// looping to wait when wave is finished
			// should non-boss enemies finish their flightpath
			// despawn these enemies, and respawn them!
			while (enemyCount > 0) {
				yield return new WaitForSeconds(1f);
				enemyCount = currentEnemies.Length + respawnEnemies.Length;
				if (currentEnemies.Length == 0 && respawnEnemies.Length != 0) {
					// Here is the respawn
					for (int x = 0; x < respawnEnemies.Length; x++) {
						EnemySpawner(respawnEnemies[x][0], respawnEnemies[x][1], respawnEnemies[x][2], respawnEnemies[x][3]);
						yield return new WaitForSeconds(enemyInterval);
					}
					respawnEnemies = new int[0][];
				} 
			}
			yield return new WaitForSeconds(waveInterval);
		}
		victory = true;
		ReturnToMenu();
	}

	// Adds to earned cash, is called when an enemy ship is destroyed
	public void AddEarnedCash(int cash) {
		cashEarned += cash;
		puis.UpdateCashText(cashEarned);
	}

	// TODO: starts the UI to show win or lose, waits for player to click to continue
	public void ReturnToMenu() {
		puis.ShowEndBox(victory);
		StartCoroutine(CloseLevel());
	}

	// TODO: is called from ui to wrap up the level
	public void EndInput() {
	//	playerEndInput = true;
	}

	// TODO: the input waiter before completely closing the level, gives the UI info
	private IEnumerator CloseLevel() {
		yield return new WaitForSeconds(2f);
		/* TODO: Turned off for test, turn on later
		while (!playerEndInput) {
			yield return new WaitForEndOfFrame();
		} */
		GameManagementScript.gms.FinishLevel(victory, cashEarned);
	}

	// EnemySpawner
	// This takes the information provided from two separate places.
	// Initially of course from the level/wave information. 
	// Secondly, from the respawner. 
	// Although not visible here, the startHp has a cool functionality:
	//		if it's 0, then an enemy is spawned with its max hp.
	//		if it's not 0 (from the respawner..) then this overrides the max hp
	void EnemySpawner(int enemyShipIndex, int flightPathIndex, int id, int startHp) {
		// Debug.Log("PlayManagementScript.EnemySpawner");
		Transform spawnIndex = fpl.FlightPaths[flightPathIndex][0];
		GameObject newEnemy = Instantiate(enemyUnits[enemyShipIndex], spawnIndex.position, Quaternion.identity, null) as GameObject;
		newEnemy.GetComponent<EnemyShipScript>().PushFlightInfo(flightPathIndex, id, startHp);
		AddEnemyToArray(ref currentEnemies, enemyShipIndex, flightPathIndex, id, startHp);
	}

	// Add Respawn
	// non-bosses that finish their flightpath end up behind the player, and cannot 
	// be shot. so, despawn them, and add them through this method.
	public void AddToRespawnEnemies(int enemyShipIndex, int flightPathIndex, int id, int respawnHp) {
		AddEnemyToArray(ref respawnEnemies, enemyShipIndex, flightPathIndex, id, respawnHp);
	}

	// Remove Enemy
	// This removes all enemies, killed or despawned after flight path.
	public void RemoveEnemyFromCurrentEnemies(GameObject enemyShip, int id) {
		int indexToRemove = -1;
		int tmpIndex = 0;
		int[][] tmpArray = new int[currentEnemies.Length-1][];
		for (int i = 0; i < currentEnemies.Length; i++) {
			if (currentEnemies[i][2] == id) {
				indexToRemove = i;
			} else {
				tmpArray[tmpIndex] = currentEnemies[i];
				tmpIndex++;
			}
		}
		if (indexToRemove != -1) {
			// Debug.Log("Enemy id: " + id + " removed from current enemies");
			currentEnemies = tmpArray;
		}
		Destroy(enemyShip, 0.2f);
	}
		 
	// used for current and respawn enemies (per wave in each level)
	// to be able to track them.
	void AddEnemyToArray(ref int[][] array, int shipIndex, int flightIndex, int id, int startHp) {
		int newLength = array.Length + 1;
		int[][] tmpArray = new int[newLength][];
		for (int i = 0; i < array.Length; i++) {
			tmpArray[i] = array[i];
		}
		tmpArray[newLength - 1] = new int[4];
		tmpArray[newLength - 1][0] = shipIndex;
		tmpArray[newLength - 1][1] = flightIndex;
		tmpArray[newLength - 1][2] = id;
		tmpArray[newLength - 1][3] = startHp;
		array = tmpArray;
	}


}
