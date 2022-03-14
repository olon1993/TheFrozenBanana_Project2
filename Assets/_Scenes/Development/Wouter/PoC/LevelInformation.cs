using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
	[SerializeField] private int levelId;
	[SerializeField] private int difficulty;
	// We can add more stuff here, perhaps reference to the player data...
	// if the ship part is collected, if all collectibles are collected. don't know..

	public int GetLevelId() {
		return levelId;
	}

	public int GetLevelDifficulty() {
		return difficulty;
	}
}
