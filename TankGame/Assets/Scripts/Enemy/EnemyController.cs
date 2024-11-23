using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IDamagable
{
    public GameObject _player;
    public bool isEnemyTurn = false;
    [SerializeField] private int enemyHp = 5;
    [HideInInspector] public int enemyX;
    [HideInInspector] public int enemyY;
    public LayerMask layerMask;
    public float maxFuel;

    public bool isJudgment = false;
    
    public enum EnemyState
    {
        Move,
        Attack
    }
    private IState<EnemyController> _currentState;
    private Dictionary<EnemyState, IState<EnemyController>> _dicState =
        new Dictionary<EnemyState, IState<EnemyController>>();
    private void Start()
    {
        enemyHp = 5;
        IState<EnemyController> Move = new EnemyMove();
        IState<EnemyController> Attack = new EnemyAttack();
        _dicState.Add(EnemyState.Move,Move);
        _dicState.Add(EnemyState.Attack,Attack);
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if (!isJudgment)
            {
                StateJudgment();    
            }
        }
        else
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        if (isEnemyTurn)
        {
            _currentState?.OperateUpdate(this);    
        }
    }

    private void StateJudgment()
    {
        isJudgment = true;
        if (Vector3.Distance(_player.transform.position,transform.position) < 6f)
        {
            Debug.Log("공격으로 전환");
            ChangeState(EnemyState.Attack);    
        }
        else
        {
            Debug.Log("이동으로 전환");
            ChangeState(EnemyState.Move);
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