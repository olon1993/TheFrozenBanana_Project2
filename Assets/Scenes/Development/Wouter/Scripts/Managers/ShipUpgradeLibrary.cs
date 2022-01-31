using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUpgradeLibrary : MonoBehaviour
{
	/*******************************************************************
	 * SHIP UPGRADE LIBRARY
	 * This is simple enough, a library of information.
	 * The variables are defined here, and are filled in the editor. 
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/
	public float[] gunFireRate, gunProjectileSpeed, gunDamage, gunSpecial, 
					cannonFireRate, cannonProjectileSpeed, cannonDamage, cannonSpecial, 
					laserFireRate, laserProjectileSpeed, laserDamage, laserSpecial,
					rocketFireRate, rocketProjectileSpeed, rocketDamage, rocketSpecial,
					shipArmor, shipStructure,
					shieldGenerator, shieldCapacitor,
					engineEnergy, engineOutput,
					coolerTanks, coolerTurbines;

	public string[] itemName, basicWUpgradeName, specialWUpgradeName, firstSUpgradeName, secondSUpgradeName;
	public Sprite[] itemIcons;

}
