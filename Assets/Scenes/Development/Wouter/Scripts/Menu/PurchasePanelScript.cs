using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanelScript : MonoBehaviour {
	/*******************************************************************
	 * PURCHASE PANEL
	 * This panel is the last check before actually purchasing an
	 * item or upgrade. It checks available funds, gives a confirm and
	 * cancel button if enough, and a message if not. On purchase, it
	 * passes this on through. 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	// editor set variables
	public GameObject confirmText, noFundsText, bConfirm;

	private int itemId, itemIndex, itemCost;
	private MenuManagementScript mms;

	public void PopulateItem(int id, int index, int cost, MenuManagementScript mmst) {
		itemId = id;		// is the 1st ID within the ShipProfile array
		itemIndex = index;  // is the 2nd ID within the ShipProfile array
		itemCost = cost;	// is the 2nd val within the Progress Array
		mms = mmst;
		CheckForFunds();
	}

	// CheckForFunds
	// checks if there is enough funds for purchase.
	private void CheckForFunds() {
		int cash = mms.GetTmpProgressProfile()[1];
		if (itemCost > cash) {
			confirmText.SetActive(false);
			bConfirm.SetActive(false);
		} else {
			noFundsText.SetActive(false);
			confirmText.GetComponent<Text>().text = GetConfirmText();
		}
	}

	// ConfirmText
	// Builds up the text shown to confirm purchase
	private string GetConfirmText() {
		string startString = "Purchase: ";
		string itemString = GameManagementScript.gms.gameLibrary.GetComponent<ShipUpgradeLibrary>().itemName[itemId];
		if (itemIndex > 0) {
			itemString += " (sub-component upgrade)";
		}
		string referString = " for: ";
		string costString = itemCost.ToString();

		string fullString = startString + itemString + referString + costString;

		return fullString;
	}

	// Purchase
	public void Purchase() {
		mms.CallUpgradeItem(itemId, itemIndex, itemCost);
		CloseWindow();
	}

	// CloseWindow
	public void CloseWindow() {
		Destroy(this.gameObject);
	}
}
