using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public TextAsset data;
    private AllData _datas;

    public GameObject block;
    public Transform env;

    public GameObject player;

    [SerializeField] private int poolSize;
    private GameObject[] _objectList;
    private void Awake()
    {
        _objectList = new GameObject[poolSize];
        // for (int i = 0; i < poolSize; i++)
        // {
        //     GameObject obj = Instantiate(block, env);
        //     obj.SetActive(false);
        //     _objectList[i] = obj;
        // }
        _datas = JsonUtility.FromJson<AllData>(data.text);
    }
    //
    // private GameObject GetObject()
    // {
    //     int count = 0;
    //     for (int i = 0; i < poolSize; i++)
    //     {
    //         if (_objectList[i].activeSelf == false)
    //         {
    //             Debug.Log(i);
    //             _objectList[i].SetActive(true);
    //             count = i;
    //             break;
    //         }
    //     }
    //     return _objectList[count];
    // }
    

    void Start()
    {
        int x = _datas.Stage[0].horizontal;
        int y = _datas.Stage[0].vertical;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject obj = Instantiate(block, env);
                obj.transform.position = new Vector3(i, 0, j);
            }
        }
        int playerX = _datas.Stage[0].x;
        int playerY = _datas.Stage[0].y;
        GameObject playerTank = Instantiate(player);
        playerTank.transform.position = new Vector3(playerX, 0.7f, playerY);
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