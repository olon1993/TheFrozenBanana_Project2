using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public class InteractableShippart : Interactable {

		[SerializeField] private GameObject part;

		protected override void Interact() {
			ILevel level = GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>();
			if(level != null)
            {
				level.Collect(part);
            }
			else
            {
				Debug.LogError("InteractableShipPart: No level object found");
            }
		}
	}
}