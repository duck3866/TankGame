using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera customCam; // Inspector에서 할당하거나
    [SerializeField] private Transform lookPosition;
    [SerializeField] private Transform cameraPosition;
    private GameObject player;
    private Transform playerTransform;

    private void Start()
    {
        if (customCam == null)
        {
            customCam = FindObjectOfType<CinemachineVirtualCamera>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        customCam.LookAt = lookPosition;
    }

    private void Update()
    {
        if (player.activeSelf)
        {
            customCam.LookAt = playerTransform;
            Vector3 nowPosition = cameraPosition.position;
            nowPosition.y = 10;
            cameraPosition.position = nowPosition;
            
        }
        else
        {
            customCam.LookAt = lookPosition;   
            Vector3 nowPosition = cameraPosition.position;
            nowPosition.y = 15;
            cameraPosition.position = nowPosition;
        }
    }
}