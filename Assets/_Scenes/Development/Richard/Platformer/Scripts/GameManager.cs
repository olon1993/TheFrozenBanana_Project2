using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheFrozenBanana
{
    public class GameManager : Singleton<GameManager>
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] String[] LEVEL_SCENE_NAMES;
        [SerializeField] String WIN_SCENE_NAME;
        [SerializeField] String GAME_OVER_SCENE_NAME;
        [SerializeField] String HUB_SCENE_NAME;
        [SerializeField] String START_MENU_SCENE_NAME;

        private int _numberOfLevels;

        private int _currentLevel = 1;

        [SerializeField] Animator transitionAnimator;
        [SerializeField] string fadeOutTransitionTriggerName;
        [SerializeField] string fadeInTransitionTriggerName;

        private float sceneTransitionTime = 1f;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            EventBroker.LevelCompleted += OnLevelCompleted;
            EventBroker.PlayerDeath += OnPlayerDeath;

            if (LEVEL_SCENE_NAMES.Length != 0)
            {
                _numberOfLevels = LEVEL_SCENE_NAMES.Length;
            }
            else
            {
                Debug.LogError(name + ": No scene names available to load");
            }
        }

        protected override void OnDestroy()
        {
            EventBroker.LevelCompleted -= OnLevelCompleted;
            EventBroker.PlayerDeath -= OnPlayerDeath;
        }

        public void StartGameFromBeginning()
        {
            _currentLevel = 1;
            LoadLevel(_currentLevel);
        }

        public void LoadCurrentLevel()
        {
            LoadLevel(_currentLevel);
        }

        public void LoadLevel(int levelNumber)
        {
            StartCoroutine(LoadSceneCoroutine(LEVEL_SCENE_NAMES[levelNumber - 1]));
        }

        IEnumerator LoadSceneCoroutine(string sceneName)
        {
            transitionAnimator.SetTrigger(fadeOutTransitionTriggerName);

            yield return new WaitForSeconds(sceneTransitionTime);

            try
            {
                SceneManager.LoadSceneAsync(sceneName);
            }
            catch
            {
                Debug.LogError("Game Manager: Unable to load scene " + sceneName);
            }

            transitionAnimator.SetTrigger(fadeInTransitionTriggerName);
        }

        private void OnLevelCompleted()
        {
            print("Game Manager: Level completed");
            _currentLevel++;
            if (_currentLevel <= _numberOfLevels)
            {
                LoadHubScene();
            }
            else
            {
                LoadWinScene();
            }
        }

        private void LoadWinScene()
        {
            StartCoroutine(LoadSceneCoroutine(WIN_SCENE_NAME));
        }

        private void OnPlayerDeath()
        {
            StartCoroutine(LoadSceneCoroutine(GAME_OVER_SCENE_NAME));
        }

        public void LoadHubScene()
        {
            StartCoroutine(LoadSceneCoroutine(HUB_SCENE_NAME));
        }

        public void ReloadGame()
        {
            StartCoroutine(LoadSceneCoroutine(START_MENU_SCENE_NAME));
        }


        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\
        public int CurrentLevel
        {
            get { return _currentLevel; }
        }
    }
}

