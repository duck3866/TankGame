using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SaveManager : MonoBehaviour
{
   public string GameDataPath;
   public static SaveManager instance;
   public Dictionary<string,bool> stageSaveDic = new Dictionary<string,bool>();
   private SaveData saveData;
   public void Awake()
   {
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(this);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void Start()
   {
      if (instance != this)
      {
         Destroy(gameObject);
      }

      for (int i = 0; i < 9; i++)
      {
         stageSaveDic.Add(i.ToString(), false);
      }
      Load();
   }

   public void ClearStage(int index)
   {
      stageSaveDic[index.ToString()] = true;
   }
   public void Save()
   {
      
   }

   public void Load()
   {
      string filePath = Application.persistentDataPath + GameDataPath;
      if (File.Exists(filePath))
      {
         Debug.Log("로딩 성공!");
         string FromJsonData = System.IO.File.ReadAllText(filePath);
         saveData = JsonUtility.FromJson<SaveData>(FromJsonData);
         stageSaveDic = saveData.stageSaveDictionary;
      }
      else
      {
         saveData = new SaveData();
      }
   }
}
