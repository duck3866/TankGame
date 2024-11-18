using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : IState<PlayerController>
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject bulletFactory;
    [SerializeField] private GameObject shootingPoint;
    private PlayerController _playerController;
    public void OperateEnter(PlayerController _player)
    {
        _playerController = _player;
    }

    public void OperateUpdate(PlayerController _player)
    {
        // TurretRotate();
        // MuzzleRotate();  
        Shooting();
    }

    public void OperateExit(PlayerController _player)
    {
        Debug.Log("공격 -> 무브");
    }
    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }
            _playerController.ChangeState(PlayerController.PlayerState.Move);
        }
        //
        //     GameObject bullet = Instantiate(bulletFactory);
        //     bullet.transform.position = shootingPoint.transform.position;
        //     Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        //     rigidbody.AddForce(muzzle.transform.forward * 5f, ForceMode.Impulse);
        // }
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
