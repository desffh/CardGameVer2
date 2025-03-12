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

    [SerializeField] GameObject PopUpCanvas;

    // ��ư Ȱ��ȭ ���� ����
    private bool isButtonActive = true;


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

        PopUpCanvas.SetActive(false);
    }

    private void Update()
    {
       if(isButtonActive == true && GameManager.Instance.Hand > 0 && PokerManager.Instance.SuitIDdata.Count > 0) 
       {
            Handbutton.interactable = true;
       }
       else
       {
            Handbutton.interactable = false;
       }
       if(isButtonActive == true && GameManager.Instance.Delete > 0 && PokerManager.Instance.SuitIDdata.Count > 0)
        {
            Treshbutton.interactable = true;
        }
       else
        {
            Treshbutton.interactable = false;

        }
    }

    // �ڵ��ư�� Ŭ������ ��
    public void OnHandButtonClick()
    {
        GameManager.Instance.DeCountHand();

        

        for (int i  = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // ����� ī���� ��ũ��Ʈ ��������
            Card selectedCard =  PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Card>();

            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            
            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.5f);
           // ȸ�� 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.rotation = Quaternion.identity;

            // myCards ����Ʈ���� �ش� ī�� ���� (���ۿ��� �����ͼ� �����ϴ� ��)
            if (selectedCard != null && KardManager.Inst.myCards.Contains(selectedCard))
            {
                KardManager.Inst.myCards.Remove(selectedCard);
            }
        }
        // ���� ī��� ������ �Ǳ�
        KardManager.Inst.SetOriginOrder();
        KardManager.Inst.CardAlignment();

        // ���ϱ� ���
        GameManager.Instance.Calculation();
    }

    // ������ ��ư�� Ŭ������ ��
    public void OnDeleteButtonClick()
    {
        GameManager.Instance.DeCountDelete();

        isButtonActive = false;

        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // ����� ī���� ��ũ��Ʈ ��������
            Card selectedCard = PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Card>();

            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.DeleteCardpos.transform.position, 0.5f);
            // ȸ�� 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DORotate(new Vector3(58, 122, 71), 3);

            // myCards ����Ʈ���� �ش� ī�� ���� (���ۿ��� �����ͼ� �����ϴ� ��)
            if (selectedCard != null && KardManager.Inst.myCards.Contains(selectedCard))
            {
                KardManager.Inst.myCards.Remove(selectedCard);
            }
        }

        GameManager.Instance.StartDeleteCard();

        // ���� ī��� ������ �Ǳ�
        KardManager.Inst.SetOriginOrder();
        KardManager.Inst.CardAlignment();

    }

    // �̺�Ʈ�� �� �Լ� -> ����� ���۵Ǹ� ��ư ��ȣ�ۿ� ��Ȱ��ȭ
    public void ButtonActive()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
        
        isButtonActive = false;
    }

    public void ButtonInactive()
    {
        isButtonActive = true;
    }


    public void RunOnClick()
    {
        PopUpCanvas.SetActive(true);
    }

    public void RunDeleteClick()
    {
        PopUpCanvas.SetActive(false);
    }
}
