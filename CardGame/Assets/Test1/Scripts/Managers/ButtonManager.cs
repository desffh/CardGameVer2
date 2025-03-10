using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] Button Handbutton;
    [SerializeField] Button Treshbutton;

    [SerializeField] HandCardPoints HandCardPoints;
    private void Start()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
    }

    private void Update()
    {
        if (PokerManager.Instance.SuitIDdata.Count > 0)
        {
            Handbutton.interactable = true;
            Treshbutton.interactable = true;
        }
        else
        {
            Handbutton.interactable = false;
            Treshbutton.interactable = false;
        }
    }

    // �ڵ带 Ŭ������ ��
    public void OnHandButtonClick()
    {
        for(int i  = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.position 
                = HandCardPoints.HandCardpos[i].transform.position;
        }
    }

    // �����⸦ Ŭ������ ��
    public void OnTreshButtonClick()
    { 
    
    }
}
