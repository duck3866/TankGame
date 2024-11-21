using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IDamagable
{
    public bool isEnemyTurn = false;
    [SerializeField] private int enemyHp = 5;
    [HideInInspector] public int enemyX;
    [HideInInspector] public int enemyY;
 

    // Update is called once per frame
    void Update()
    {
        if (isEnemyTurn)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (enemyX < MapManager.Instace.x - 1)
                {
                    enemyX += 1;
                    PlayerMoving(Vector3.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (enemyX > 0)
                {
                    enemyX -= 1;
                    PlayerMoving(Vector3.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (enemyY < MapManager.Instace.y - 1)
                {
                    enemyY += 1;
                    PlayerMoving(Vector3.forward);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (enemyY > 0)
                {
                    enemyY -= 1;
                    PlayerMoving(Vector3.back);
                }
            }
        }
    }
    private void PlayerMoving(Vector3 dir)
    {
        transform.position += dir * 1;
        transform.forward = dir;
        GameManager.Instance.TurnChange("Player");
    }

    private void OnEnable()
    {
        enemyHp = 5;
        isEnemyTurn = false;
    }

    private void OnDisable()
    {
        MapManager.Instace.EnemyDieCheck();
    }

    public void TakeDamage(int hitPower)
    {
        if (enemyHp >= 1)
        {
            enemyHp -= hitPower;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}