using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PressureSwitch : MonoBehaviour
    {
        [SerializeField] GameObject movingPlatform;
        [SerializeField] GameObject[] movingPlatforms;
        [SerializeField] float platformSpeed = 3;

        PlatformController platformController;
        Animator buttonAnimator;

        [SerializeField] bool offToDeactivate;

        void Awake()
        {
            buttonAnimator = GetComponent<Animator>();
            if (movingPlatform != null) platformController = movingPlatform.GetComponent<PlatformController>();
            SetPlatformSpeed(0);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CanPushButtons") || other.gameObject.CompareTag("Enemy"))
            {
                print("pressure switch activated");
                buttonAnimator.SetTrigger("ButtonDown");
                SetPlatformSpeed(platformSpeed);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CanPushButtons") || other.gameObject.CompareTag("Enemy"))
            {
                buttonAnimator.SetTrigger("ButtonUp");
                if(offToDeactivate)
                {
                    SetPlatformSpeed(0);
                }
            }
        }

        void SetPlatformSpeed(float speed)
        {
            if (movingPlatforms.Length > 0)
            {
                foreach (GameObject platform in movingPlatforms)
                {
                    platform.GetComponent<PlatformController>().Speed = speed;
                }
                return;
            }
            platformController.Speed = speed;
        }
    }
}
