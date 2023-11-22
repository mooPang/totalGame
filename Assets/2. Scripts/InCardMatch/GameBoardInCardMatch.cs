using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardInCardMatch : MonoBehaviour
{
    private int rowCount;
    private int colCount;

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardSprites;

    private List<int> cardIDList = new List<int>(); //섞인 모든 카드 담고 있음, 총 18장

    void Start()
    {
        GenerateCardID();
        ShuffleCardID();
        InitGameBoard();
    }

    void GenerateCardID()
    {
        for (int i = 0; i < cardSprites.Length; i++)
        {
            cardIDList.Add(i);
            cardIDList.Add(i);      //0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 ..

            GameManagerInCardMatch.instance.cardIdList.Add(i);  //game over 위한 ID체크용
        }
    }

    void ShuffleCardID()
    {
        int cardCount = cardIDList.Count;

        for (int i = 0; i < cardCount; i++)
        {
            int randonIdx = Random.Range(i, cardCount);
            int temp = cardIDList[randonIdx];
            cardIDList[randonIdx] = cardIDList[i];
            cardIDList[i] = temp;
        }
    }

    void InitGameBoard()
    {
        float spaceY = 3.6f;
        float spaceX = 2.8f;
        rowCount = 3;
        colCount = 6;
        int cardIdx = 0;

        for (int row = 0; row < rowCount; row++) 
        {
            for (int col = 0; col < colCount; col++)
            {
                //Vector3 pos = Vector3.zero;
                float posX = (col - (int)(colCount / 2)) * spaceX + (spaceX / 2);
                float posY = (row - (int)(rowCount / 2)) * spaceY;
                Vector3 pos = new Vector3(posX, posY + 0.6f, 0f);    //초기 위치
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity);
                CardControllerInCardMatch card = cardObject.GetComponent<CardControllerInCardMatch>();
                int cardID = cardIDList[cardIdx++];
                card.SetCardID(cardID);
                card.SetCardSprite(cardSprites[cardID]);

            }
        }
    }
}
