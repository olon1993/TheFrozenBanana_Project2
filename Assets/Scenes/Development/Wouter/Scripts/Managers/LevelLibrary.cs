using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelLibrary : MonoBehaviour
{

	/*******************************************************************
	 * LEVEL LIBRARY
	 * The level library is where all levels are placed into coding.
	 * The structure is as follows:
	 *		levels[a][b][c][d]
	 *			- a: level id   - used to start up levels
	 *			- b: wave id    - splits levels into waves of enemies
	 *			- c: wave index - id of ships to spawn
	 *				NOTE: when [c] = 0 -> contains wave timer info
	 *				      when [c] > 0 -> contains enemy info
	 *		    - d: enemy info - this is simply made up into 2 ints
	 *							  no more is needed for functionality
	 *				NOTE: when [c] = 0 -> it's wave timers.
	 *						   [d] = 0 -> interval between waves
	 *						   [d] = 1 -> interval between spawns
	 *					             these are indices, switched to 
	 *					             float using waveTimers array. 
	 *					       [c] > 1 -> it's ship info
	 *					       [d] = 0 -> ship index, converted in play
	 *					             script with Enemy Units array
	 *					       [d] = 1 -> flight path, converted in play
	 *					             script referencing flight path lib
	 * 
	 * Script is setup into two parts.
	 * PART I
	 * All the basic functionality: Array management
	 * 
	 * PART II
	 * Build up of all levels. 
	 * 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Initial testing with one level
	 *	0.2     WH  Added a bunch of levels for system testing. 
	 *******************************************************************/
	// editor setup variables
	public float[] waveTimers; // seems specific, but is only used to revert to a float time for [][][0][1] of levels

	// private variables
	private int[][][] waveIndexer; // This is the game's wave library, contains per wave: enemies, and spawn timers

	// public variables
	public int[][][][] levels;    // [level] [wave] [wave index] [wave info: row-0 -> timers; rows-1+ -> ships]
	public bool libraryReady = false;

	void Awake() {
		CreateLevelDictionary();
		libraryReady = true;
    }

	void AddIntsToWave(ref int[][] array, int intA, int intB) {
		// Debug.Log("LevelLibrary.AddIntsToWave");
		try {
			int[] ints = new int[2];
			ints[0] = intA;
			ints[1] = intB;
			AddIntArrayToWave(ref array, ints);
		} catch (Exception e) {
			Debug.Log("Error on AddIntsToWave: " + e);
		}
	}

	void AddIntArrayToWave(ref int[][] array, int[] shipAndPath) {
		// Debug.Log("LevelLibrary.AddIntArrayToWave");
		try {
			int newLength = array.Length + 1;
			int[][] tmpArray = new int[newLength][];
			for (int i = 0; i < array.Length; i++) {
				tmpArray[i] = array[i];
			}
			tmpArray[newLength - 1] = shipAndPath;
			array = tmpArray;
		} catch (Exception e) {
			Debug.Log("Error on AddIntArrayToWave: " + e);
		}
	}

	void AddWaveToLevel(ref int[][][] level, int [][] wave) {
		// Debug.Log("LevelLibrary.AddWaveToLevel");
		try {
			int newLength = level.Length + 1;
			int[][][] tmpArray = new int[newLength][][];
			
			for (int i = 0; i < level.Length; i++) {
				tmpArray[i] = level[i];
			}
			tmpArray[newLength - 1] = wave;
			level = tmpArray;

		} catch (Exception e) {
			Debug.Log("Error on AddWaveToLevel: "+ e);
		}
	}

	void AddLevelToLibrary(ref int[][][][] array, int[][][] wavesLevel) {
		// Debug.Log("LevelLibrary.AddLevelToLibrary");
		try {
			int newLength = array.Length + 1;
			int[][][][] tmpArray = new int[newLength][][][];

			for (int i = 0; i < array.Length; i++) {
				tmpArray[i] = array[i];
			}
			tmpArray[newLength - 1] = wavesLevel;
			array = tmpArray;

		} catch (Exception e) {
			Debug.Log("Error on AddWaveToLevel: " + e);
		}
	}

	void CreateLevelDictionary() {

		levels = new int[0][][][];
		CreateLevelOne();
		CreateLevelTwo();
		CreateLevelThree();
		CreateLevelFour();
		CreateLevelFive();
		CreateLevelSix();
		CreateLevelSeven();
		CreateLevelEight();
		CreateLevelNine();
		CreateLevelTen();
		CreateLevelEleven();
		CreateLevelTwelve();
		CreateLevelThirteen();
		CreateLevelFourteen();
		CreateLevelFifteen();
		CreateLevelSixteen();
		CreateLevelSeventeen();
		CreateLevelEighteen();
	}


	/***********************************************************
	 * LEVELS
	 *   - here we have the levels built up
	 *   - build per wave
	 *   - each wave first needs a timer on [0][]
	 *   - each successive part is for an enemy unit
	 *   - once a wave is complete, add the wave to the indexer
	 *   - once a level is complete, add the waves to the levels 
	 ***********************************************************/

	void CreateLevelOne() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 0);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 0, 1);
		AddIntsToWave(ref waveB, 0, 1);
		AddIntsToWave(ref waveB, 0, 1);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 3);
		AddIntsToWave(ref waveC, 0, 0);
		AddIntsToWave(ref waveC, 0, 1);
		AddIntsToWave(ref waveC, 0, 0);
		AddIntsToWave(ref waveC, 0, 1);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelTwo() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 2);
		AddIntsToWave(ref waveA, 0, 2);
		AddIntsToWave(ref waveA, 0, 3);
		AddIntsToWave(ref waveA, 0, 2);
		AddIntsToWave(ref waveA, 0, 3);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 3, 2);
		AddIntsToWave(ref waveB, 0, 2);
		AddIntsToWave(ref waveB, 0, 3);
		AddIntsToWave(ref waveB, 0, 4);
		AddIntsToWave(ref waveB, 0, 5);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 2);
		AddIntsToWave(ref waveC, 0, 0);
		AddIntsToWave(ref waveC, 0, 1);
		AddIntsToWave(ref waveC, 0, 6);
		AddIntsToWave(ref waveC, 0, 7);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);

	}

	void CreateLevelThree() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 1);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 0, 2);
		AddIntsToWave(ref waveB, 0, 3);
		AddIntsToWave(ref waveB, 1, 6);
		AddIntsToWave(ref waveB, 1, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 3, 3);
		AddIntsToWave(ref waveC, 8, 12);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelFour() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 4);
		AddIntsToWave(ref waveA, 1, 0);
		AddIntsToWave(ref waveA, 1, 1);
		AddIntsToWave(ref waveA, 1, 0);
		AddIntsToWave(ref waveA, 1, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 1, 2);
		AddIntsToWave(ref waveB, 1, 3);
		AddIntsToWave(ref waveB, 1, 2);
		AddIntsToWave(ref waveB, 1, 3);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 2);
		AddIntsToWave(ref waveC, 1, 0);
		AddIntsToWave(ref waveC, 1, 1);
		AddIntsToWave(ref waveC, 1, 2);
		AddIntsToWave(ref waveC, 1, 3);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}
	
	void CreateLevelFive() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 2, 8);
		AddIntsToWave(ref waveA, 2, 9);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 1, 10);
		AddIntsToWave(ref waveB, 1, 11);
		AddIntsToWave(ref waveB, 1, 6);
		AddIntsToWave(ref waveB, 1, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 5, 1);
		AddIntsToWave(ref waveC, 2, 2);
		AddIntsToWave(ref waveC, 2, 3);
		AddIntsToWave(ref waveC, 1, 8);
		AddIntsToWave(ref waveC, 1, 9);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelSix() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 4);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 1);
		AddIntsToWave(ref waveA, 0, 8);
		AddIntsToWave(ref waveA, 0, 9);
		AddIntsToWave(ref waveA, 1, 10);
		AddIntsToWave(ref waveA, 1, 11);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 2, 2);
		AddIntsToWave(ref waveB, 2, 3);
		AddIntsToWave(ref waveB, 2, 6);
		AddIntsToWave(ref waveB, 2, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 2, 2);
		AddIntsToWave(ref waveC, 9, 13);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelSeven() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 4);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 1);
		AddIntsToWave(ref waveA, 1, 0);
		AddIntsToWave(ref waveA, 1, 1);
		AddIntsToWave(ref waveA, 2, 0);
		AddIntsToWave(ref waveA, 2, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 2, 2);
		AddIntsToWave(ref waveB, 2, 3);
		AddIntsToWave(ref waveB, 3, 4);
		AddIntsToWave(ref waveB, 3, 5);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 4);
		AddIntsToWave(ref waveC, 4, 8);
		AddIntsToWave(ref waveC, 4, 9);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelEight() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 3);
		AddIntsToWave(ref waveA, 0, 0);
		AddIntsToWave(ref waveA, 0, 1);
		AddIntsToWave(ref waveA, 1, 0);
		AddIntsToWave(ref waveA, 1, 1);
		AddIntsToWave(ref waveA, 2, 0);
		AddIntsToWave(ref waveA, 2, 1);
		AddIntsToWave(ref waveA, 3, 2);
		AddIntsToWave(ref waveA, 3, 3);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 4, 3);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 3, 3);
		AddIntsToWave(ref waveC, 3, 0);
		AddIntsToWave(ref waveC, 3, 1);
		AddIntsToWave(ref waveC, 3, 2);
		AddIntsToWave(ref waveC, 3, 3);
		AddWaveToLevel(ref waveIndexer, waveC);

		int[][] waveD = new int[0][];
		AddIntsToWave(ref waveD, 3, 2);
		AddIntsToWave(ref waveD, 4, 6);
		AddIntsToWave(ref waveD, 4, 7);
		AddWaveToLevel(ref waveIndexer, waveD);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelNine() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 1);
		AddIntsToWave(ref waveA, 4, 8);
		AddIntsToWave(ref waveA, 4, 9);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 3, 0);
		AddIntsToWave(ref waveB, 3, 0);
		AddIntsToWave(ref waveB, 3, 0);
		AddIntsToWave(ref waveB, 3, 0);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 3);
		AddIntsToWave(ref waveC, 3, 1);
		AddIntsToWave(ref waveC, 3, 1);
		AddIntsToWave(ref waveC, 3, 1);
		AddIntsToWave(ref waveC, 3, 1);
		AddWaveToLevel(ref waveIndexer, waveC);

		int[][] waveD = new int[0][];
		AddIntsToWave(ref waveD, 3, 3);
		AddIntsToWave(ref waveD, 10, 14);
		AddWaveToLevel(ref waveIndexer, waveD);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelTen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 4, 0);
		AddIntsToWave(ref waveA, 4, 0);
		AddIntsToWave(ref waveA, 4, 0);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 4, 1);
		AddIntsToWave(ref waveB, 4, 1);
		AddIntsToWave(ref waveB, 4, 1);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 3);
		AddIntsToWave(ref waveC, 5, 0);
		AddIntsToWave(ref waveC, 5, 1);
		AddIntsToWave(ref waveC, 5, 0);
		AddIntsToWave(ref waveC, 5, 1);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelEleven() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 2);
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 4, 3);
		AddIntsToWave(ref waveA, 5, 2);
		AddIntsToWave(ref waveA, 5, 3);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 3, 2);
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 4, 4);
		AddIntsToWave(ref waveB, 5, 5);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 2);
		AddIntsToWave(ref waveC, 5, 0);
		AddIntsToWave(ref waveC, 5, 1);
		AddIntsToWave(ref waveC, 5, 6);
		AddIntsToWave(ref waveC, 5, 7);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelTwelve() {

		waveIndexer = new int[0][][];
		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 4, 0);
		AddIntsToWave(ref waveA, 4, 1);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 5, 2);
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 5, 6);
		AddIntsToWave(ref waveB, 5, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 3, 3);
		AddIntsToWave(ref waveC, 11, 12);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelThirteen() {


		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 4);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddIntsToWave(ref waveA, 4, 0);
		AddIntsToWave(ref waveA, 4, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 5, 8);
		AddIntsToWave(ref waveB, 5, 9);
		AddIntsToWave(ref waveB, 6, 2);
		AddIntsToWave(ref waveB, 6, 3);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 2);
		AddIntsToWave(ref waveC, 4, 0);
		AddIntsToWave(ref waveC, 4, 1);
		AddIntsToWave(ref waveC, 5, 2);
		AddIntsToWave(ref waveC, 5, 3);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelFourteen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 4, 2);
		AddIntsToWave(ref waveA, 2, 8);
		AddIntsToWave(ref waveA, 2, 9);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 4, 10);
		AddIntsToWave(ref waveB, 4, 11);
		AddIntsToWave(ref waveB, 5, 6);
		AddIntsToWave(ref waveB, 5, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 5, 1);
		AddIntsToWave(ref waveC, 5, 2);
		AddIntsToWave(ref waveC, 5, 3);
		AddIntsToWave(ref waveC, 5, 8);
		AddIntsToWave(ref waveC, 5, 9);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelFifteen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 4);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddIntsToWave(ref waveA, 5, 8);
		AddIntsToWave(ref waveA, 5, 9);
		AddIntsToWave(ref waveA, 6, 10);
		AddIntsToWave(ref waveA, 6, 11);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 5, 2);
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 5, 6);
		AddIntsToWave(ref waveB, 5, 7);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 2, 2);
		AddIntsToWave(ref waveC, 12, 13);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelSixteen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 4);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddIntsToWave(ref waveA, 6, 0);
		AddIntsToWave(ref waveA, 6, 1);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 3);
		AddIntsToWave(ref waveB, 5, 2);
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 6, 4);
		AddIntsToWave(ref waveB, 6, 5);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 4);
		AddIntsToWave(ref waveC, 7, 8);
		AddIntsToWave(ref waveC, 7, 9);
		AddWaveToLevel(ref waveIndexer, waveC);

		AddLevelToLibrary(ref levels, waveIndexer);
	}

	void CreateLevelSeventeen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 5, 3);
		AddIntsToWave(ref waveA, 5, 0);
		AddIntsToWave(ref waveA, 5, 1);
		AddIntsToWave(ref waveA, 6, 0);
		AddIntsToWave(ref waveA, 6, 1);
		AddIntsToWave(ref waveA, 6, 0);
		AddIntsToWave(ref waveA, 6, 1);
		AddIntsToWave(ref waveA, 7, 2);
		AddIntsToWave(ref waveA, 7, 3);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 5, 3);
		AddIntsToWave(ref waveB, 7, 2);
		AddIntsToWave(ref waveB, 7, 3);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 3, 3);
		AddIntsToWave(ref waveC, 6, 0);
		AddIntsToWave(ref waveC, 6, 1);
		AddIntsToWave(ref waveC, 6, 2);
		AddIntsToWave(ref waveC, 6, 3);
		AddWaveToLevel(ref waveIndexer, waveC);

		int[][] waveD = new int[0][];
		AddIntsToWave(ref waveD, 3, 2);
		AddIntsToWave(ref waveD, 6, 0);
		AddIntsToWave(ref waveD, 6, 1);
		AddIntsToWave(ref waveD, 6, 6);
		AddIntsToWave(ref waveD, 6, 7);
		AddWaveToLevel(ref waveIndexer, waveD);

		int[][] waveE = new int[0][];
		AddIntsToWave(ref waveE, 3, 3);
		AddIntsToWave(ref waveE, 8, 12);
		AddIntsToWave(ref waveE, 9, 13);
		AddWaveToLevel(ref waveIndexer, waveE);

		AddLevelToLibrary(ref levels, waveIndexer);

	}

	void CreateLevelEighteen() {

		waveIndexer = new int[0][][];

		int[][] waveA = new int[0][];
		AddIntsToWave(ref waveA, 3, 1);
		AddIntsToWave(ref waveA, 7, 8);
		AddIntsToWave(ref waveA, 7, 9);
		AddWaveToLevel(ref waveIndexer, waveA);

		int[][] waveB = new int[0][];
		AddIntsToWave(ref waveB, 4, 2);
		AddIntsToWave(ref waveB, 7, 0);
		AddIntsToWave(ref waveB, 7, 0);
		AddIntsToWave(ref waveB, 7, 0);
		AddIntsToWave(ref waveB, 7, 0);
		AddWaveToLevel(ref waveIndexer, waveB);

		int[][] waveC = new int[0][];
		AddIntsToWave(ref waveC, 4, 3);
		AddIntsToWave(ref waveC, 7, 1);
		AddIntsToWave(ref waveC, 7, 1);
		AddIntsToWave(ref waveC, 7, 1);
		AddIntsToWave(ref waveC, 7, 1);
		AddWaveToLevel(ref waveIndexer, waveC);

		int[][] waveD = new int[0][];
		AddIntsToWave(ref waveD, 3, 3);
		AddIntsToWave(ref waveD, 13, 14);
		AddWaveToLevel(ref waveIndexer, waveD);

		AddLevelToLibrary(ref levels, waveIndexer);

	}
}
