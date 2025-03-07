using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private string _playSceneName;
    [SerializeField] private GameObject _settingPanel;
    /// <summary>
    /// 시작 버튼 클릭 함수
    /// </summary>
    public void OnClickStartButton()
    {
        SceneManager.LoadScene(_playSceneName);
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
