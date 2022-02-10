using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagementScript : MonoBehaviour
{

	/*******************************************************************
	 *  GAME MANAGER
	 *  This is the engine of the game. It starts everything up, 
	 *  switches between scenes, and does all the player data 
	 *  manipulation for progress, upgrades and cash.
	 *
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	// static variables : DO NOT DESTROY!!!
	public static GameObject gm; 
	public static GameManagementScript gms;
	public static LevelLibrary ll;
	public static ShipUpgradeLibrary sul;
	public static PlayerDataScript pds;

	// editor set variables
	public GameObject gameLibrary; 
	public GameObject starsBackground;
	public GameObject canvasObject;
	public GameObject mainMenu;
	public GameObject playManager;
	
	// private variables

	private GameObject canvas;
	private int profileId = -1;

	// On awake checks if the game management script has already been
	// made. if not, make this the main. if it has been made already
	// destroy this one.
    void Awake() {
		if (gms != null) {
			Destroy(this);
			return;
		}
		gm = this.gameObject;
		gms = this.gameObject.GetComponent<GameManagementScript>();
		DontDestroyOnLoad(this);

		StartCoroutine(LoadGame());
    }

	// called externally to set the profile being used
	public void SetProfileId(int id) {
		profileId = id;
	}

	// called to check which profile is being used
	public int GetProfileId() {
		return profileId;
	}

	// called externally to retrieve the level library
	public LevelLibrary GetLevelLibrary() {
		return ll;
	}

	// play level is called externally to get the game to
	// start the level
	public void PlayLevel() {
		int level = pds.GetProfileProgress(profileId)[0];
		int[][] psp = pds.GetProfileShip(profileId);
		StartCoroutine(LoadLevel(level, psp));
	}

	// finish level wraps up the level scene and updates
	// the profile with the finish info.
	public void FinishLevel(bool win, int cash) {
		pds.UpddateProfileForLevel(profileId, cash, win);
		StartCoroutine(ReturnToMenu());
	}

	// the upgrade information is given through this script and 
	// sent to the player data script to finish processing
	public void ProcessUpgrade(int itemId, int itemIndex, int itemCost) {
		pds.UpdateProfileForPurchase(profileId, itemId, itemIndex, itemCost);
	}

	// LoadGame is called on start to load necessary assets
	IEnumerator LoadGame() {
		yield return new WaitForEndOfFrame();
		GameObject stars = Instantiate(starsBackground) as GameObject;
		DontDestroyOnLoad(stars);
		GameObject lib = Instantiate(gameLibrary) as GameObject;
		DontDestroyOnLoad(lib);
		StartCoroutine(LoadLibraries(lib));
		StartCoroutine(LoadProfiles(lib));
		StartCoroutine(LoadMenu());
	}

	// LoadLibraries sets the library variables
	IEnumerator LoadLibraries(GameObject lib) {
		yield return new WaitForEndOfFrame();
		ll = lib.GetComponent<LevelLibrary>();
		sul = lib.GetComponent<ShipUpgradeLibrary>();
	}

	// LoadProfiles gets any player data available
	IEnumerator LoadProfiles(GameObject lib) {
		yield return new WaitForEndOfFrame();
		pds = lib.GetComponent<PlayerDataScript>();
		bool load = pds.LoadData();
		if (!load) {
			pds.CreateNewSaveFile();
		}
	}

	// LoadMenu - Called on start to go to the menu
	IEnumerator LoadMenu() {
		yield return new WaitForEndOfFrame();
		SceneManager.LoadScene("wMenuScene");
		yield return new WaitForSeconds(1f);
		canvas = Instantiate(canvasObject) as GameObject;
		GameObject mm = Instantiate(mainMenu,canvas.transform) as GameObject;
		mm.GetComponent<MenuManagementScript>().StartMainMenu();
	}

	// ReturnToMenu - like load menu, but called after a level
	IEnumerator ReturnToMenu() {
		yield return new WaitForEndOfFrame();
		SceneManager.LoadScene("wMenuScene");
		yield return new WaitForSeconds(1f);
		canvas = Instantiate(canvasObject) as GameObject;
		GameObject mm = Instantiate(mainMenu, canvas.transform) as GameObject;
		mm.GetComponent<MenuManagementScript>().SetProfile(profileId);
		mm.GetComponent<MenuManagementScript>().StartInGameMenu();
	}

	// LoadLevel - starts up a level
	IEnumerator LoadLevel(int lvl, int[][] shipInfo) {
		yield return new WaitForEndOfFrame();
		SceneManager.LoadScene("wPlayScene");
		yield return new WaitForSeconds(1f);
		GameObject pm = Instantiate(playManager, null) as GameObject;
		canvas = Instantiate(canvasObject) as GameObject;
		pm.GetComponent<PlayManagementScript>().SetupLevel(lvl, shipInfo, sul, canvas);
	}
}
