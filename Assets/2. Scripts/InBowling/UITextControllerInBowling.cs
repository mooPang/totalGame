using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextControllerInBowling : MonoBehaviour
{
    public TMP_Text CurrentRoundTxt;
    public TMP_Text FirstScoreTxt;
    public TMP_Text SecondScoreTxt;
    public TMP_Text ThirdScoreTxt;
    public TMP_Text TotalScoreTxt;
    public TMP_Text HiddenTxt;      //스패어, 스트라이크, 더블, 터키 표시

    private void Awake()
    {
        //CurrentRoundTxt.text = "";
        //FirstScoreTxt.text = "";
        //SecondScoreTxt.text = "";
        //TotalScoreTxt.text = "";
    }

    void Start()
    {
        
    }

    void Update()
    {
        UITextController();
    }

    void UITextController()
    {
        CurrentRoundTxt.text = "  [" + GameManagerInBowling.instance.currentRound.ToString() + "R/10R]";
        FirstScoreTxt.text = "  1st Score : " + GameManagerInBowling.instance.firstTrialScore.ToString();
        SecondScoreTxt.text = "  2nd Score : " + GameManagerInBowling.instance.secondTrialScore.ToString();
        //ThirdScoreTxt.text = "  3rd Score : " + GameManagerInBowling.instance..ToString();
        TotalScoreTxt.text = "  Total Score : " + GameManagerInBowling.instance.stackTotalScore.ToString();
    }
}
