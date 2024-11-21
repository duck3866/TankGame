using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMove : IState<PlayerController>
{
    private PlayerController _playerController;

    private float _moveDelay = 0.1f;
    private List<Renderer> _pathRenderers = new List<Renderer>(); // 경로 블록의 렌더러들
    private bool _pathDrawn = false; // 클릭했는지 체크
    private List<Vector3> _currentPath = new List<Vector3>(); // 현재 경로 저장 리스트

    private float _fuel;

    private float _weight;
    private bool _isInit = false;
    private Collider[] _colliders;

    private void Initialize()
    {
        _fuel = _playerController.maxFuel;
        _colliders = new Collider[DataManager.Instance.datas.Stage[0].horizontal *
                                 DataManager.Instance.datas.Stage[0].vertical];
    }

    public void OperateEnter(PlayerController _player)
    {
        _playerController = _player;
        if (!_isInit)
        {
            _isInit = true;
            Initialize();
        }
    }

    public void OperateUpdate(PlayerController _player)
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Vector3 targetPos = new Vector3(raycastHit.transform.position.x, _playerController.transform.position.y,
                    raycastHit.transform.position.z);

                if (!_pathDrawn)
                {
                    // 경로 계산 및 시각화
                    _currentPath = FindPath(_playerController.transform.position, targetPos);
                    if (_currentPath.Count > 0)
                    {
                        HighlightPath(_currentPath);
                        _pathDrawn = true;
                    }
                }
                else
                {
                    _playerController.StartCoroutine(MoveAlongPath(_currentPath));
                    // ResetPathColors();
                    _pathDrawn = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            CancelPath();
        }
    }

    public void OperateExit(PlayerController _player)
    {
        Debug.Log("무브 -> 공격");
    }

    private void HighlightPath(List<Vector3> path)
    {
        // 현재 위치도 경로 색상으로 변경
        Vector3 currentPosition = _playerController.transform.position;
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
            Physics.OverlapSphereNonAlloc(position, 0.1f, _colliders, _playerController.layerMask); // 해당 위치에 있는 블록 찾기
        for (int i = 0; i < collidersCount; i++)
        {
            Renderer renderer = _colliders[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!_pathRenderers.Contains(renderer))
                {
                    _pathRenderers.Add(renderer);
                    renderer.material.color = Color.blue; // 블록의 색상을 변경
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

    private void CancelPath()
    {
        if (_pathDrawn)
        {
            ResetPathColors();
            _pathDrawn = false;
            _currentPath.Clear(); // 경로 데이터 초기화
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
            if (_fuel < 1)
            {
                break;
            }

            MapManager.Instace.MapList[_playerController.playerX, _playerController.playerY] = 0;
            if (step.x > _playerController.transform.position.x)
            {
                _playerController.playerX += 1;
                PlayerMoving(Vector3.right);
            }
            else if (step.x < _playerController.transform.position.x)
            {
                _playerController.playerX -= 1;
                PlayerMoving(Vector3.left);
            }
            else if (step.z > _playerController.transform.position.z)
            {
                _playerController.playerY += 1;
                PlayerMoving(Vector3.forward);
            }
            else if (step.z < _playerController.transform.position.z)
            {
                _playerController.playerY -= 1;
                PlayerMoving(Vector3.back);
            }
            MapManager.Instace.MapList[_playerController.playerX, _playerController.playerY] = 1;
            _pathRenderers[0].material.color = Color.white;
            Renderer renderer = _pathRenderers[0];
            _pathRenderers.Remove(renderer);
            _fuel -= 1;
            yield return new WaitForSeconds(_moveDelay);
        }

        _fuel = _playerController.maxFuel;
        _weight = 0;
        ResetPathColors();
        _playerController.ChangeState(PlayerController.PlayerState.Attack);
    }

    private void PlayerMoving(Vector3 dir)
    {
        _playerController.transform.position += dir * 1;
        _playerController.transform.forward = dir;
    }
}