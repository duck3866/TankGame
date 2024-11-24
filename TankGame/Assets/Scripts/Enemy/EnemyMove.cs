using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EnemyMove : IState<EnemyController>
{
    private EnemyController _enemyController;
    
    private float _moveDelay = 0.1f;
    private List<Renderer> _pathRenderers = new List<Renderer>(); // 경로 블록의 렌더러들
    // private bool _pathDrawn = false; // 클릭했는지 체크
    private List<Vector3> _currentPath = new List<Vector3>(); // 현재 경로 저장 리스트
    private float _fuel;

    private float _weight;
    private bool _isInit = false;
    
    private Collider[] _colliders;

    private bool _isJug = false;
     private void Initialize()
    {
        _fuel = _enemyController.maxFuel;
        _colliders = new Collider[DataManager.Instance.datas.Stage[0].horizontal *
                                 DataManager.Instance.datas.Stage[0].vertical];
    }

    public void OperateEnter(EnemyController _enemy)
    {
        _enemyController = _enemy;
        _isJug = false;
        if (!_isInit)
        {
            _isInit = true;
            Initialize();
        }

        
    }

    public void OperateUpdate(EnemyController _enemy)
    {
        if (_enemyController.isEnemyTurn && !_isJug)
        {
            Collider[] hitColliders = Physics.OverlapSphere(_enemyController._player.transform.position, 3f, _enemyController.layerMask);
            if (hitColliders != null)
            {
                int randomPos = Random.Range(0, hitColliders.Length - 1);
                Debug.Log(hitColliders[randomPos].name);
                Vector3 startPos = new Vector3(hitColliders[randomPos].transform.position.x,_enemyController.transform.position.y,hitColliders[randomPos].transform.position.z);
                _currentPath = FindPath(_enemyController.transform.position, startPos);
                if (_currentPath.Count > 0)
                {
                    HighlightPath(_currentPath);
                }
            }
            _isJug = true;
            _enemyController.StartCoroutine(WaitMove());
        }
    }

    private IEnumerator WaitMove()
    {
        yield return new WaitForSeconds(1f); 
        _enemyController.StartCoroutine(MoveAlongPath(_currentPath));
    }

    public void OperateExit(EnemyController _enemy)
    {
        Debug.Log("무브 -> 공격");
    }

    private void HighlightPath(List<Vector3> path)
    {
        // 현재 위치도 경로 색상으로 변경
        Vector3 currentPosition = _enemyController.transform.position;
        HighlightBlockAtPosition(currentPosition);
        foreach (Vector3 position in path)
        {
            if (_weight >= _fuel)
            {
                return;
            }

            HighlightBlockAtPosition(position);
            _weight++;
        }
    }

    private void HighlightBlockAtPosition(Vector3 position)
    {
        int collidersCount =
            Physics.OverlapSphereNonAlloc(position, 0.1f, _colliders, _enemyController.layerMask); // 해당 위치에 있는 블록 찾기
        for (int i = 0; i < collidersCount; i++)
        {
            Renderer renderer = _colliders[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!_pathRenderers.Contains(renderer))
                {
                    _pathRenderers.Add(renderer);
                    renderer.material.color = Color.red; // 블록의 색상을 변경
                }
            }
        }
    }

    private void ResetPathColors()
    {
        foreach (Renderer renderer in _pathRenderers)
        {
            if (renderer != null)
            {
                renderer.material.color = Color.white; // 원래 색상으로 복구
            }
        }

        _pathRenderers.Clear(); // 리스트 초기화
    }

    // private void CancelPath()
    // {
    //     if (_pathDrawn)
    //     {
    //         ResetPathColors();
    //         _pathDrawn = false;
    //         _currentPath.Clear(); // 경로 데이터 초기화
    //     }
    // }

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
            if (_fuel < 1)
            {
                break;
            }

            MapManager.Instace.MapList[_enemyController.enemyX, _enemyController.enemyY] = 0;
            if (step.x > _enemyController.transform.position.x)
            {
                _enemyController.enemyX += 1;
                EnemyMoving(Vector3.right);
            }
            else if (step.x < _enemyController.transform.position.x)
            {
                _enemyController.enemyX -= 1;
                EnemyMoving(Vector3.left);
            }
            else if (step.z > _enemyController.transform.position.z)
            {
                _enemyController.enemyY += 1;
                EnemyMoving(Vector3.forward);
            }
            else if (step.z < _enemyController.transform.position.z)
            {
                _enemyController.enemyY -= 1;
                EnemyMoving(Vector3.back);
            }
            MapManager.Instace.MapList[_enemyController.enemyX, _enemyController.enemyY] = 1;
            _pathRenderers[0].material.color = Color.white;
            Renderer renderer = _pathRenderers[0];
            _pathRenderers.Remove(renderer);
            _fuel -= 1;
            yield return new WaitForSeconds(_moveDelay);
        }

        _fuel = _enemyController.maxFuel;
        _weight = 0;
        ResetPathColors();
        _enemyController.isJudgment = false;
        GameManager.Instance.TurnChange("Player");
        // _enemyController.ChangeState(EnemyController.EnemyState.Attack);
    }

    private void EnemyMoving(Vector3 dir)
    {
        _enemyController.transform.position += dir * 1;
        _enemyController.transform.forward = dir;
    }
}
