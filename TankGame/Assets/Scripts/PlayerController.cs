using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject bulletFactory;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private float distance;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 destinationPoint;
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMove();
        Shooting();
        TurretRotate();
        MuzzleRotate();
    }

    private void PlayerMove()
    {
        
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = shootingPoint.transform.position;
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.AddForce(muzzle.transform.forward * 5f,ForceMode.Impulse);      
        }
    }

    private void TurretRotate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        turret.transform.Rotate(0,horizontal,0);
    }

    private void MuzzleRotate()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float currentXRotation = muzzle.transform.eulerAngles.x;
        if (currentXRotation > 180)
        {
            currentXRotation -= 360;
        }
        currentXRotation = Mathf.Clamp(currentXRotation + vertical, -90f, 0f);
        if (currentXRotation < 0)
        {
            currentXRotation += 360;
        }
        // muzzle.transform.eulerAngles = new Vector3(currentXRotation, 0, 0);
        muzzle.transform.localEulerAngles = new Vector3(currentXRotation, 0, 0);
    }

}
