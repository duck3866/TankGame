using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera customCam; // Inspector에서 할당하거나
    [SerializeField] private Vector3 offset = new Vector3(-5,6,0);
    [SerializeField] private float rotSpeed = 200f;
    private GameObject player;
    private Transform playerTransform;
    
    private float my = -51;
    private float mx = 90;
    void Start()
    {
        if (customCam == null)
        {
            customCam = FindObjectOfType<CinemachineVirtualCamera>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        if (playerTransform != null && customCam != null)
        {
            customCam.Follow = playerTransform;
        }
    }
    
    void Update()
    {
        if (player.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");
        
            mx += mouseX * rotSpeed * Time.deltaTime;
            my += mouseY * rotSpeed * Time.deltaTime;
        
            my = Mathf.Clamp(my, -90f, 90f);
            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
        var cinemachine = customCam.GetCinemachineComponent<CinemachineTransposer>();
        cinemachine.m_FollowOffset = offset;
    }
}
