using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class PlayerDataScript : MonoBehaviour
{
	/*******************************************************************
	 * PLAYER DATA SCRIPT
	 * This script does all the IO work, and is the only place where 
	 * mutations are done on the actual data. All information here is
	 * private.
	 * 
	 * The main functionality can be categorized into 6 functionalities:
	 *		SaveData - saves data to the local file. uses arrays:
	 *				allShipProfiles
	 *				allPlayerProgress
	 *		LoadData - loads data from the local file. uses arrays:
	 *				allShipProfiles
	 *				allPlayerProgress
	 *		SaveProfile - called after mutations to save a selected 
	 *					  profile.
	 *		LoadProfile - loads up a profile for mutations. 
	 *		Retrieves - gathers information regarding a selected profile
	 *		Mutations - changes information on the selected profile
	 * 
	 * Default Location
	 * C:\Users\<Username>\AppData\LocalLow\<CompanyName>\<ProjectName>\ppdata.dat
	 * My Location
	 * C:\Users\hiems\AppData\LocalLow\DefaultCompany\New Unity Project\ppdata.dat
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	private int[][][] allShipProfiles;
	private int[][]   allPlayerProgress;
	private int[][]   ppShipInfo;       // this is the current profile being used
	private int[]     ppPlayerProgress; // this is 2 ints, level progression and currency amt

	public void CreateNewSaveFile() {
		int[][][] newShipProfiles = new int[3][][];
		newShipProfiles[0] = NewShipProfile();
		newShipProfiles[1] = NewShipProfile();
		newShipProfiles[2] = NewShipProfile();

		allShipProfiles = newShipProfiles;

		int[][] newPlayerProgress = new int[3][];
		newPlayerProgress[0] = NewPlayerProgress();
		newPlayerProgress[1] = NewPlayerProgress();
		newPlayerProgress[2] = NewPlayerProgress();

		allPlayerProgress = newPlayerProgress;

		SaveData();
	}



	// SaveData - Called internally only.
	private void SaveData() {
		if (!File.Exists(Application.persistentDataPath + "/ppdata.dat")) {
			FileStream file = File.Create(Application.persistentDataPath + "/ppdata.dat");
			file.Close();
		}
		// startup formatter and open file.
		BinaryFormatter bf = new BinaryFormatter();
		FileStream ioFile = File.Open(Application.persistentDataPath + "/ppdata.dat", FileMode.Open);

		// now, to save the data, the data to be saved needs to be serialized. 
		// this means adding all the necessary data into the PlayerData class.
		PlayerData data = new PlayerData();

		// now add the necessary information into playerdata by assigning it to variables
		data.allProfilesShipData = allShipProfiles;
		data.allProfilesPlayerProgress = allPlayerProgress;

		// then serialize the data
		bf.Serialize(ioFile, data);

		// after opening a file, close the file to prevent locking issues
		ioFile.Close();
	}

	// LoadData - called from Game Management
	public bool LoadData() {
		bool fileFound = false;
		if (!File.Exists(Application.persistentDataPath + "/ppdata.dat")) {
			return fileFound;
		} else {

			fileFound = true;
		}
		// startup formatter and open file.
		BinaryFormatter bf = new BinaryFormatter();
		FileStream ioFile = File.Open(Application.persistentDataPath + "/ppdata.dat", FileMode.Open);

		// deserialize the information in order to read it
		PlayerData data = (PlayerData)bf.Deserialize(ioFile);
	
		// load up the profile from the data
		allShipProfiles = data.allProfilesShipData;
		allPlayerProgress = data.allProfilesPlayerProgress;

		// after opening a file, close the file to prevent locking issues
		ioFile.Close();
		return fileFound;
	}

	// LoadProfile - called internally only to setup for mutations
	private void LoadProfile(int i) {
		if (i < allShipProfiles.Length) {
			ppShipInfo = allShipProfiles[i];
			ppPlayerProgress = allPlayerProgress[i];
		}
	}

	// SaveProfile - saves mutations into the array and then the file
	private void SaveProfile(int i) {
		allShipProfiles[i] = ppShipInfo;
		allPlayerProgress[i] = ppPlayerProgress;
		SaveData();
	}

	// Mutation - Purchase -> called to edit a ship item value and remove
	//						  used funds
	public void UpdateProfileForPurchase(int profile, int item, int index, int cost) {
		LoadProfile(profile);
		ppShipInfo[item][index]++;
		ppPlayerProgress[1] -= cost;
		SaveProfile(profile);
	}

	// Mutation - Level ->  called after a level is finish to change 
	//						player progress, win or lose. 
	public void UpddateProfileForLevel(int profile, int cash, bool complete) {
		LoadProfile(profile);
		ppPlayerProgress[1] += cash;
		if (complete) {
			ppPlayerProgress[0]++;
		}
		SaveProfile(profile);
	}

	// Retrieve - Progress
	public int[] GetProfileProgress(int id) {
		int[] tmpArray = allPlayerProgress[id];
		return tmpArray;
	}

	// Retrieve - ShipValues
	public int[][] GetProfileShip(int id) {
		int[][] tmpArray = allShipProfiles[id];
		return tmpArray;
	}

	// BELOW IS FOR NEW PROFILES ONLY

	// This is called internally only
	private int[][] NewShipProfile() {
		int[][] newShipInfo = new int[8][];

		// Guns
		newShipInfo[0] = new int[5];
		newShipInfo[0][0] = 1; // max 1  - unlock				- 1
		newShipInfo[0][1] = 0; // max 10 - shots per second		- 0
		newShipInfo[0][2] = 0; // max 10 - projectile speed		- 0
		newShipInfo[0][3] = 0; // max 10 - damage				- 0
		newShipInfo[0][4] = 2; // max 4  - guns					- 2

		// Cannon
		newShipInfo[1] = new int[5];
		newShipInfo[1][0] = 0; // max 1  - unlock				- 0
		newShipInfo[1][1] = 0; // max 10 - shots per second		- 0
		newShipInfo[1][2] = 0; // max 10 - projectile speed		- 0
		newShipInfo[1][3] = 0; // max 10 - damage				- 0
		newShipInfo[1][4] = 0; // max 4  - explosive area		- 0

		// Lasers
		newShipInfo[2] = new int[5];
		newShipInfo[2][0] = 0; // max 1  - unlock				- 0
		newShipInfo[2][1] = 0; // max 10 - shots per second		- 0
		newShipInfo[2][2] = 0; // max 10 - projectile speed		- 0
		newShipInfo[2][3] = 0; // max 10 - damage				- 0
		newShipInfo[2][4] = 0; // max 4  - splits				- 0

		// Rockets
		newShipInfo[3] = new int[5];
		newShipInfo[3][0] = 0; // max 1  - unlock				- 0
		newShipInfo[3][1] = 0; // max 10 - shots per second		- 0
		newShipInfo[3][2] = 0; // max 10 - projectile speed		- 0
		newShipInfo[3][3] = 0; // max 10 - damage				- 0
		newShipInfo[3][4] = 2; // max 4  - rockets amount		- 2

		// Ship Structure
		newShipInfo[4] = new int[3];
		newShipInfo[4][0] = 1; // max 1  - unlock
		newShipInfo[4][1] = 0; // max 10 - Armor
		newShipInfo[4][2] = 0; // max 10 - HP

		// Shields
		newShipInfo[5] = new int[3];
		newShipInfo[5][0] = 1; // max 1  - unlock
		newShipInfo[5][1] = 0; // max 10 - Max Shield
		newShipInfo[5][2] = 0; // max 10 - Shield regen

		// Engine
		newShipInfo[6] = new int[3];
		newShipInfo[6][0] = 1; // max 1  - unlock
		newShipInfo[6][1] = 0; // max 10 - Mission time
		newShipInfo[6][2] = 0; // max 10 - Speed

		// Coolers
		newShipInfo[7] = new int[3];
		newShipInfo[7][0] = 1; // max 1  - unlock
		newShipInfo[7][1] = 0; // max 10 - Total heat
		newShipInfo[7][2] = 0; // max 10 - heat cooldown

		return newShipInfo;
	}

	// this is called internally only
	private int[] NewPlayerProgress() {
		int[] otherInfo = new int[2];
		otherInfo[0] = 0; // 0
		otherInfo[1] = 0; // 0
		return otherInfo;
	}
}

// The SERIALIZABLE Class PlayerData is what is actually
// stored in a binary file. 

[Serializable]
class PlayerData {
	public int[][][] allProfilesShipData;
	public int[][]   allProfilesPlayerProgress;
}
