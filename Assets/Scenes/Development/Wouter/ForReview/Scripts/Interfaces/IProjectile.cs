using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public interface IProjectile {
		Damage damage { get; set; }
		GameObject child { get; set; }
		Vector3 direction { get; set; }
		float velocity { get; set; }
		bool active { get; set; }
		void TriggerCollision(Collider2D col);
		void Setup(Vector3 start, Vector3 target, Quaternion rotation);
	}
}
