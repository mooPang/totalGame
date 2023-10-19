using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GAMETYPE
{
    BOWLING,
    CARDMATCH,
    CLAYSHOOT,
    ONLYUP,
    _MAX_
}

public class UIManagerInMain : MonoBehaviour
{
    private int m_iCurSelectPage;

    [SerializeField]
    private GameObject m_goSelectScreen;

    [SerializeField]
    private GameObject m_goRecordScreen;

    [SerializeField]
    private Text m_txtGameName;

    [SerializeField]
    private Image m_imgGame;

    [SerializeField]
    private Text m_txtSoundValue;

    [SerializeField]
    private Slider m_slidSound;

    public struct GAMEINFO
    {
        public GAMETYPE    eGameType;
        public string      strGameName;
        public Sprite      sprGame;
        public string      strSceneName;
    }

    [SerializeField]
    private string[] GameNames;

    [SerializeField]
    private Sprite[] sprGames;

    [SerializeField]
    private string[] SceneNames;

    List<GAMEINFO> m_listGameInfo;

    private void Awake()
    {
        m_listGameInfo = new List<GAMEINFO>();
        m_iCurSelectPage = (int)GAMETYPE.BOWLING;
    }

    private void Start()
    {
        for (int idx = 0; idx < GameNames.Length; idx++)
        {
            GAMEINFO gameInfo;
            gameInfo.eGameType      = (GAMETYPE)idx;
            gameInfo.strGameName    = GameNames[idx];
            gameInfo.sprGame        = sprGames[idx];
            gameInfo.strSceneName   = SceneNames[idx];

            m_listGameInfo.Add(gameInfo);
        }
    }

    public void OnClickGameSelect()
    {
        m_goSelectScreen.SetActive(true);

        ShowGameType((GAMETYPE)m_iCurSelectPage);
    }

    public void OnClickCloseSelect()
    {
        m_goSelectScreen.SetActive(false);

        m_iCurSelectPage = (int)GAMETYPE.BOWLING;
    }

    public void OnClickSelectRightBtn()
    {
        m_iCurSelectPage++;

        if(m_iCurSelectPage >= (int)GAMETYPE._MAX_)
        {
            m_iCurSelectPage = (int)GAMETYPE.BOWLING;
        }

        ShowGameType((GAMETYPE)m_iCurSelectPage);
    }

    public void OnClickSelectLeftBtn()
    {
        m_iCurSelectPage--;

        if(m_iCurSelectPage < (int)GAMETYPE.BOWLING)
        {
            m_iCurSelectPage = (int)GAMETYPE.ONLYUP;
        }

        ShowGameType((GAMETYPE)m_iCurSelectPage);
    }

    private void ShowGameType(GAMETYPE eGameType)
    {
        m_txtGameName.text = m_listGameInfo[(int)eGameType].strGameName;
        m_imgGame.sprite = m_listGameInfo[(int)eGameType].sprGame;
    }

    public void OnClickGameStart()
    {
        m_goSelectScreen.SetActive(false);

        SceneManager.LoadScene(m_listGameInfo[m_iCurSelectPage].strSceneName);
    }

    public void OnClickRecordBtn()
    {
        m_goRecordScreen.SetActive(true);


    }

    public void OnClickCloseRecordBtn()
    {
        m_goRecordScreen.SetActive(false);
    }
    
    public void OnClickCloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnChangeSoundValue()
    {

    }
}
