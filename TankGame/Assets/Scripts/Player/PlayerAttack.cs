using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : IState<PlayerController>
{
    private float minAttackPower = 1;
    private float maxAttackPower = 9;
    private int lineSegments = 60;
    private float timeOfTheFlight = 5;
    
    private PlayerController _playerController;
    public void OperateEnter(PlayerController _player)
    {
        _playerController = _player;
        _playerController.lineRenderer.enabled = true;
    }

    public void OperateUpdate(PlayerController _player)
    {
        GameObject currentObject = _playerController.shootingPoint;
        ShowTrajectoryLine(currentObject.transform.position, currentObject.transform.forward * 6f);
        TurretRotate();
        MuzzleRotate();
        Shooting();
    }

    public void OperateExit(PlayerController _player)
    {
        _playerController.lineRenderer.positionCount = 0;
        _playerController.lineRenderer.enabled = false;
        
        _playerController.playerTurn++;
        UIManager.Instance.CheckPlayerTurn();
        GameManager.Instance.TurnChange("Enemy");
    }
    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }
            GameObject currentObject = _playerController.shootingPoint;
            
            _playerController.audioSource.Play();
            
            GameObject bullet = BulletManager.Instace.GetObject();
            bullet.transform.position = currentObject.transform.position;
            
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            
            Transform shootPoint = _playerController.shootingPoint.transform;
            Vector3 shootDirection = shootPoint.forward.normalized; // 방향 벡터 정규화

            bulletRigidbody.velocity = Vector3.zero; // 기존 속도 초기화
            bulletRigidbody.AddForce(shootDirection * _playerController.attackPower, ForceMode.Impulse);
            _playerController.ChangeState(PlayerController.PlayerState.Move);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerController.attackPower < maxAttackPower)
            {
                _playerController.attackPower++;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            if (_playerController.attackPower > minAttackPower)
            {
                _playerController.attackPower--;
            }
        }
        UIManager.Instance.powerGageSlider.value =  _playerController.attackPower / maxAttackPower;
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
    public void ShowTrajectoryLine(Vector3 startPoint, Vector3 startVelocity)
    {
        float timeStep = timeOfTheFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTrajectoryLine(startPoint, startVelocity, timeStep);

        _playerController.lineRenderer.positionCount = lineSegments;
        _playerController.lineRenderer.SetPositions(lineRendererPoints);
    }
    private Vector3[] CalculateTrajectoryLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];
        lineRendererPoints[0] = startPoint;
        
        Vector3 currentPoint = startPoint;
        Vector3 currentVelocity = startVelocity;

        for (int i = 1; i < lineSegments; i++)
        {
            Vector3 progressBeforeGravity = currentVelocity * timeStep;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeStep * timeStep;
            Vector3 nextPosition = currentPoint + progressBeforeGravity - gravityOffset;
            
            Vector3 direction = (nextPosition - currentPoint).normalized;

            float baseDistance = Vector3.Distance(currentPoint, nextPosition) * (_playerController.attackPower / 9);
            RaycastHit raycastHit;
            if (Physics.Raycast(currentPoint, direction, out raycastHit, baseDistance))
            {
                if (raycastHit.collider.CompareTag("Block"))
                {
                    lineRendererPoints[i] = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
                    
                    for (int j = i + 1; j < lineSegments; j++)
                    {
                        lineRendererPoints[j] = lineRendererPoints[i];
                    }
                    break;
                }
            }
            
            Vector3 normalizedDirection = (nextPosition - currentPoint).normalized;
            nextPosition = currentPoint + (normalizedDirection * baseDistance);
        
            lineRendererPoints[i] = nextPosition;
            currentPoint = nextPosition;
            currentVelocity += Physics.gravity * timeStep;
        }

        return lineRendererPoints;
    }
}
