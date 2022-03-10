using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TheFrozenBanana
{
    public class CameraController : MonoBehaviour
    {
        CinemachineVirtualCamera cinemachineCamera;
        private GameObject player;

        private void Awake()
        {
            cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
            if (cinemachineCamera == null) return;

            StartCoroutine(FindPlayer());
        }

        private IEnumerator FindPlayer()
        {
            
            while (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                yield return new WaitForEndOfFrame();
            }
            cinemachineCamera.Follow = player.transform;
            //cinemachineCamera.LookAt = player.transform;
        }
    }
}

