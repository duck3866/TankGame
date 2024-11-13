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
        muzzle.transform.Rotate(vertical,0,0);
    }
}
