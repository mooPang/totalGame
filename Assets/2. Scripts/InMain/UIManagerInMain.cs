using DG.Tweening.Plugins;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
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
    private int m_iCurSelectTypePage;

    private int m_iCurSelectRecordPage;

    [SerializeField]
    private GameObject m_goSelectScreen;

    [SerializeField]
    private GameObject m_goRecordScreen;

    [SerializeField]
    private GameObject m_goOptionScreen;

    [SerializeField]
    private Text m_txtSelectGameName;

    [SerializeField]
    private Image m_imgGame;

    [SerializeField]
    private Text m_txtSoundValue;

    [SerializeField]
    private Slider m_slidSound;

    [SerializeField]
    private Text m_txtRecordGameName;

    [SerializeField]
    private Text[] m_txtRecords;

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

    public struct GAMERECORDINFO
    {
        public GAMETYPE eGameType;
        public string strGameName;
        public float[] iRankingRecord;
    }

    List<GAMERECORDINFO> m_listGameRecordInfo;


    private void Awake()
    {
        m_listGameInfo = new List<GAMEINFO>();
        m_iCurSelectTypePage = (int)GAMETYPE.BOWLING;
        m_iCurSelectRecordPage = (int)GameKind.BOWLING;
        m_listGameRecordInfo = new List<GAMERECORDINFO>();
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

        for (int idx = 0; idx < 3; idx++)
        {
            GAMERECORDINFO gameRecordInfo;
            gameRecordInfo.eGameType = (GAMETYPE)idx;
            gameRecordInfo.strGameName = GameNames[idx];
            gameRecordInfo.iRankingRecord = new float[3];

            DataManager.Instance.LoadGameData((GameKind)idx);

            for (int jdx = 0; jdx < 3; jdx++)
            {
                if(DataManager.instance.data != null)
                {
                    if (DataManager.instance.data.recordDataList.Count - 1 >= jdx)
                    {
                        gameRecordInfo.iRankingRecord[jdx] = float.Parse(DataManager.instance.data.recordDataList[jdx]);
                    }
                    else
                    {
                        gameRecordInfo.iRankingRecord[jdx] = 0.0f;
                    }
                }
                else
                {
                    gameRecordInfo.iRankingRecord[jdx] = 0.0f;
                }
            }

            m_listGameRecordInfo.Add(gameRecordInfo);
        }
    }

    public void OnClickGameSelect()
    {
        m_goSelectScreen.SetActive(true);

        ShowGameType((GAMETYPE)m_iCurSelectTypePage);
    }

    public void OnClickCloseSelect()
    {
        m_goSelectScreen.SetActive(false);

        m_iCurSelectTypePage = (int)GAMETYPE.BOWLING;
    }

    public void OnClickSelectRightBtn()
    {
        m_iCurSelectTypePage++;

        if(m_iCurSelectTypePage >= (int)GAMETYPE._MAX_)
        {
            m_iCurSelectTypePage = (int)GAMETYPE.BOWLING;
        }

        ShowGameType((GAMETYPE)m_iCurSelectTypePage);
    }

    public void OnClickSelectLeftBtn()
    {
        m_iCurSelectTypePage--;

        if(m_iCurSelectTypePage < (int)GAMETYPE.BOWLING)
        {
            m_iCurSelectTypePage = (int)GAMETYPE.ONLYUP;
        }

        ShowGameType((GAMETYPE)m_iCurSelectTypePage);
    }

    private void ShowGameType(GAMETYPE eGameType)
    {
        m_txtSelectGameName.text = m_listGameInfo[(int)eGameType].strGameName;
        m_imgGame.sprite = m_listGameInfo[(int)eGameType].sprGame;
    }

    public void OnClickGameStart()
    {
        m_goSelectScreen.SetActive(false);

        SceneManager.LoadScene(m_listGameInfo[m_iCurSelectTypePage].strSceneName);
    }

    public void OnClickRecordBtn()
    {
        m_goRecordScreen.SetActive(true);

        ShowGameRecord((GameKind)m_iCurSelectRecordPage);
    }

    public void OnClickCloseRecordBtn()
    {
        m_goRecordScreen.SetActive(false);

        m_iCurSelectRecordPage = (int)GameKind.BOWLING;
    }

    public void OnClickRecordRBtn()
    {
        m_iCurSelectRecordPage++;

        if(m_iCurSelectRecordPage > (int)GameKind.CLAY)
        {
            m_iCurSelectRecordPage = (int)GameKind.BOWLING;
        }

        ShowGameRecord((GameKind)m_iCurSelectRecordPage);
    }

    public void OnClickRecordLBtn()
    {
        m_iCurSelectRecordPage--;

        if (m_iCurSelectRecordPage < 0)
        {
            m_iCurSelectRecordPage = (int)GameKind.CLAY;
        }

        ShowGameRecord((GameKind)m_iCurSelectRecordPage);
    }

    public void ShowGameRecord(GameKind eGameKind)
    {
        GAMERECORDINFO newRecordInfo = m_listGameRecordInfo[(int)eGameKind];

        m_txtRecordGameName.text = newRecordInfo.strGameName;

        for(int idx = 0; idx < m_txtRecords.Length; idx++)
        {
            if (newRecordInfo.iRankingRecord[idx] != 0.0f)
            {
                if (eGameKind == GameKind.CARDMATCH)
                    m_txtRecords[idx].text = newRecordInfo.iRankingRecord[idx].ToString("N2");
                else
                    m_txtRecords[idx].text = Mathf.FloorToInt(newRecordInfo.iRankingRecord[idx]).ToString();
            }
            else
            {
                m_txtRecords[idx].text = "기 록 없 음";
            }
        }
    }

    public void OnClickCloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickOptionScreen()
    {
        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if(DataManager.instance.data != null)
        {
            if (DataManager.instance.data.recordDataList.Count != 0)
            {
                int iSndValue = int.Parse(DataManager.instance.data.recordDataList[0]);

                m_slidSound.value = iSndValue;
                m_txtSoundValue.text = DataManager.instance.data.recordDataList[0];
            }
        }

        m_goOptionScreen.SetActive(true);
    }

    public void OnClickCloseOption()
    {
        m_goOptionScreen.SetActive(false);
    }

    public void OnChangeSoundValue()
    {
        int iSndValue = Mathf.FloorToInt(m_slidSound.value);
        m_txtSoundValue.text = iSndValue.ToString();

        DataManager.Instance.SaveGameData(GameKind.SOUND, m_txtSoundValue.text);
    }
}
