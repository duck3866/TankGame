using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instace = null;
    
    public GameObject block;
    public Transform parent;

    public GameObject player;
    private PlayerController _playerController;
    
    [SerializeField] private int poolSize;
    private GameObject[] _objectList;
    [SerializeField] private int mapIndex;
    
    [SerializeField] private float dropHeight = 10f;  // 블록이 시작할 높이
    [SerializeField] private float dropDuration = 1f; // 떨어지는데 걸리는 시간
    [SerializeField] private float blockDelay = 0.05f; // 블록간 딜레이

    public int[,] MapList;

    public int x;
    public int y;
    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        player = Instantiate(player);
        _playerController = player.GetComponent<PlayerController>();
        player.transform.position = new Vector3(-5, 10, 5);
    }

    private void Start()
    {
        mapIndex = 0;
        poolSize = DataManager.Instance.datas.Stage[0].horizontal * DataManager.Instance.datas.Stage[0].vertical;
        _objectList = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(block, parent);
            obj.SetActive(false);
            _objectList[i] = obj;
        }
        ResetList();
        ChangeMap(mapIndex);
        
    }
    private void ResetList()
    {
        player.SetActive(false);
        for (int i = 0; i < poolSize; i++)
        {
            _objectList[i].SetActive(false);
        }
    }
    
    private GameObject GetObject()
    {
        int count = 0;
        
        for (int i = 0; i < poolSize; i++)
        {
            if (_objectList[i].activeSelf == false)
            {
                _objectList[i].SetActive(true);
                count = i;
                break;
            }
        }
        return _objectList[count];
    }
        
    private void ChangeMap(int value)
    {
        StartCoroutine(FallingBlock(value));
    }
    
    

    public void OnClickPlus()
    {
        mapIndex++;
        ResetList();
        ChangeMap(mapIndex);
    }
    public void OnClickMinus()
    {
        mapIndex--;
        ResetList();
        ChangeMap(mapIndex);
    }
    private IEnumerator FallingBlock(int value)
    {
        x = DataManager.Instance.datas.Stage[value].horizontal;
        y = DataManager.Instance.datas.Stage[value].vertical;
        MapList = new int[x, y];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                MapList[i, j] = 0;
                GameObject obj = GetObject();
                StartCoroutine(DropBlock(obj, new Vector3(i, dropHeight, j), new Vector3(i, 0, j)));
                obj.name = i + "/" + j; 
                yield return new WaitForSeconds(blockDelay);
            }
        }
        int playerX = DataManager.Instance.datas.Stage[value].x;
        int playerY = DataManager.Instance.datas.Stage[value].y;
        // MapList[playerX, playerY] = 1;
        _playerController.playerX = playerX;
        _playerController.playerY = playerY;
        
        player.SetActive(true);
        player.transform.position = new Vector3(playerX, 0.55f, playerY);
        
        yield return null;
    }

    private IEnumerator DropBlock(GameObject obj, Vector3 startPos, Vector3 endPos)
    {
        float elapsed = 0f;
        obj.transform.position = startPos;
        
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / dropDuration;
            float easeProgress = 1f - Mathf.Pow(1f - progress, 3);
            obj.transform.position = Vector3.Lerp(startPos, endPos, easeProgress);
            yield return null;
        }
        
        obj.transform.position = endPos;
    }
}
