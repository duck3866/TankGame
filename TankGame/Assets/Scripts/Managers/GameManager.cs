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
    public Queue<GameObject> TurnList = new Queue<GameObject>(); 
    public string nowTurn;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void TurnChange()
    {
        GameObject gameObject = TurnList.Peek();
        TurnList.Enqueue(gameObject);
        TurnList.Dequeue();
        if (TurnList.Peek().CompareTag("Player"))
        {
            playerController = TurnList.Peek().GetComponent<PlayerController>();
            playerController.isPlayerTurn = true;
            nowTurn = "Player Turn";
        }
        else
        {
            enemyController = TurnList.Peek().GetComponent<EnemyController>();
            enemyController.isEnemyTurn = true;
            nowTurn = "Enemy Turn";
        }
        UIManager.Instance.TurnTextChange();
    }
}
