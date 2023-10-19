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
    /// ������ ���� �� �ҷ����� ���� Ŭ����
    /// </summary>
    
    //System.IO : ���� �����

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
                instance = go.AddComponent<DataManager>() as DataManager;   //��ũ��Ʈ ÷���ϸ鼭 ������ Ȱ��

                DontDestroyOnLoad(go);  //scene �ε��, �ı����� ���� ������Ʈ ���� (ex> scene ��ȯ)
            }
            return instance; 
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [Tooltip("GameDataFile.json")]
    string GameDataFileName = "GameDataFile.json";  //���� ������ ���� �̸� ����

    public GameSavingData data = new GameSavingData();    //����� Ŭ���� ����

    /// <summary>
    /// LoadGame
    /// </summary>
    public void LoadGameData(GameKind gameKind)
    {
        string directoryPath = Application.persistentDataPath + "/" + gameKind.ToString();  //���Ӹ��� ���� ����
        string filePath = directoryPath + "/" + GameDataFileName;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);   //������

            data = JsonUtility.FromJson<GameSavingData>(fromJsonData);  //�ҷ���

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
        string toJsonData = JsonUtility.ToJson(data, true); //Ŭ���� -> Json �������� ��ȯ   //true : ������ ���� ����

        File.WriteAllText(filePath, toJsonData);    //������ ������ ���, ������ ���� ����� ����
    }

    public void OrderbyDesc(GameKind gameKind, string strData, bool isDesc)
    {
        //List<string> newList = new List<string>();  //������������ ��� ����

        //���� ����Ʈ + ���� ���� �߰�
        data.recordDataList.Add(strData);
        //�������� ����
        if (isDesc)
        {
            data.recordDataList = data.recordDataList.OrderByDescending(i => i).ToList();
        }
        else
        {
            data.recordDataList = data.recordDataList.OrderBy(i => i).ToList();
        }

        //�������� ���ְ� ����Ʈ 3���� ����
        if (data.recordDataList.Count > 3)
        {
            data.recordDataList.RemoveAt(data.recordDataList.Count - 1);
        }
    }
}
