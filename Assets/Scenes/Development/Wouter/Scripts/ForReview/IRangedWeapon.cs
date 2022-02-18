using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public interface IRangedWeapon : IWeapon {

		GameObject projectile { get; }
		Transform rotationalCenter { get; }
		Transform target { get; set; }
		Renderer weaponObject { get; }
		Vector3 localScale { get; }
		bool canAim { get; set; }
		float projectileKillTime { get; set; }
	}
}
