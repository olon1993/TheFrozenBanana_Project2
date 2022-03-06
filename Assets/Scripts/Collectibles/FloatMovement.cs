using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class FloatMovement : MonoBehaviour 
	{
		[SerializeField] private float _moveHorizontal;
		[SerializeField] private float _moveVertical;
		[SerializeField] private float _moveTime;
		[SerializeField] private float _speedHorizontal;
		[SerializeField] private float _speedVertical;
		private Vector3 _currentLocation;
		private Vector3 _centerLocation;

		private void Awake() {
			_centerLocation = gameObject.transform.position;
			_currentLocation = gameObject.transform.position;
			if (_moveHorizontal > Mathf.Epsilon) {
				StartCoroutine(FloatX());
			}
			if (_moveVertical > Mathf.Epsilon) {
				StartCoroutine(FloatY());
			}
		}

		public IEnumerator FloatX() {
			float x = 0;
			float t = 0;
			float sign = 1;
			while (true) {
				t += Mathf.Sign(sign) * _speedHorizontal * (Time.deltaTime * _moveTime) / _moveTime;
				if (t > 1) {
					t = 1;
					sign = -1;
				} else if (t < 0) {
					t = 0;
					sign = 1;
				}
				x = Mathf.Lerp(-_moveHorizontal, _moveHorizontal, t);
				_currentLocation = new Vector3(_centerLocation.x + x, _currentLocation.y, _centerLocation.z);
				gameObject.transform.position = _currentLocation;
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator FloatY() {
			float y = 0;
			float t = 0;
			float sign = 1;
			while (true) {
				t += Mathf.Sign(sign) * _speedVertical * (Time.deltaTime * _moveTime) / _moveTime;
				if (t > 1) {
					t = 1;
					sign = -1;
				} else if (t < 0) {
					t = 0;
					sign = 1;
				}
				y = Mathf.Lerp(-_moveVertical, _moveVertical, t);
				_currentLocation = new Vector3(_currentLocation.x, _centerLocation.y + y, _centerLocation.z);
				gameObject.transform.position = _currentLocation;
				yield return new WaitForEndOfFrame();
			}
		}
	}
}