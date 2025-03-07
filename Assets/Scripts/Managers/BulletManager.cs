using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject[] bulletList;
    public static BulletManager Instace = null;
    public GameObject bulletFactory;
    private void Awake()
    {
         
        if (Instace == null)
        {
            Instace = this;
        }
        bulletList = new GameObject[50];
        for (int i = 0; i < 50; i++)
        {
            GameObject bullet = Instantiate(bulletFactory,transform);
            bulletList[i] = bullet;
            bullet.SetActive(false);
        }
    }
    public GameObject GetObject()
    {
        int count = 0;
        
        for (int i = 0; i < 50; i++)
        {
            if (bulletList[i].activeSelf == false)
            {
                bulletList[i].SetActive(true);
                count = i;
                break;
            }
        }
        return bulletList[count];
    }
}
