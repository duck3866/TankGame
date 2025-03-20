using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Windows;
using File = UnityEngine.Windows.File;

public class SaveManager : MonoBehaviour
{
   public string GameDataPath;
   public static SaveManager instance;
   public List<bool> stageSaveDic = new List<bool>();
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

      // JSON에서 불러오기 전에 기본값 설정
      for (int i = 0; i < 9; i++)
      {
         stageSaveDic.Add(false);
      }

      // List<bool> 직접 로드
      List<bool> loadedData = LoadJsonFile<List<bool>>(Application.dataPath, "SaveData");

      if (loadedData != null)
      {
         stageSaveDic = loadedData;
         Debug.Log("데이터 로드 성공!");
      }
      else
      {
         Debug.Log("저장된 데이터 없음. 기본값 사용.");
      }
   }


   public void ClearStage(int index)
   {
      stageSaveDic[index] = true;
      string jsonData = ObjectToJson(stageSaveDic); // 리스트를 직접 JSON으로 변환
      CreateJsonFile(Application.dataPath, "SaveData", jsonData);
   }


   private void CreateJsonFile(string createPath, string fileName, string jsonData)
   {
      string fullPath = Path.Combine(createPath, fileName+".json");
      byte[]  bytes = Encoding.UTF8.GetBytes(jsonData);
      using (FileStream fs = new FileStream(fullPath, FileMode.Create))
      {
         fs.Write(bytes, 0, bytes.Length);
      }
   }
   private T LoadJsonFile<T>(string loadPath, string fileName)
   {
      string fullPath = Path.Combine(loadPath,fileName + ".json");
      if (!File.Exists(fullPath))
      {
         Debug.Log("파일이 없습니다!");
         return default;
      }

      string jsonData;
      using (FileStream fs = new FileStream(fullPath, FileMode.Open,FileAccess.Read))
      using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
      {
         jsonData = sr.ReadToEnd();
      }

      return JsonToObject<T>(jsonData);
   }
   
   public string ObjectToJson(object obj)
   {
      return JsonConvert.SerializeObject(obj,Formatting.Indented);
   }
   public T JsonToObject<T>(string json)
   {
      return JsonConvert.DeserializeObject<T>(json);
   }
}
