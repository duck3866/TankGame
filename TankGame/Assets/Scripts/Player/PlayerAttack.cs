using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : IState<PlayerController>
{
    private PlayerController _playerController;
    public void OperateEnter(PlayerController _player)
    {
        _playerController = _player;
    }

    public void OperateUpdate(PlayerController _player)
    {
        GameObject currentObject = _playerController.shootingPoint;
        _playerController.Projectile.ShowTrajectoryLine(currentObject.transform.position, currentObject.transform.forward * 6f);
        TurretRotate();
        MuzzleRotate();
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
    }
    private void TurretRotate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        _playerController.turret.transform.Rotate(0, horizontal, 0);
    }

    private void MuzzleRotate()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float currentXRotation = _playerController.muzzle.transform.eulerAngles.x;
        if (currentXRotation > 180)
        {
            currentXRotation -= 360;
        }

        currentXRotation = Mathf.Clamp(currentXRotation + vertical, -90f, 0f);
        if (currentXRotation < 0)
        {
            currentXRotation += 360;
        }
        _playerController.muzzle.transform.localEulerAngles = new Vector3(currentXRotation, 0, 0);
    }
}
