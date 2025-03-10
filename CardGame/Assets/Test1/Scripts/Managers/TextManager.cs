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

    // ����Ʈ�� �ƹ� ���� ������� ������ �� �� ""
    public void PokerTextUpdate(string pokertext = "")
    {
        
        PokerText.text = pokertext;
    }

    public void PlusTextUpdate(int sum)
    {
        PlusText.text = sum.ToString();
    }
}
