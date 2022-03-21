using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelTweaking : MonoBehaviour
{
    CinemachineVirtualCamera playerCam;
    CinemachineFramingTransposer transposer;

    [SerializeField] Vector3 playerCamOffset;

    void Awake()
    {
        playerCam = FindObjectOfType<CinemachineVirtualCamera>();
        transposer = FindObjectOfType<CinemachineFramingTransposer>();
    }

    void Start()
    {
        transposer.m_TrackedObjectOffset = playerCamOffset;
    }


}
