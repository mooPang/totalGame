using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;

public enum GameKind
{
    BOWLING,
    CARDMATCH,
    CLAY,
    UP,
    SOUND

}

public class DataManager : MonoBehaviour
{
    /// <summary>
    /// 데이터 저장 및 불러오기 관리 클래스
    /// </summary>
    
    //System.IO : 파일 입출력

    static GameObject go;

    public static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                go = new GameObject();
                go.name = "DataManager";
                instance = go.AddComponent<DataManager>() as DataManager;   //스크립트 첨부하면서 변수로 활용

                DontDestroyOnLoad(go);  //scene 로드시, 파괴하지 않을 오브젝트 설정 (ex> scene 전환)
            }
            return instance; 
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [Tooltip("GameDataFile.json")]
    string GameDataFileName = "GameDataFile.json";  //게임 데이터 파일 이름 설정

    public GameSavingData data = new GameSavingData();    //저장용 클래스 변수

    /// <summary>
    /// LoadGame
    /// </summary>
    public void LoadGameData(GameKind gameKind)
    {
        string directoryPath = Application.persistentDataPath + "/" + gameKind.ToString();  //게임마다 폴더 생성
        string filePath = directoryPath + "/" + GameDataFileName;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);   //저장경로

            data = JsonUtility.FromJson<GameSavingData>(fromJsonData);  //불러옴

            if (data != null)
                Debug.LogError(data.recordDataList[0]);
        }
    }

    public void SaveGameData(GameKind gameKind, string strData, bool isDesc = false)
    {
        if (gameKind != GameKind.SOUND)
        {
            OrderbyDesc(gameKind, strData, isDesc);
        }

        string filePath = Application.persistentDataPath + "/" + gameKind.ToString() + "/" + GameDataFileName;
        string toJsonData = JsonUtility.ToJson(data, true); //클래스 -> Json 형식으로 전환   //true : 가독성 좋기 위함

        File.WriteAllText(filePath, toJsonData);    //기존에 있으면 덮어씀, 없으면 새로 만들어 저장
    }

    public void OrderbyDesc(GameKind gameKind, string strData, bool isDesc)
    {
        //List<string> newList = new List<string>();  //내림차순으로 담기 위함

        //기존 리스트 + 현재 점수 추가
        data.recordDataList.Add(strData);
        //내림차순 정렬
        if (isDesc)
        {
            data.recordDataList = data.recordDataList.OrderByDescending(i => i).ToList();
        }
        else
        {
            data.recordDataList = data.recordDataList.OrderBy(i => i).ToList();
        }

        //마지막꺼 없애고 리스트 3개로 만듦
        if (data.recordDataList.Count > 3)
        {
            data.recordDataList.RemoveAt(data.recordDataList.Count - 1);
        }
    }
}
