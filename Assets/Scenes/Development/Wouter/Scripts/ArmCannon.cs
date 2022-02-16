using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

public class ArmCannon : MonoBehaviour
{
	// fields to setup
	[SerializeField] private GameObject projectile;
	[SerializeField] private Transform rotationalCenter, projectileSpawnPoint, target;
	[SerializeField] private Renderer weaponObject;
	[SerializeField] private Camera cam;

	// values to setup
	[SerializeField] private float firingCooldown;

	private ILocomotion _locomotion;
	private bool firing, lockRefire;
	private float lookDir;
	private Vector3 localScale;

	private void Awake() {
		localScale = new Vector3(1.5f,1.5f,1f);
		weaponObject.enabled = false;
		_locomotion = gameObject.GetComponent<ILocomotion>();
	}

	private void Update() {
		UpdateTargetLocation();
		UpdateRotation();
		UpdateInput();
	}

	private void UpdateInput() {
		if (!firing) {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				firing = true;
				if (!lockRefire) {
					StartCoroutine(Shoot());
				}
			}
		}
		if (firing) {
			if (Input.GetKeyUp(KeyCode.Mouse0)) {
				firing = false;
			}
		}
	}

	// This makes the mouse pointer into an in world target
	// WORKS!
	private void UpdateTargetLocation() {
		Vector2 mousePos = Input.mousePosition;
		Vector3 pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
		target.position = pos;
		if (Vector2.Distance(gameObject.transform.position, target.position) < 1) {
			target.position = gameObject.transform.position + new Vector3(_locomotion.HorizontalLook,0,0);
		}
	}

	// This updates the rotation of the weapon graphics
	// WORKS!
	private void UpdateRotation() {
		// need 2 vectors: the base vector (which is the look vector of the player)
		// and the vector to the target position
		Vector2 vBase = new Vector2(_locomotion.HorizontalLook, 0);
		Vector2 vDir = target.position - gameObject.transform.position;
		float angle = Vector2.Angle(vBase, vDir) * _locomotion.HorizontalLook;
		if (target.position.y < gameObject.transform.position.y) {
			angle = -angle;
		}
		weaponObject.transform.localScale = new Vector3(_locomotion.HorizontalLook * localScale.x, localScale.y, localScale.z);
		weaponObject.transform.rotation = Quaternion.Euler(0,0,angle);
	}

	// This triggers the shots being fired
	// WORKS GOOD ENOUGH
	IEnumerator Shoot() {
		weaponObject.enabled = true;
		while (firing && !lockRefire) {
			if (lockRefire) {
				yield return new WaitForSeconds(firingCooldown);
			}
			CreateProjectile();
			lockRefire = true;
			yield return new WaitForSeconds(firingCooldown);
			lockRefire = false;
		}
		weaponObject.enabled = false;
	}

	// This spawns the projectile
	// WORKS! 
	// Issue: if the target distance is very close to player, the rendering does not match the movement.
	//        this has to do with the target and spawn locations.
	private void CreateProjectile() {
		Quaternion rot = weaponObject.transform.rotation;
		if (_locomotion.HorizontalLook < 0) {
			rot *= Quaternion.Euler(0,0,180);
		}
		GameObject proj = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity, null) as GameObject;
		proj.GetComponent<PlayerProjectile>().Setup(gameObject.transform.position, target.position, rot);
		Destroy(proj, 3f);
	}
}
