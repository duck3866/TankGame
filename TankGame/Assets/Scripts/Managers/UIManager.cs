using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    private PlayerController _playerController;
    
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Image[] images;
    [SerializeField] private GameObject diePanel;
    
    public static UIManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        if (player == null)
        {
            player = GameManager.Instance.player;
            if (player != null)
            {
                _playerController = player.GetComponent<PlayerController>();
                textMeshProUGUI.text = "현재 턴 수 : " + _playerController.playerTurn;    
            }
        }
    }
    public void CheckPlayerTurn()
    {
        textMeshProUGUI.text = "현재 턴 수 : " + _playerController.playerTurn;
    }
    public void CheckImage()
    { 
        for (int i = 1; i <= images.Length; i++)
        {
            if (_playerController.playerHp < i)
            {
                images[i-1].gameObject.SetActive(false);
            }
            else
            {
                images[i-1].gameObject.SetActive(true);
            }
        }
    }

    public void DiePanel()
    {
        diePanel.SetActive(true);
    }
}
