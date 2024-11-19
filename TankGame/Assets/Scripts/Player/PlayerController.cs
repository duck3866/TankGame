using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public bool isAttackMode = false;
    public LayerMask layerMask;
    public float maxFuel;

    public GameObject muzzle;
    public GameObject turret;
    public GameObject bulletFactory;
    public GameObject shootingPoint;

    public Projectile Projectile; 
        
    public int playerX;
    public int playerY;

    public enum PlayerState
    {
        Move,
        Attack
    }

    private IState<PlayerController> _currentState;
    private Dictionary<PlayerState, IState<PlayerController>> _dicState =
        new Dictionary<PlayerState, IState<PlayerController>>();
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        IState<PlayerController> Move = new PlayerMove();
        IState<PlayerController> Attack = new PlayerAttack();
        _dicState.Add(PlayerState.Move,Move);
        _dicState.Add(PlayerState.Attack,Attack);
        
        ChangeState(PlayerState.Move);
    }
    public void ChangeState(PlayerState newState)
    {
        _currentState?.OperateExit(this);
        _currentState = _dicState[newState];
        _currentState.OperateEnter(this);
    }
    public void Update()
    {
        _currentState?.OperateUpdate(this);
    }
}

public class Node
{
    public Vector3 Position { get; set; }
    public Node Parent { get; set; }
    public float GCost { get; set; }// 이동 비용(시작->현재)
    public float HCost { get; set; }// 휴리스틱 비용(현재->목표)
    public float FCost => GCost + HCost; // 총비용

    public Node(Vector3 position)
    {
        Position = position;
        Parent = null;
        GCost = 0;
        HCost = 0;
    }
}