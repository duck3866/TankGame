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
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isAttackMode = false;

    public int playerX;
    public int playerY;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAttackMode && Input.GetMouseButtonDown(0))
        {
            isAttackMode = true;
        }
        if (isAttackMode)
        {
            MuzzleRotate();
            TurretRotate();
            Shooting();
        }
        else
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (playerX < MapManager.Instace.x - 1)
            {
                playerX += 1;
                PlayerMoving(Vector3.right);    
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (playerX > 0)
            {
                playerX -= 1;
                PlayerMoving(Vector3.left);   
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (playerY < MapManager.Instace.y -1)
            {
                playerY += 1;
                PlayerMoving(Vector3.forward);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (playerY > 0)
            {
                playerY -= 1;
                PlayerMoving(Vector3.back);
            }
        }
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
            rigidbody.AddForce(muzzle.transform.forward * 5f, ForceMode.Impulse);
            isAttackMode = false;
        }
    }

    private void PlayerMoving(Vector3 dir)
    {
        transform.position += dir * 1;
        transform.forward = dir;
    }

    private void TurretRotate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        turret.transform.Rotate(0, horizontal, 0);
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

        muzzle.transform.localEulerAngles = new Vector3(currentXRotation, 0, 0);
    }
}