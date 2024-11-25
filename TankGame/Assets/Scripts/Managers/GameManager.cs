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
    public List<GameObject> TurnList = new List<GameObject>(); 
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
        GameObject gameObject = TurnList[0];
        TurnList.Remove(gameObject);
        TurnList.Add(gameObject);
        if (TurnList[0].CompareTag("Player"))
        {
            playerController = TurnList[0].GetComponent<PlayerController>();
            playerController.isPlayerTurn = true;
            nowTurn = "Player Turn";
        }
        else
        {
            enemyController = TurnList[0].GetComponent<EnemyController>();
            enemyController.isEnemyTurn = true;
            nowTurn = "Enemy Turn";
        }
        UIManager.Instance.TurnTextChange();
    }
}
