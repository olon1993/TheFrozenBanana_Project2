using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public interface ICollectible 
	{
		public enum CollectibleType 
		{
			SHIPPART, CD, HEALTH, STAMINA
		}
		Collider2D collectionBox { get; }
		SpriteRenderer spriteRenderer { get; }
		Sprite collectionSprite { get; }
		AudioSource audioSource { get; }
		bool collected { get; }
		Vector3 centerLocation { get; }
		float moveVertical { get; }
		float moveHorizontal { get; }
		float moveTime { get; }
		float timeAfterCollectRemove { get; }
		IEnumerator FloatMovement();
		void Collect(GameObject player);
	}
}
