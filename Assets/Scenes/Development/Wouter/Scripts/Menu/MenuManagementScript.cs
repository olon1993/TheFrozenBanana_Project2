using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagementScript : MonoBehaviour { 

	/*******************************************************************
	 * MENU MANAGER
	 * This is where the menu (magic) happens. This handles the main
	 * menu, the in game menu and the upgrade and purchase panels.
	 * For easier coding purposes and data safety, temporary data is 
	 * stored here. If data changes, the manager refreshes the data by
	 * retrieving it from the source and updating the information in
	 * the temporary arrays. Four main parts here are
	 *		MainMenu   - this is the main menu.
	 *		IngameMenu - this is the menu structure once a profile is
	 *					 chosen. 
	 *		Upgrades   - The panel that opens to show options available
	 *				     for purchase.
	 *		Purchase   - the confirmation panel before purchase is 
	 *					 actually processed.
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *	0.2		WH	Added progress bar and cash information
	 *******************************************************************/
	// editor set variables
	// prefabs
	public GameObject itemContainer;
	public GameObject purchasePanel;
	public GameObject upgradePanel;

	   // menu structures
	public GameObject[] mainMenuTree;
	public GameObject mainMenuContainer;
	public GameObject[] inGameMenuTree;
	public GameObject inGameMenuContainer;

	   // others
	public Text cashAmt, progressAmt;
	public Slider progressSlider;

	// run time edited items
	public Button bStart, bUpgrades;
	public Text profileText;

	// temporary info for data handling
	public GameObject[] tmpObjects;
	private int[][] tmpShipInfo;
	private int[] tmpProgressInfo;
	private int tmpPage, maxLevels;
	private GameObject tmpUpgradePanel;

	private void Awake() {
		tmpObjects = new GameObject[0];
	}

	// MainMenu
	public void StartMainMenu() {
		mainMenuContainer.SetActive(true);
		inGameMenuContainer.SetActive(false);
		for (int i = 0; i < mainMenuTree.Length; i++) {
			mainMenuTree[i].SetActive(false);
		}
		mainMenuTree[0].SetActive(true);
	}

	// SwitchMainPanel
	// switches between main menu pages
	public void SwitchMainPanel(int pageId) {
		for (int i = 0; i < mainMenuTree.Length; i++) {
			mainMenuTree[i].SetActive(false);
		}
		mainMenuTree[pageId].SetActive(true);
	}

	// SetProfile
	// is called when selecting a profile to set the
	// ID in the game manager.
	public void SetProfile(int id) {
		GameManagementScript.gms.SetProfileId(id);
		profileText.text = "Profile " + (id+1).ToString();
		StartInGameMenu();
	}

	// InGame Menu
	public void StartInGameMenu() {
		mainMenuContainer.SetActive(false);
		inGameMenuContainer.SetActive(true);
		for (int i = 0; i < mainMenuTree.Length; i++) {
			inGameMenuTree[i].SetActive(false);
		}
		inGameMenuTree[0].SetActive(true);
		ScreenProfile();
	}

	// SwitchInGamePanel
	// switches between in game menu pages
	public void SwitchInGamePanel(int pageId) {

		for (int i = 0; i < inGameMenuTree.Length; i++) {
			inGameMenuTree[i].SetActive(false);
		}
		inGameMenuTree[pageId].SetActive(true);
		if (pageId > 0) {
			PopulateUpgradeItems(pageId);
		}
	}

	// ScreenProfile
	// Retrieves the profile information and temporarily stores it
	// for display purposes.
	private void ScreenProfile() {
		int tmpId = GameManagementScript.gms.GetProfileId();
		tmpShipInfo = GameManagementScript.pds.GetProfileShip(tmpId);
		tmpProgressInfo = GameManagementScript.pds.GetProfileProgress(tmpId);
		// this is a check to see if any levels have been played. If 0 levels have been played,
		// no money, and so no upgrades are available. Disable Upgrades button.
		if (tmpProgressInfo[0] == 0) {
			bUpgrades.interactable = false;
		} else {
			bUpgrades.interactable = true;
		}
		maxLevels = GameManagementScript.gms.GetLevelLibrary().levels.Length;
		if (tmpProgressInfo[0] == maxLevels) {
			bStart.interactable = false;
		} else {
			bStart.interactable = true;
		}
		cashAmt.text = tmpProgressInfo[1].ToString();
		UpdateProgressBar(tmpProgressInfo[0], maxLevels);
		// Now, for some reason if this is called only once, the bar does not show properly.
		// Call it twice, it works! ......
		UpdateProgressBar(tmpProgressInfo[0], maxLevels);
	}

	// UpdateProgressBar
	// v 0.2 - Added
	private void UpdateProgressBar(int current, int max) { 
		progressSlider.minValue = 0;
		progressSlider.value = (float)current;
		progressSlider.maxValue = max;
		float perc = 100 * ((float)current / (float)max);
		progressAmt.text = Mathf.RoundToInt(perc).ToString() + "%";
	}

	// PopulateUpgradeItems
	// based on the page id (referring to ship weapons or ship systems)
	// upgrade / purchase items available according to what type they are
	// limited to where a player is on levels
	public void PopulateUpgradeItems(int pageId) {

		PurgePanels();
		tmpPage = pageId;
		int itemLimiter = 0;
		if (tmpProgressInfo[0] >= 6) {
			itemLimiter = 3;
		} else if (tmpProgressInfo[0] >= 4) {
			itemLimiter = 2;
		} else if (tmpProgressInfo[0] >= 2) {
			itemLimiter = 1;
		}

		int counter = 0;

		if (pageId == 1) {
			// weapons
			for (int i = 0; i < 4; i++) {
				GameObject ic = Instantiate(itemContainer, inGameMenuTree[pageId].transform) as GameObject;
				ic.GetComponent<ItemContainerScript>().PopulateItem(i, tmpShipInfo, this);
				AddPanel(ic);
				counter++;
				if (counter > itemLimiter) {
					break;
				}
			}
		} else if (pageId == 2) {
			// systems
			for (int i = 4; i < 8; i++) {
				GameObject ic = Instantiate(itemContainer, inGameMenuTree[pageId].transform) as GameObject;
				ic.GetComponent<ItemContainerScript>().PopulateItem(i, tmpShipInfo, this);
				AddPanel(ic);
				counter++;
				if (counter > itemLimiter) {
					break;
				}
			}
		}
		OrganizePanels();
	}
	 
	// AddPanel
	// add items to the panel necessary to show all available options
	private void AddPanel(GameObject panel) {
		int newLength = tmpObjects.Length + 1; 
		GameObject[] tmpArray = new GameObject[newLength];
		for (int i = 0; i < tmpObjects.Length; i++) {
			tmpArray[i] = tmpObjects[i];
		}
		tmpArray[newLength - 1] = panel;
		tmpObjects = tmpArray;
	}

	// OrganizePanels
	// lays out the UI elements accordingly
	private void OrganizePanels() {
		// put all objects in tmpObjects array in order using rectTransform
		// only change y factor in anchors. height is 0.2f, padding per object is 0.025f.
		float yMax = 1f;
		float yHeight = 0.2f;
		float yPad = 0.025f;
		int count = 0;
		for (int i = 0; i < tmpObjects.Length; i++) {
			RectTransform rct = tmpObjects[i].GetComponent<RectTransform>();
			float minX = rct.anchorMin.x;
			float minY = yMax - (count + 1) * (yHeight + yPad);
			float maxX = rct.anchorMax.x;
			float maxY = yMax - ((count * yHeight) + ((count + 1) * yPad));
			Vector2 vMin = new Vector2(minX, minY);
			Vector2 vMax = new Vector2(maxX, maxY);
			rct.anchorMin = vMin;
			rct.anchorMax = vMax;
			count++;
		}
	}

	// PurgePanels
	// clears out the temporary panels information
	private void PurgePanels() {

		if (tmpObjects.Length > 0) {
			for (int i = tmpObjects.Length; i > 0; i--) {
				Destroy(tmpObjects[i - 1]);
			}
		}
		tmpObjects = new GameObject[0];
	}

	// OpenPurchasePanel
	// Purchase panel can use the purchase panel for buying new items as well as for upgradeable items
	public void OpenPurchasePanel(int id, int index, int cost) {
		GameObject pp = Instantiate(purchasePanel, this.gameObject.transform) as GameObject;
		pp.GetComponent<PurchasePanelScript>().PopulateItem(id, index, cost, this);
	}

	// OpenUpgradePanel
	// Upgrade panel shows the upgrade options for an item
	public void OpenUpgradePanel(int id) {
		tmpUpgradePanel = Instantiate(upgradePanel, this.gameObject.transform) as GameObject;
		tmpUpgradePanel.GetComponent<UpgradePanelScript>().PopulateItem(id, this);
	}

	// CloseUpgradePanel
	private void CloseUpgradePanel() {
		Destroy(tmpUpgradePanel);
	}

	// CallUpgradeItem
	// Called to upgrade an item. Refreshes all windows after
	public void CallUpgradeItem(int itemId, int itemIndex, int itemCost) {
		GameManagementScript.gms.ProcessUpgrade(itemId, itemIndex, itemCost);
		CloseUpgradePanel();
		ScreenProfile();
		SwitchInGamePanel(tmpPage);
		OpenUpgradePanel(itemId);
	}

	// GetTmpShipProfile
	// really can be done differently, this was a quicker solution
	public int[][] GetTmpShipProfile() {
		return tmpShipInfo;
	}

	// GetTmpProgressProfile
	// really can be done differently, this was a quicker solution
	public int[] GetTmpProgressProfile() {
		return tmpProgressInfo;
	}

	// PlayLevel
	// Button caller to start up level. This passes to game manager
	public void PlayLevel() {
		GameManagementScript.gms.PlayLevel();
	}

	// Close App
	// not done correctly, but closes app (not in editor btw). 
	public void CloseApp() {
		// this is not the proper way to do this. 
		Application.Quit();
	}
}
