using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour,IDamagable
{
    [SerializeField] private Image[] images;
    public GameObject _player;
    public bool isEnemyTurn = false;
    [SerializeField] private int enemyHp = 5;
    [HideInInspector] public int enemyX;
    [HideInInspector] public int enemyY;
    public LayerMask layerMask;
    public float maxFuel;

    public GameObject muzzle;
    public GameObject turret;
    
    public GameObject shootingPoint;

    private AudioSource AudioSource;
    
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
        AudioSource = GetComponent<AudioSource>();
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
    public void CheckImage()
    { 
        for (int i = 1; i <= images.Length; i++)
        {
            if (enemyHp < i)
            {
                images[i-1].color = Color.white;
            }
            else
            {
                images[i-1].color = Color.red;
            }
        }
    }
    private void StateJudgment()
    {
        isJudgment = true;
        if (Vector3.Distance(_player.transform.position,transform.position) < 6f)
        {
            // Debug.Log("공격으로 전환");
            ChangeState(EnemyState.Attack);    
        }
        else
        {
            // Debug.Log("이동으로 전환");
            ChangeState(EnemyState.Move);
        }
    }
    private void OnEnable()
    {
        isJudgment = false;
        enemyHp = 5;
        isEnemyTurn = false;
        CheckImage();
    }

    public void PlaySound()
    {
        AudioSource.Play();
    }
    private void OnDestroy()
    {
        GameManager.Instance.TurnList.Remove(this.gameObject);
        if (isEnemyTurn)
        {
            GameManager.Instance.TurnChange();
        }
        MapManager.Instace.EnemyDieCheck();
        MapManager.Instace.enemyList.Remove(this.gameObject);
        ChangeState(EnemyState.Move);
    }

    public void TakeDamage(int hitPower)
    {
        if (enemyHp > 1)
        {
            enemyHp -= hitPower;
        }
        else
        {
            Destroy(gameObject);
        }
        CheckImage();
    }

    public void ChangeState(EnemyState newState)
    {
        _currentState?.OperateExit(this);
        _currentState = _dicState[newState];
        _currentState.OperateEnter(this);
    }
}