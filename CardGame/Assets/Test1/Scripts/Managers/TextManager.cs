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


    private void Start()
    {
        
    }

    // ����Ʈ�� �ƹ� ���� ������� ������ �� �� ""
    public void PokerTextUpdate(string pokertext = "")
    {
        PokerText.text = pokertext;
    }
    // ���ϱ�
    public void PlusTextUpdate(int plussum)
    {
        PlusText.text = plussum.ToString();
    }

    // ���ϱ�
    public void MultipleTextUpdate(int multisum)
    {
        MultiplyText.text = multisum.ToString();
    }

    // ������ �ϼ��Ǹ� ȣ��
    public void PokerUpdate(int plus, int multiple)
    {
        PlusText.text = plus.ToString();
        MultiplyText.text = multiple.ToString(); 
    }

    // ��ü ����
    public void TotalScoreUpdate(int totalscore)
    {
        TotalScoreText.text = totalscore.ToString();
    }
}
