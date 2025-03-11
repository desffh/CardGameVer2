using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance { get; private set; }

    [SerializeField] Button Handbutton;
    [SerializeField] Button Treshbutton;

    [SerializeField] HandCardPoints HandCardPoints;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.7f);
           // ȸ�� 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.rotation = Quaternion.identity;
        
        }
        // ���ϱ� ���
        GameManager.Instance.Calculation();
    }

    // �����⸦ Ŭ������ ��
    public void OnDeleteButtonClick()
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.DeleteCardpos.transform.position, 1);
            // ȸ�� 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DORotate(new Vector3(58, 122, 71), 3);
        }
    }
}
