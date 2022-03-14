using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public class InteractableShippart : Interactable {

		[SerializeField] private GameObject part;

		protected override void Interact() {
			GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>().Collect(part);
		}
	}
}