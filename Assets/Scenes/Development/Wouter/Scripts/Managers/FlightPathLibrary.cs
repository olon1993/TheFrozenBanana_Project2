using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlightPathLibrary : MonoBehaviour
{
	/*******************************************************************
	 * FLIGHT PATH LIBRARY
	 * The flight path library provides a guided track for enemies. By
	 * storing all of these in a simple transform[][] the library can
	 * be used to give an iteration of flight points [FlightMarkers]
	 * for enemies to fly to one by one, as well as where the enemy
	 * spawns from. 
	 * FYI
	 * All markers are noted as the following grid (S-> Spawn):
	 * 
	 *	S1	S0	S2
	 *	 1	 0	 2	Edge markers
	 * ------------	Screen border
	 *	 4	 3	 5
	 *	 7	 6	 8
	 *	10	 9	11
	 *	13	12	14
	 *	16	15	17  Center screen
	 *	19	18	20
	 *	22	21	23
	 *	25	24	26
	 *	28	27	29
	 * ------------ Screen border
	 *	31	30	32
	 *	34	33	35
	 * 
	 * NOTE
	 * Although not handled in this script but in EnemyShipScript, 
	 * there is a difference between bosses and non-bosses. bosses go
	 * back and forth between locations, whereas non-bosses do the 
	 * entire flightpath once, despawn, and if not killed, respawn to 
	 * do their flight path again. 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	// editor setup variables
	public Transform[] Spawners, FlightMarkers;

	// private set variables
	public Transform[][] FlightPaths;

	// public variables
	public bool libraryReady = false;

	void Awake() {
		FlightPaths = new Transform[0][];
		SetFlightPaths();
		libraryReady = true;
	}

	// Add flight point adds a single point to a flight path
	public void AddFlightPoint(ref Transform[] array, Transform add) {
		// Debug.Log("FlightPathLibrary.AddFlightPoint");
		try {
			int newLength = array.Length + 1;
			Transform[] tmpArray = new Transform[newLength];
			for (int i = 0; i < array.Length; i++) {
				tmpArray[i] = array[i];
			}
			tmpArray[newLength - 1] = add;
			array = tmpArray;
		} catch (Exception e) {
			Debug.Log(e);
		}
	}

	// Add flight path adds the complete flight path (all points)
	// to the flight path library
	public void AddFlightPath(ref Transform[][] array, Transform[] add) {
		// Debug.Log("FlightPathLibrary.AddFlightPath");
		try {
			int newLength = array.Length + 1;
			Transform[][] tmpArray = new Transform[newLength][];
			for (int i = 0; i < array.Length; i++) {
				tmpArray[i] = array[i];
			}
			tmpArray[newLength - 1] = add;
			array = tmpArray;
		} catch (Exception e) {
			Debug.Log(e);
		}
	}

	// Set flight paths creates all the flight paths in order
	// See excel for library
	void SetFlightPaths() {
		// flightpath i-0 : simple strafe left to right
		Transform[] FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[1]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[4]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[10]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[20]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 1 : simple strafe right to left
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[2]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[5]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[11]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[19]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 2 : from center, dodge to left
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[3]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[9]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[13]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 3 : from center, dodge to right
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[3]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[9]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[14]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 4 : from center, psyche to right
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[3]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[9]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[13]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[20]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 5 : from center, psyche to left
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[3]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[9]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[14]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[19]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 6 : left start, go back, then right
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[1]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[7]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[13]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[22]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[14]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);
		// flightpath i- 7 : right start, go back, then left
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[2]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[8]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[14]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[23]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[13]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 8 : left hesitate
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[1]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[7]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[16]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[10]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i- 9 : right hesitate
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[2]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[8]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[17]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[11]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i-10 : left, pause, strafe right
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[1]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[4]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[10]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[7]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[17]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[35]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i-11 : right, pause, strafe left
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[2]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[5]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[11]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[8]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[16]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[34]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i-12 : boss flight path, sway back and forth
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[6]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[4]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[6]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[5]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i-13 : boss flight path, sway back and forth and backwards
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[6]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[7]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[8]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[4]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[5]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);

		// flightpath i-14 : boss flight path, forward and back, sway back and forth
		FlightPathTmp = new Transform[0];
		AddFlightPoint(ref FlightPathTmp, Spawners[0]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[3]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[10]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[11]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[7]);
		AddFlightPoint(ref FlightPathTmp, FlightMarkers[8]);
		AddFlightPath(ref FlightPaths, FlightPathTmp);
	}
}
