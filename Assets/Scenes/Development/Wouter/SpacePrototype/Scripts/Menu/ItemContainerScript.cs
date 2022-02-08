using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainerScript : MonoBehaviour {

	/*******************************************************************
	 * ITEM CONTAINER
	 * The Item container is essentially a button with information. This
	 * is created when opening an upgrade window. Here it shows all the
	 * stats of an item in a bar, the name and icon of it, and acts as
	 * a gateway between the in game menu and the upgrade panel. The
	 * items are only shown if available. If they are available but not
	 * yet purchased, the container shows a lock icon. 
	 * NOTE: 
	 * Due to the poor manner in how information is stored for upgrades
	 * there is a horrible bit of code below that per item retrieves
	 * the necessary information.
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	// editor set variables
	public GameObject itemIcon;
	public GameObject itemName;
	public GameObject itemLock;
	public GameObject[] sliders;

	// private variables
	private MenuManagementScript mms;
	private int itemId;
	private ShipUpgradeLibrary sulRef;
	private int[][] tmpShipInfo;
	private bool locked;

	private string cVal = "val";
	private string cMax = "max";
	
	// PopulateItem
	// is called after instantiation to fill the values. 
	public void PopulateItem(int id, int[][] tShipInfo, MenuManagementScript mmst) {
		itemId = id;
		mms = mmst;
		tmpShipInfo = tShipInfo;
		sulRef = GameManagementScript.gms.gameLibrary.GetComponent<ShipUpgradeLibrary>();
		RetrieveLibraryInfo();
	}

	// RetrieveLibraryInfo
	// using the profile info and the libraries, the
	// required information is gathered. 
	private void RetrieveLibraryInfo() {
		itemIcon.GetComponent<Image>().sprite = sulRef.itemIcons[itemId];

		itemName.GetComponent<Text>().text = sulRef.itemName[itemId];
		foreach (GameObject slider in sliders) {
			slider.SetActive(false);
		}
		if (tmpShipInfo[itemId][0] == 0) {
			// Locked
			locked = true;
		} else {
			itemLock.SetActive(false);
			for (int i = 1; i < tmpShipInfo[itemId].Length; i++) {
				sliders[i - 1].SetActive(true);
				Slider tmpSlider = sliders[i - 1].GetComponent<Slider>();
				tmpSlider.minValue = 0;
				tmpSlider.maxValue = RetrieveValue(itemId, i, cMax); // this is the length of the specific value array in game library (ship upgrade library) - 1. 
				tmpSlider.value = RetrieveValue(itemId, i, cVal); // this is the value from the tmpShipInfo
			}
		}
	}

	// ItemClick
	// The button functionality behind the scene.
	public void ItemClick() {
		if (locked) {
			mms.OpenPurchasePanel(itemId, 0, 500);
		} else {
			mms.OpenUpgradePanel(itemId);
		}
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
