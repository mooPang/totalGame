using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]  // 인스펙터 창에 클래스나 구조체 정보 노출

public class GameSavingData
{
    /// <summary>
    /// 저장해야 할 데이터 관리
    /// </summary>

    //게임 공용 데이터 리스트
    public List<string> recordDataList = new List<string>();

}
