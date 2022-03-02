using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform _spawnLocation;

        GameObject[] shipParts;
        int numberOfShipPartsFound;
        bool allShipPartsHaveBeenFound;

        bool levelCompleted;

        private void Awake()
        {
            Instantiate(_playerPrefab, _spawnLocation.position, Quaternion.identity);

            EventBroker.LevelExitReached += OnLevelExitReached;
            EventBroker.ShipPartFound += OnShipPartFound;
        }

        private void OnDestroy()
        {
            EventBroker.LevelExitReached -= OnLevelExitReached;
            EventBroker.ShipPartFound -= OnShipPartFound;
        }

        private void Start()
        {
            shipParts = GameObject.FindGameObjectsWithTag("ShipPart");
        }

        public void OnLevelExitReached()
        {
            print("Level exit reached");
            if (allShipPartsHaveBeenFound && !levelCompleted)
            {
                levelCompleted = true;
                print("Level exit reached and all ship parts have been found");
                EventBroker.CallLevelCompleted();
            }
        }

        private void OnShipPartFound()
        {
            numberOfShipPartsFound++;
            if (numberOfShipPartsFound == shipParts.Length)
            {
                allShipPartsHaveBeenFound = true;
                EventBroker.CallAllShipPartsFound();
            }
        }

    }
}
