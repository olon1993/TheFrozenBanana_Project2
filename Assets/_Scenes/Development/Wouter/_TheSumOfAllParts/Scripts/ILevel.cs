using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public interface ILevel {

		GameObject playerPrefab { get; }
		GameObject playerSpawnPoint { get; }
		GameObject[] collectibles { get; }
		GameObject levelCompleteTextBox { get; }
		bool levelAccessible { get; set; }
		bool levelComplete { get; set; }
		void StartupLevel();
		void Collect(GameObject name);
		void ExitLevel();
		void CreateTeleportEffect();
	}
}
