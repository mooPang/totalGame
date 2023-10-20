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
                instance = go.AddComponent<UIManager>() as UIManager;   //��ũ��Ʈ ÷���ϸ鼭 ������ Ȱ��

                DontDestroyOnLoad(go);  //scene �ε��, �ı����� ���� ������Ʈ ���� (ex> scene ��ȯ)
            }
            return instance;
        }
    }

    private string m_strCurSceneName;

    private void Awake()
    {
        instance = this;
    }

    public void OnClickPauseBtn()
    {
        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;

        goPauseMenu.SetActive(true);

        m_strCurSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 0.0f;
    }

    public void OnClickContinueBtn()
    {
        GameObject goPauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        goPauseMenu.SetActive(false);

        Time.timeScale = 1.0f;
    }

    public void OnClickRestartBtn()
    {
        Time.timeScale = 1.0f;
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
        SceneManager.LoadScene("Main");
    }
}
