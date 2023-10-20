using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static GameObject go;

    public static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                go = new GameObject();
                go.name = "UIManager";
                instance = go.AddComponent<UIManager>() as UIManager;   //스크립트 첨부하면서 변수로 활용

                DontDestroyOnLoad(go);  //scene 로드시, 파괴하지 않을 오브젝트 설정 (ex> scene 전환)
            }
            return instance;
        }
    }

    private string m_strCurSceneName;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Time.timeScale == 0 && !IsActivePause())
        {
            OnActiveGameOverMenu();
        }
    }

    public void OnClickPauseBtn()
    {
        if (IsActivePause()) return;

        GameObject goPauseBG = GameObject.Find("Canvas").transform.Find("PauseBG").gameObject;
        goPauseBG.SetActive(true);

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        goPauseMenu.SetActive(true);

        m_strCurSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 0.0f;
    }

    public bool IsActivePause()
    {
        bool isActive = false;

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;

        isActive = goPauseMenu.activeSelf;

        return isActive;
    }

    public void OnClickContinueBtn()
    {
        GameObject goPauseBG = GameObject.Find("Canvas").transform.Find("PauseBG").gameObject;
        goPauseBG.SetActive(false);

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        goPauseMenu.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void OnClickRestartBtn()
    {
        Time.timeScale = 1.0f;

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        GameObject goContinue = goPauseMenu.transform.Find("ContinueBtn").gameObject;
        goContinue.SetActive(true);

        GameObject goPauseTxt = goPauseMenu.transform.Find("PauseTxt").gameObject;
        goPauseTxt.GetComponent<Text>().text = "일시 정지";

        SceneManager.LoadSceneAsync(m_strCurSceneName);
    }

    public void OnClickOptionBtn()
    {
        GameObject goOptionScreen = GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject;

        if(goOptionScreen != null)
            goOptionScreen.SetActive(true);

        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if (DataManager.instance.data != null)
        {
            if (DataManager.instance.data.recordDataList.Count != 0)
            {
                int iSndValue = int.Parse(DataManager.instance.data.recordDataList[0]);

                GameObject goSoundSlider = goOptionScreen.transform.Find("SoundSlider").gameObject;
                GameObject goSoundValue = goOptionScreen.transform.Find("SoundValue").gameObject;

                if (goSoundSlider != null)
                    goSoundSlider.GetComponent<Slider>().value = iSndValue;

                if (goSoundValue != null)
                    goSoundValue.GetComponent<Text>().text = DataManager.instance.data.recordDataList[0];
            }
        }
    }

    public void OnCloseOptionScreenBtn()
    {
        GameObject goOptionScreen = GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject;

        if (goOptionScreen != null)
            goOptionScreen.SetActive(false);
    }

    public void OnChangeSoundValue()
    {
        GameObject goOptionScreen = GameObject.Find("Canvas").transform.Find("OptionScreen").gameObject;
        GameObject goSoundSlider = goOptionScreen.transform.Find("SoundSlider").gameObject;
        GameObject goSoundValue = goOptionScreen.transform.Find("SoundValue").gameObject;

        int iSndValue = Mathf.FloorToInt(goSoundSlider.GetComponent<Slider>().value);
        goSoundValue.GetComponent<Text>().text = iSndValue.ToString();

        DataManager.Instance.SaveGameData(GameKind.SOUND, goSoundValue.GetComponent<Text>().text);
    }

    public void OnClickMenuBtn()
    {
        Time.timeScale = 1.0f;

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        GameObject goContinue = goPauseMenu.transform.Find("ContinueBtn").gameObject;
        goContinue.SetActive(true);

        GameObject goPauseTxt = goPauseMenu.transform.Find("PauseTxt").gameObject;
        goPauseTxt.GetComponent<Text>().text = "일시 정지";

        SceneManager.LoadScene("Main");
    }

    public void OnActiveGameOverMenu()
    {
        GameObject goPauseBG = GameObject.Find("Canvas").transform.Find("PauseBG").gameObject;
        goPauseBG.SetActive(true);

        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        goPauseMenu.SetActive(true);

        GameObject goPauseTxt = goPauseMenu.transform.Find("PauseTxt").gameObject;
        goPauseTxt.GetComponent<Text>().text = "게임 종료";

        GameObject goContinue = goPauseMenu.transform.Find("ContinueBtn").gameObject;
        goContinue.SetActive(false);

        m_strCurSceneName = SceneManager.GetActiveScene().name;
    }
}
