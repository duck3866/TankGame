using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour,IDamagable
{
    public LineRenderer lineRenderer;
    public bool isPlayerTurn = false;
    public LayerMask layerMask;
    public float maxFuel;

    public GameObject muzzle;
    public GameObject turret;
    public GameObject shootingPoint;

    public int playerHp = 5;
    public int playerTurn = 0;
    
    [HideInInspector] public int playerX;
    [HideInInspector] public int playerY;

    public enum PlayerState
    {
        Move,
        Attack,
        Idle
    }
    
    public AudioSource audioSource;
    private IState<PlayerController> _currentState;
    private Dictionary<PlayerState, IState<PlayerController>> _dicState =
        new Dictionary<PlayerState, IState<PlayerController>>();
    private void Start()
    {
        playerHp = 5;
        playerTurn = 0;
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        IState<PlayerController> Move = new PlayerMove();
        IState<PlayerController> Attack = new PlayerAttack();
        _dicState.Add(PlayerState.Move,Move);
        _dicState.Add(PlayerState.Attack,Attack);
        ChangeState(PlayerState.Move);

        isPlayerTurn = true;
    }
    public void ChangeState(PlayerState newState)
    {
        _currentState?.OperateExit(this);
        _currentState = _dicState[newState];
        _currentState.OperateEnter(this);
    }
    public void Update()
    {
        if (isPlayerTurn)
        {
            _currentState?.OperateUpdate(this);   
        }
    }

    public void TakeDamage(int hitPower)
    {
        if (playerHp >= 1)
        {
            playerHp -= hitPower; 
            UIManager.Instance.CheckImage();
        }
        else
        {
            UIManager.Instance.DiePanel();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        maxFuel = 10;
        playerHp = 5;
        playerTurn = 0;
        isPlayerTurn = true;
        UIManager.Instance.Initialized();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,3f);
    }
}