using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TextAsset data;
    private AllData _datas;

    public GameObject block;
    public Transform parent;

    public GameObject player;

    [SerializeField] private int poolSize;
    private GameObject[] _objectList;
    private int _mapIndex;
    
    private void Awake()
    {
        _mapIndex = 0;
        
        poolSize *= 10;
        _objectList = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(block, parent);
            obj.SetActive(false);
            _objectList[i] = obj;
        }
        ResetList();
        _datas = JsonUtility.FromJson<AllData>(data.text);
        player = Instantiate(player);
    }

    private void ResetList()
    {
        
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

    public void OnClickButton(int index)
    {
        player.SetActive(false);
        ResetList();
        if (index > 0)
        {
            if (_mapIndex > 0)
            {
                _mapIndex--;    
            }
        }
        else if(index < 0)
        {
            if (_mapIndex < _datas.Stage.Length - 1)
            {
                _mapIndex++;    
            }
        }
        ChangeMap(_mapIndex);
    }

    private void ChangeMap(int value)
    {
        int x = _datas.Stage[value].horizontal;
        int y = _datas.Stage[value].vertical;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject obj = GetObject();
                obj.transform.position = new Vector3(i, 0, j);
            }
        }
        int playerX = _datas.Stage[value].x;
        int playerY = _datas.Stage[value].y;
        player.SetActive(true);
        player.transform.position = new Vector3(playerX, 0.55f, playerY);
    }
    private void Start()
    {
        ChangeMap(0);
    }
}
[Serializable]
public class AllData
{
    public MapData[] Stage;
}
[Serializable]
public class MapData
{
    public int stage;
    public string name;
    public int horizontal;
    public int vertical;
    public int x;
    public int y;
}