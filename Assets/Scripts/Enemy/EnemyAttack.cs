using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttack : IState<EnemyController>
{
    private EnemyController _enemyController;
    private float attackPower;
    private bool isShoot = false;
    public void OperateEnter(EnemyController sender)
    {
        attackPower = 9;
        _enemyController = sender;
        isShoot = false;
    }

    public void OperateUpdate(EnemyController sender)
    {
        if (!isShoot)
        {
            isShoot = true;
            _enemyController.StartCoroutine(WaitShoot());
        }
    }

    private IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(2f);

        _enemyController.PlaySound();
        GameObject currentObject = _enemyController.turret;
        
        Vector3 targetPosition = _enemyController._player.transform.position;
        Vector3 shootingPosition = _enemyController.shootingPoint.transform.position;
        targetPosition.y = shootingPosition.y;
        
        currentObject.transform.LookAt(targetPosition);
        JudgmentAttackPower();
        yield return new WaitForSeconds(0.3f);
        GameObject bullet = BulletManager.Instace.GetObject();
        bullet.transform.position = shootingPosition;
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = Vector3.zero;
        
        bulletRigidbody.AddForce(currentObject.transform.forward * attackPower, ForceMode.Impulse);
        
        _enemyController.isJudgment = false;
        GameManager.Instance.TurnChange();
        _enemyController.isEnemyTurn = false;
    }

    private void JudgmentAttackPower()
    {
        float distance = Vector3.Distance(_enemyController._player.transform.position, _enemyController.transform.position);
        
        Vector3 targetRotation;
        if (distance > 3f)
        {
            targetRotation = new Vector3(-30, 0, 0);
            attackPower = 9;
        }
        else if (distance > 2f)
        {
            targetRotation = new Vector3(-50, 0, 0);
            attackPower = 4;
        }
        else
        {
            targetRotation = new Vector3(-70, 0, 0);
            attackPower = 3;
        }
        _enemyController.muzzle.transform.localEulerAngles = targetRotation;
    }

    public void OperateExit(EnemyController sender)
    {
        
    }
}
