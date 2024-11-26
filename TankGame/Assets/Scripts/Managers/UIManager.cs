using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    public Slider powerGageSlider;
    private PlayerController _playerController;
    [SerializeField] private TextMeshProUGUI clearTurnText;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Image[] images;
    [SerializeField] private GameObject diePanel;
    public GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI nowTurn;
    public static UIManager Instance = null;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject leftButton;
    public AudioClip[] AudioClips;
    [HideInInspector] public AudioSource AudioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
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

    public void TurnTextChange()
    {
        nowTurn.text = GameManager.Instance.nowTurn;
    }
    public void CheckImage()
    { 
        for (int i = 1; i <= images.Length; i++)
        {
            if (_playerController.playerHp < i)
            {
                images[i-1].color = Color.white;
            }
            else
            {
                images[i-1].color = Color.red;
            }
        }
    }

    public void Initialized()
    {
        if (_playerController != null)
        {
            TurnTextChange();
            CheckPlayerTurn();
            CheckImage();   
        }
    }

    public void ReRoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameClearPanel()
    {
        AudioSource.clip = AudioClips[1];
        AudioSource.Play();
        clearTurnText.text = "턴 수 :"+_playerController.playerTurn;
        player.SetActive(false);
        clearPanel.SetActive(true);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        if (MapManager.Instace.mapIndex == 0)
        {
            leftButton.SetActive(false);
        }
        else if (MapManager.Instace.mapIndex == 3)
        {
            rightButton.SetActive(false);
        }
    }
    public void DiePanel()
    {
        AudioSource.clip = AudioClips[0];
        AudioSource.Play();
        diePanel.SetActive(true);
    }
}
