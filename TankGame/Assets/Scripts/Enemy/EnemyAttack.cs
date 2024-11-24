using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<EnemyController>
{
    private EnemyController _enemyController;
    private bool isShoot = false;
    public void OperateEnter(EnemyController sender)
    {
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
        Debug.Log("이거 매번 해야해");
        yield return new WaitForSeconds(2f);

       
        GameObject currentObject = _enemyController.turret;
        
        Vector3 targetPosition = _enemyController._player.transform.position;
        Vector3 shootingPosition = _enemyController.shootingPoint.transform.position;
        targetPosition.y = shootingPosition.y;
        
        currentObject.transform.LookAt(targetPosition);

        GameObject bullet = BulletManager.Instace.GetObject();
        bullet.transform.position = shootingPosition;
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = Vector3.zero; // 기존 속도 초기화
        // Vector3 direction = (targetPosition - shootingPosition).normalized;
        bulletRigidbody.AddForce(currentObject.transform.forward * 9f, ForceMode.Impulse);
        
        _enemyController.isJudgment = false;
        GameManager.Instance.TurnChange("Player");
    }


    public void OperateExit(EnemyController sender)
    {
        GameManager.Instance.TurnChange("Player");
    }
}
