using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildProjectile : MonoBehaviour {

	[SerializeField] private PlayerProjectile pp;
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private float refreshRate;
	private SpriteRenderer sr;
	private int spriteIndex;

	private void Awake() {
		sr = gameObject.GetComponent<SpriteRenderer>();
		StartCoroutine(UpdateSprites());
	}

	private void OnTriggerEnter2D(Collider2D col) {
		pp.TriggerCollision(col);
	}

	private IEnumerator UpdateSprites() {
		while (true) {
			yield return new WaitForSeconds(refreshRate);
			UpdateSpriteRenderer();
		}
	}

	private void UpdateSpriteRenderer() {
		if (sprites.Length > 1) {
			spriteIndex++;
			if (spriteIndex == sprites.Length) {
				spriteIndex = 0;
			}
			sr.sprite = sprites[spriteIndex];
		}
	}
}