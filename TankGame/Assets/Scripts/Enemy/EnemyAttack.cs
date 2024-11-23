using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<EnemyController>
{
    private EnemyController _enemyController;
    public void OperateEnter(EnemyController sender)
    {
        _enemyController = sender;
    }

    public void OperateUpdate(EnemyController sender)
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("dsds");
            _enemyController.ChangeState(EnemyController.EnemyState.Move);
        }
    }

    public void OperateExit(EnemyController sender)
    {
        GameManager.Instance.TurnChange("Player");
    }
}
