using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public class ProjectileChild : MonoBehaviour {

		[SerializeField] private IProjectile projectileParent;
		private Collider2D myCollider;

		private void Awake() {
			projectileParent = gameObject.GetComponentInParent<IProjectile>();
			myCollider = gameObject.GetComponent<Collider2D>();
			if (myCollider == null) {
				Debug.LogError(gameObject.GetComponentInParent<GameObject>().name + " has no proper collider setup! Check child in Prefab!");
			}
 		}

		private void OnTriggerEnter2D(Collider2D col) {
			projectileParent.TriggerCollision(col);
		}
	}
}