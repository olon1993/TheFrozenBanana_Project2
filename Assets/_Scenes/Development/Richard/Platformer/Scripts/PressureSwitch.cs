using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PressureSwitch : MonoBehaviour
    {
        [SerializeField] GameObject movingPlatform;
        [SerializeField] float platformSpeed = 3;

        PlatformController platformController;
        Animator buttonAnimator;

        [SerializeField] bool offToDeactivate;

        void Awake()
        {
            buttonAnimator = GetComponent<Animator>();
            platformController = movingPlatform.GetComponent<PlatformController>();
            platformController.Speed = 0;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CanPushButtons") || other.gameObject.CompareTag("Enemy"))
            {
                print("pressure switch activated");
                platformController.Speed = platformSpeed;
                buttonAnimator.SetTrigger("ButtonDown");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CanPushButtons") || other.gameObject.CompareTag("Enemy"))
            {
                buttonAnimator.SetTrigger("ButtonUp");
                if(offToDeactivate)
                {
                    platformController.Speed = 0;
                }
            }
        }
    }
}
