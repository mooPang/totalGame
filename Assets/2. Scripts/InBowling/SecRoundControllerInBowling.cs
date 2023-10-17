using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SecRoundControllerInBowling : MonoBehaviour
{
    public TMP_Text[] txtRound;
    private GameObject eachTextRound;


    private void Awake()
    {
        txtRound = new TMP_Text[11];
    }
    void Start()
    {
        InitializeTextRound();
    }

    void Update()
    {
        
    }

    void InitializeTextRound()
    {
        for (int i = 0; i < txtRound.Length; i++)
        {
            eachTextRound = transform.GetChild(i).gameObject;
            txtRound[i] = eachTextRound.GetComponentInChildren<TMP_Text>(); //각각 세팅

            if (i > 0)
                txtRound[i].text = (i) + "R";
        }
    }
}
