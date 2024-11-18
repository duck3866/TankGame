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

    [SerializeField] private float moveDelay;
    [SerializeField] private bool isAttackMode = false;
    
    
    private List<Renderer> pathRenderers = new List<Renderer>(); // 경로 블록의 렌더러들
    private bool pathDrawn = false; // 경로 시각화 여부 플래그
    private List<Vector3> currentPath = new List<Vector3>(); // 현재 경로 저장 리스트
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float fuel;
    [SerializeField] private float maxFuel;
    private float hap;

    private Collider[] colliders;

    public int playerX;
    public int playerY;

    public enum PlayerState
    {
        Ready,
        Move,
        Shoot,
        End
    }

    private PlayerState _playerState;
    private void Start()
    {
        fuel = maxFuel;
        colliders = new Collider[DataManager.Instance.datas.Stage[0].horizontal *
                                 DataManager.Instance.datas.Stage[0].vertical];
        _playerState = PlayerState.Ready;
    }
  
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Vector3 targetPos = new Vector3(raycastHit.transform.position.x, transform.position.y, raycastHit.transform.position.z);

                if (!pathDrawn)
                {
                    // 경로 계산 및 시각화
                    currentPath = FindPath(transform.position, targetPos);
                    if (currentPath.Count > 0)
                    {
                        HighlightPath(currentPath);
                        pathDrawn = true;
                    }
                }
                else
                {
                    // 경로 따라 이동 및 색상 초기화
                    StartCoroutine(MoveAlongPath(currentPath));
                    // ResetPathColors();
                    pathDrawn = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            CancelPath();
        }
    }
    private void HighlightPath(List<Vector3> path)
    {
        // 현재 위치도 경로 색상으로 변경
        Vector3 currentPosition = transform.position;
        HighlightBlockAtPosition(currentPosition);
        foreach (Vector3 position in path)
        {
            if (hap >= fuel)
            {
                return;
            }
            HighlightBlockAtPosition(position);
            hap++;
        }
    }

    private void HighlightBlockAtPosition(Vector3 position)
    {
        int collidersCount = Physics.OverlapSphereNonAlloc(position, 0.1f,colliders,layerMask); // 해당 위치에 있는 블록 찾기
        for (int i = 0; i < collidersCount; i++)
        {
            Renderer renderer = colliders[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!pathRenderers.Contains(renderer))
                {
                    pathRenderers.Add(renderer);
                    renderer.material.color = Color.blue; // 블록의 색상을 변경
                }
            }
        }
    }
    private void ResetPathColors()
    {
        foreach (Renderer renderer in pathRenderers)
        {
            if (renderer != null)
            {
                renderer.material.color = Color.white; // 원래 색상으로 복구
            }
        }
        pathRenderers.Clear(); // 리스트 초기화
    }

    private void CancelPath()
    {
        if (pathDrawn)
        {
            ResetPathColors();
            pathDrawn = false;
            currentPath.Clear(); // 경로 데이터 초기화
        }
    }

    private List<Vector3> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(startPosition);
        Node targetNode = new Node(targetPosition);
        
        openList.Add(startNode);
        while (openList.Count > 0)
        {
            //현재 비용을 찾는다. (FCost(총비용)가 낮은거)
            Node currentNode = openList.OrderBy(node => node.FCost).First();
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            //도착했으면
            if (currentNode.Position == targetPosition)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (var neighborPos in GetNeighbors(currentNode.Position))
            {
                Node neighborNode = new Node(neighborPos);
                if (closedList.Any(node => node.Position == neighborNode.Position))
                    continue;
                // 비용 계산?
                float newGCost = currentNode.GCost + Vector3.Distance(currentNode.Position, neighborNode.Position);
                if (newGCost < neighborNode.GCost || openList.All(node => node.Position != neighborNode.Position))
                {
                    neighborNode.GCost = newGCost;
                    neighborNode.HCost = Vector3.Distance(neighborNode.Position, targetNode.Position);
                    neighborNode.Parent = currentNode;
                    if (openList.All(node => node.Position != neighborNode.Position))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        return new List<Vector3>();
    }

    private List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        // 시작점에서 목표점 순서로 경로를 반환
        path.Reverse();
        return path;
    }

    private List<Vector3> GetNeighbors(Vector3 currentPos)
    {
        List<Vector3> neighbors = new List<Vector3>();
        neighbors.Add(currentPos + Vector3.right);
        neighbors.Add(currentPos + Vector3.left);
        neighbors.Add(currentPos + Vector3.forward);
        neighbors.Add(currentPos + Vector3.back);

        return neighbors;
    }
    private IEnumerator MoveAlongPath(List<Vector3> path)
    {
        foreach (Vector3 step in path)
        {
            if (fuel < 1)
            {
                break;
            }
            if (step.x > transform.position.x)
            {
                playerX += 1;
                PlayerMoving(Vector3.right);   
            }
            else if (step.x < transform.position.x)
            {
                playerX -= 1;
                PlayerMoving(Vector3.left);  
            }
            else if (step.z > transform.position.z)
            {
                playerY += 1;
                PlayerMoving(Vector3.forward);
            }
            else if (step.z < transform.position.z)
            {
                playerY -= 1;
                PlayerMoving(Vector3.back);
            }

            pathRenderers[0].material.color = Color.white;
            Renderer renderer = pathRenderers[0];
            pathRenderers.Remove(renderer);
            fuel -= 1;
            hap = 0;
            yield return new WaitForSeconds(moveDelay);
        }
        fuel = maxFuel;
        ResetPathColors();
    }
    private void PlayerMoving(Vector3 dir)
    {
        transform.position += dir * 1;
        transform.forward = dir;
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