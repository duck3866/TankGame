using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private string _playSceneName;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _stagePanel;
    [SerializeField] private Animator _settingPanelAnimator;
    [SerializeField] private GameObject StagePanel;
    [SerializeField] private GameObject StageContant;
    
    public void Start()
    {
        // _settingPanelAnimator.SetTrigger("Start");
        OnSettingStageButton();
    }

    public void OnSettingStageButton()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject button = Instantiate(StagePanel,StageContant.transform);
            button.SetActive(true);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(()=>StageManager.instance.OnStageButtonClicked(index));
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "stage_" + i.ToString();
            Debug.Log($"{i} 흠..");
        }

        StageManager.instance.index = 0;
    }
    /// <summary>
    /// 시작 버튼 클릭 함수
    /// </summary>
    public void OnClickStartButton()
    {
        // SceneManager.LoadScene(_playSceneName);
        _stagePanel.SetActive(true);
    }
    
    /// <summary>
    /// 스테이지 버튼 종료 함수
    /// </summary>
    public void OnClickCloseStageButton()
    {
        // SceneManager.LoadScene(_playSceneName);
        _stagePanel.SetActive(false);
    }
    /// <summary>
    /// 종료 버튼 클릭 함수
    /// </summary>
    public void OnClickExitButton()
    {
        Application.Quit();
    }
    /// <summary>
    /// 설정 창을 비활성화하는 함수
    /// </summary>
    public void OnCloseSettingButton()
    {
        _settingPanel.SetActive(false);
    }
    /// <summary>
    /// 설정 창을 활성화하는 함수
    /// </summary>
    public void OnOpenSettingButton()
    {
        _settingPanel.SetActive(true);
    }
}
