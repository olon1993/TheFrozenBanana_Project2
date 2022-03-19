using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class PlayerData : MonoBehaviour {

		// cannot add the actual ICollectible because after adding, once destroyed, 
		// the ICollectible added is removed creating a reference error
		private ICollectible.CollectibleType[][] allCollectibleTypes;
		private bool[][] allCollectibleStatus;
		private int playerDeaths;


		// DATA RETRIEVERS
		public bool[] RetrieveLevelObjectStatus(int level) {
			return allCollectibleStatus[level];
		}

		public ICollectible.CollectibleType[] RetrieveLevelObjectTypes(int level) {
			return allCollectibleTypes[level];
		}

		public bool[][] RetrieveAllStatus() {
			return allCollectibleStatus;
		}

		public ICollectible.CollectibleType[][] RetrieveAllTypes() {
			return allCollectibleTypes;
		}

		public int RetrieveDeaths() {
			return playerDeaths;
		}

		// UPDATES

		public void PlayerDied() {
			playerDeaths++;
		}

		public void UpdateCollectibles(int level, bool[] update) {
			allCollectibleStatus[level] = update;
		}


		/******************************************************/
		/*************CREATIONARY OF DATA**********************/
		/******************************************************/


		// Startup of game: creates all items to create library
		public void CreateCollectibleData() {
			allCollectibleTypes = new ICollectible.CollectibleType[0][];
			allCollectibleStatus = new bool[0][];

			GameObject[] allLevels = wGameManager.gm.RetrieveLevels();
			foreach (GameObject level in allLevels) {
				GameObject tmpLevel = Instantiate(level) as GameObject;
				CreateNewCollectibleArray(tmpLevel.GetComponent<ILevel>().collectibles);
				Destroy(tmpLevel, 1f);
			}
			/**TESTING COUNT**/
			int countCD = 0;
			int countPART = 0;
			int levelCount = 0;
			for (int i = 0; i < allCollectibleTypes.Length; i++) {
				levelCount++;
				for (int j = 0; j < allCollectibleTypes[i].Length; j++) {
					if (allCollectibleTypes[i][j] == ICollectible.CollectibleType.CD) {
						countCD++;
					} else if (allCollectibleTypes[i][j] == ICollectible.CollectibleType.SHIPPART) {
						countPART++;
					}
					
				}
			}
			Debug.Log("Total levels checked: " + levelCount);
			Debug.Log("Total CDs counted   : " + countCD);
			Debug.Log("Total PARTS counted : " + countPART);
		}

		private void CreateNewCollectibleArray(GameObject[] newCollectibles) {
			int id = NewLevelArray();
			foreach (GameObject collectible in newCollectibles) {
				if (collectible.GetComponent<ICollectible>().collectibleTypeDefinition == ICollectible.CollectibleType.CD ||
					collectible.GetComponent<ICollectible>().collectibleTypeDefinition == ICollectible.CollectibleType.SHIPPART) {
					AddType(id, collectible.GetComponent<ICollectible>().collectibleTypeDefinition);
					AddStatus(id, collectible.GetComponent<ICollectible>().collected);
				}
			}
		}
		
		// ARRAY FILLERS BELOW //

		private int NewLevelArray() {
			int newId = allCollectibleStatus.Length;
			int newLength = newId + 1;
			ICollectible.CollectibleType[][] tmpTypes = new ICollectible.CollectibleType[newLength][];
			bool[][] tmpStatus = new bool[newLength][];
			for (int i = 0; i < newId; i++) {
				tmpTypes[i] = allCollectibleTypes[i];
				tmpStatus[i] = allCollectibleStatus[i];
			}
			allCollectibleTypes = tmpTypes;
			allCollectibleStatus = tmpStatus;
			allCollectibleTypes[newId] = new ICollectible.CollectibleType[0];
			allCollectibleStatus[newId] = new bool[0];
			return newId;
		}

		private void AddType(int id, ICollectible.CollectibleType newType) {
			int newId = allCollectibleTypes[id].Length;
			int newLength = newId + 1;
			ICollectible.CollectibleType[] tmpTypes = new ICollectible.CollectibleType[newLength];
			for (int i = 0; i < newId; i++) {
				tmpTypes[i] = allCollectibleTypes[id][i];
			}
			tmpTypes[newId] = newType;
			allCollectibleTypes[id] = tmpTypes;
		}

		private void AddStatus(int id, bool newStatus) {
			int newId = allCollectibleStatus[id].Length;
			int newLength = newId + 1;
			bool[] tmpStatus = new bool[newLength];
			for (int i = 0; i < newId; i++) {
				tmpStatus[i] = allCollectibleStatus[id][i];
			}
			tmpStatus[newId] = newStatus;
			allCollectibleStatus[id] = tmpStatus;
			Debug.Log("Player data matrix now contains: [" + id + "," + newId +"]" );
		}
	}
}
