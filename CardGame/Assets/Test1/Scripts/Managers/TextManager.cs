using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PokerText;

    [SerializeField] TextMeshProUGUI PlusText;

    private void Start()
    {
        
    }

    // 리스트에 아무 값도 들어있지 않으면 빈 값 ""
    public void PokerTextUpdate(string pokertext = "")
    {
        
        PokerText.text = pokertext;
    }

    public void PlusTextUpdate(int sum)
    {
        PlusText.text = sum.ToString();
    }
}
