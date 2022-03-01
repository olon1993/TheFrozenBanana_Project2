using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
	[SerializeField] private int levelId;
	// We can add more stuff here, perhaps reference to the player data...
	// if the ship part is collected, if all collectibles are collected. don't know..

	public int GetLevelId() {
		Debug.Log("This script is where the information can be placed from player data.");
		return levelId;
	}
}
