using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TheFrozenBanana
{
    public class LevelManager : MonoBehaviour
    {
        // Player
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform _spawnLocation;

        // Collectables
        [SerializeField] TextMeshProUGUI _shipPartsCountText;
        private GameObject[] shipParts;
        private int numberOfShipPartsFound;
        private bool allShipPartsHaveBeenFound;

        // Music
        [SerializeField] AudioClip _levelMusic;

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
            UpdateUI();
            AudioEvents.CallPlaySoundClip(_levelMusic);
        }

        private void UpdateUI()
        {
            if(_shipPartsCountText != null)
            _shipPartsCountText.text = numberOfShipPartsFound + "/" + shipParts.Length;
        }

        private void OnLevelExitReached()
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
            UpdateUI();
            if (numberOfShipPartsFound == shipParts.Length)
            {
                allShipPartsHaveBeenFound = true;
                EventBroker.CallAllShipPartsFound();
            }
        }

    }
}
