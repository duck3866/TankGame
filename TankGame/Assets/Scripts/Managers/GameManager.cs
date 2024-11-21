using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    public static GameManager Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }
        }
    }
    public void TurnChange(string name)
    {
        switch (name)
        {
            case "Player":
                playerController.isPlayerTurn = true;
                break;
            case "Enemy":
                break;
        }
    }
}
