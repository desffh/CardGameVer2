using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PokerText;

    [SerializeField] TextMeshProUGUI PlusText;
    [SerializeField] TextMeshProUGUI MultiplyText;

    [SerializeField] TextMeshProUGUI TotalScoreText;

    [SerializeField] AnimationManager animationmanager;

    private RectTransform plusTextPosition;
    private RectTransform MultiTextPosition;

    
    private void Start()
    {
       
    }

    // 리스트에 아무 값도 들어있지 않으면 빈 값 ""
    public void PokerTextUpdate(string pokertext = "")
    {
        PokerText.text = pokertext;
    }
    // 더하기
    public void PlusTextUpdate(int plussum = 0)
    {
        PlusText.text = plussum.ToString();
        animationmanager.CaltransformAnime(PlusText);
    }

    // 곱하기
    public void MultipleTextUpdate(int multisum = 0)
    {
        MultiplyText.text = multisum.ToString();

    }

    // 족보가 완성되면 호출
    public void PokerUpdate(int plus, int multiple)
    {
        PlusText.text = plus.ToString();
        MultiplyText.text = multiple.ToString();
    }

    // 전체 점수
    public void TotalScoreUpdate(int totalscore)
    {
        TotalScoreText.text = totalscore.ToString();
        animationmanager.CaltransformAnime(TotalScoreText);
    }
}
