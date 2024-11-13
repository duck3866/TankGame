using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataManager : MonoBehaviour
{
    public TextAsset data;
    private AllData _datas;

    public GameObject block;
    public Transform parent;

    public GameObject player;

    [SerializeField] private int poolSize;
    private GameObject[] _objectList;
    [SerializeField] private int _mapIndex;
    
    [SerializeField] private float dropHeight = 10f;  // 블록이 시작할 높이
    [SerializeField] private float dropDuration = 1f; // 떨어지는데 걸리는 시간
    [SerializeField] private float blockDelay = 0.05f; // 블록간 딜레이

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
        player.SetActive(false);
    }
    private void Start()
    {
        ChangeMap(_mapIndex);
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
    private void ChangeMap(int value)
    {
        StartCoroutine(FallingBlock(value));
    }
    
    private IEnumerator FallingBlock(int value)
    {
        int x = _datas.Stage[value].horizontal;
        int y = _datas.Stage[value].vertical;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject obj = GetObject();
                StartCoroutine(DropBlock(obj, new Vector3(i, dropHeight, j), new Vector3(i, 0, j)));
                yield return new WaitForSeconds(blockDelay);
            }
        }
        int playerX = _datas.Stage[value].x;
        int playerY = _datas.Stage[value].y;
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

    public void OnClickPlus()
    {
        _mapIndex++;
        ResetList();
        ChangeMap(_mapIndex);
    }
    public void OnClickMinus()
    {
        _mapIndex--;
        ResetList();
        ChangeMap(_mapIndex);
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