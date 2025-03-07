using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    public TextAsset data;
    public AllData datas;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        datas = JsonUtility.FromJson<AllData>(data.text);
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
    public int playerX;
    public int playerY;
    public int enemyX;
    public int enemyY;
    public int enemyCount;
}