using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public interface IRangedWeapon : IWeapon {

		GameObject projectile { get; }
		Transform aimTool { get; set; }
		Transform target { get; set; }
		Renderer weaponObject { get; }
		Vector3 localScale { get; }
		bool canAim { get; set; }
		float maxAngle { get; set; }
		float projectileKillTime { get; set; }
		Camera cam { get; set; }
	}
}
