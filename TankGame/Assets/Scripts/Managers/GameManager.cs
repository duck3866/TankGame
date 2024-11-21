using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerController playerController;
    public static GameManager Instance = null;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public EnemyController enemyController;

    public string nowTurn;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }
        }
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                enemyController = enemy.GetComponent<EnemyController>();
            }
        }
    }
    public void TurnChange(string name)
    {
        switch (name)
        {
            case "Player":
                playerController.isPlayerTurn = true;
                enemyController.isEnemyTurn = false;
                nowTurn = "Player Turn";
                break;
            case "Enemy":
                enemyController.isEnemyTurn = true;
                playerController.isPlayerTurn = false;
                nowTurn = "Enemy Turn";
                break;
        }
        UIManager.Instance.TurnTextChange();
    }
}
