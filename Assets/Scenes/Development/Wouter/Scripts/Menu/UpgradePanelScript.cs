using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelScript : MonoBehaviour
{
	/*******************************************************************
	 * UPGRADE PANEL
	 * The upgrade panel is where a player can see all available
	 * upgrades, the current status of the upgrades, the costs, etc.
	 * It is populated using the item id that was selected to open this
	 * panel. Using the ID the script looks up all upgrade options for
	 * the item within the Ship Upgrade Library.
	 * Each ship item will populate its own list, according to what is
	 * available to that item. 
	 * Each upgradeable item is made in the form of a button as a 
	 * container, and all the information within it (slider, text, etc.)
	 * is setup to not catch raycasting. 
	 * 
	 * NOTE: 
	 * Due to the poor manner in how information is stored for upgrades
	 * there is a horrible bit of code below that per item retrieves
	 * the necessary information.
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *	0.2     WH  Added simple lines to disable buttons if not usable
	 *******************************************************************/
	// editor set variables
	public Text panelName;
	public Image panelIcon;
	public GameObject[] bSliders, tTooltips;
	public Text[] sliderNames, sliderCosts;

	// private variables
	public int itemId;
	public int[] upgradeCost = new int[4];
	private int[][] tmpShipInfo;
	private MenuManagementScript mms;
	private ShipUpgradeLibrary sulRef;
	private TooltipTexts ttt;

	private string cMax = "max";
	private string cVal = "val";
	
	// called upon after instantiation to fill info
	public void PopulateItem(int id, MenuManagementScript mmst) {
		itemId = id;
		mms = mmst;
		ttt = GetComponent<TooltipTexts>();
		tmpShipInfo = mms.GetTmpShipProfile();
		sulRef = GameManagementScript.gms.gameLibrary.GetComponent<ShipUpgradeLibrary>();
		SetupPanelInfo();
	}

	// sets up entire panel and info within
	private void SetupPanelInfo() {
		foreach (GameObject slider in bSliders) {
			slider.SetActive(false);
		}
		panelName.text = sulRef.itemName[itemId];
		panelIcon.sprite = sulRef.itemIcons[itemId];
		if (itemId < 4) {
			// Slider setup for weapons
			for (int i = 1; i < tmpShipInfo[itemId].Length; i++) {
				bSliders[i - 1].SetActive(true);
				if (i != 4) {
					sliderNames[i - 1].text = sulRef.basicWUpgradeName[i - 1];
				} else {
					sliderNames[i - 1].text = sulRef.specialWUpgradeName[itemId];
				}
				sliderCosts[i - 1].text = GetWeaponSliderCost(bSliders[i-1],i);
				SetSliderValues(i);

			}
		} else {
			// Slider setup for systems
			for (int i = 1; i < tmpShipInfo[itemId].Length; i++) {
				bSliders[i - 1].SetActive(true);
				if (i == 1) { 
					sliderNames[i - 1].text = sulRef.firstSUpgradeName[itemId-4];
				}
				else if (i == 2) {
					sliderNames[i - 1].text = sulRef.secondSUpgradeName[itemId-4];
				}
				sliderCosts[i - 1].text = GetSystemSliderCost(bSliders[i-1], i);
				SetSliderValues(i);

			}
		}
	}

	// Called to retrieve the cost of the slider. 
	// v0.2: If upgrade is maxed, it will also disable the button.
	private string GetWeaponSliderCost(GameObject slider, int i) {
		string val = "";
		int currentLevel = RetrieveValue(itemId,i,cVal); // this is the current upgrade level of the item. 
		int maxLevel = RetrieveValue(itemId,i,cMax);
		if (currentLevel == maxLevel) {
			val = "Max";
			slider.GetComponent<Button>().interactable = false;
			upgradeCost[i-1] = 0;
			return val;
		}
		if (itemId < 4) {
			if (i == 1 || i == 3) {
				upgradeCost[i - 1] = (currentLevel + 1) * 100;
			} else if (i == 2) {
				upgradeCost[i - 1] = (currentLevel + 1) * 50;
			} else if (i == 4) {
				upgradeCost[i - 1] = (currentLevel + 1) * 500;
			}
		}
		val = upgradeCost[i - 1].ToString();

		return val;
	}

	// Called to retrieve the cost of the slider. 
	// v0.2: If upgrade is maxed, it will also disable the button.
	private string GetSystemSliderCost(GameObject slider, int i) {
		string val = "";
		int currentLevel = RetrieveValue(itemId, i, cVal); // this is the current upgrade level of the item. 
		int maxLevel = RetrieveValue(itemId, i, cMax);
		if (currentLevel == maxLevel) {
			val = "Max";
			slider.GetComponent<Button>().interactable = false;
			upgradeCost[i - 1] = 0;
			return val;
		}
		upgradeCost[i - 1] = (currentLevel + 1) * 100;
		val = upgradeCost[i - 1].ToString();
		return val;
	}

	// does what it says
	private void SetSliderValues(int i) {
		Slider tmpSlider = bSliders[i - 1].GetComponentInChildren<Slider>();
		tmpSlider.minValue = 0;
		tmpSlider.maxValue = RetrieveValue(itemId, i, cMax); // this is the length of the specific value array in game library (ship upgrade library) - 1. 
		tmpSlider.value = RetrieveValue(itemId, i, cVal); // this is the value from the tmpShipInfo
	}

	// Calls a sub-window as a control step before actual purchase
	public void PurchaseItem(int i) {
		mms.OpenPurchasePanel(itemId, i, upgradeCost[i - 1]);
	}

	public void ShowTooltip(int index) {
		tTooltips[index].SetActive(true);
		tTooltips[index].GetComponentInChildren<Text>().text = ttt.RetrieveTooltipText(itemId, index);
	}

	public void HideTooltip(int index) {
		tTooltips[index].SetActive(false);
	}

	public void CloseWindow() { 
		Destroy(this.gameObject);
	}


	// HORRIBLE CODING BELOW

	// Probably the worst way to create a lookup method, but fuck it...it should work and won't need any editing later on. 
	// especially with the string comparison...horrible. but easier to read than with even more ints :P
	private int RetrieveValue(int item, int index, string txt) {
		//	Debug.Log("Retrieve Value of: item("+ item + "); index(" + index + "); val(" + txt + ");");
		int returnVal = 0;
		if (item == 0) {
			// guns
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.gunFireRate.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.gunProjectileSpeed.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 3) {
				if (txt == cMax) {
					returnVal = sulRef.gunDamage.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 4) {
				if (txt == cMax) {
					returnVal = sulRef.gunSpecial.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 1) {
			// cannon
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.cannonFireRate.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.cannonProjectileSpeed.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 3) {
				if (txt == cMax) {
					returnVal = sulRef.cannonDamage.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 4) {
				if (txt == cMax) {
					returnVal = sulRef.cannonSpecial.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 2) {
			// laser
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.laserFireRate.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.laserProjectileSpeed.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 3) {
				if (txt == cMax) {
					returnVal = sulRef.laserDamage.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 4) {
				if (txt == cMax) {
					returnVal = sulRef.laserSpecial.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 3) {
			// rockets
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.rocketFireRate.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.rocketProjectileSpeed.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 3) {
				if (txt == cMax) {
					returnVal = sulRef.rocketDamage.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 4) {
				if (txt == cMax) {
					returnVal = sulRef.rocketSpecial.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 4) {
			// hull
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.shipArmor.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.shipStructure.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 5) {
			// shield
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.shieldGenerator.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.shieldCapacitor.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 6) {
			// engine
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.engineEnergy.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.engineOutput.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		} else if (item == 7) {
			// cooling
			if (index == 1) {
				if (txt == cMax) {
					returnVal = sulRef.coolerTanks.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			} else if (index == 2) {
				if (txt == cMax) {
					returnVal = sulRef.coolerTurbines.Length - 1;
				} else if (txt == cVal) {
					returnVal = tmpShipInfo[item][index];
				}
			}
		}

		//	Debug.Log("Value is: " + returnVal);

		return returnVal;
	}
}
