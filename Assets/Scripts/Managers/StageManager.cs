using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public int index = 0;

    public void Awake()
    {
        if (instance == null)
        {
            Debug.Log("비어있으니까 채운다.");
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("새로 만든거 죽인다.");
            Destroy(gameObject);
            return;
        }
        
    }

    public void Start()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnStageButtonClicked(int clickIndex)
    {
        index = clickIndex;
        Debug.Log(index+" 와우 섹123스");
        SceneManager.LoadScene("Main");
    }
}
