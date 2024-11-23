using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IDamagable
{
    private GameObject _player;
    public bool isEnemyTurn = false;
    [SerializeField] private int enemyHp = 5;
    [HideInInspector] public int enemyX;
    [HideInInspector] public int enemyY;
    public LayerMask layerMask;
    public float maxFuel;
    
    public enum EnemyState
    {
        Move,
        Attack,
        Idle
    }
    private IState<EnemyController> _currentState;
    private Dictionary<EnemyState, IState<EnemyController>> _dicState =
        new Dictionary<EnemyState, IState<EnemyController>>();
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        enemyHp = 5;
        IState<EnemyController> Move = new EnemyMove();
        IState<EnemyController> Attack = new EnemyAttack();
        _dicState.Add(EnemyState.Move,Move);
        _dicState.Add(EnemyState.Attack,Attack);
        ChangeState(EnemyState.Move);
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnemyTurn)
        {
            _currentState?.OperateUpdate(this);    
        }
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

    public void ChangeState(EnemyState newState)
    {
        _currentState?.OperateExit(this);
        _currentState = _dicState[newState];
        _currentState.OperateEnter(this);
    }
}