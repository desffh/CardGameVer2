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

    // 버튼 활성화 상태 여부
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

    // 핸드버튼을 클릭했을 때
    public void OnHandButtonClick()
    {
        GameManager.Instance.DeCountHand();

        

        for (int i  = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // 저장된 카드의 스크립트 가져오기
            Card selectedCard =  PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Card>();

            // 저장된 프리팹의 위치값을 변경하기 위해 컴포넌트 가져오기
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            
            // 위치를 HandCardPoints로 이동
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.5f);
           // 회전 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.rotation = Quaternion.identity;

            // myCards 리스트에서 해당 카드 제거 (버퍼에서 가져와서 저장하는 곳)
            if (selectedCard != null && KardManager.Inst.myCards.Contains(selectedCard))
            {
                KardManager.Inst.myCards.Remove(selectedCard);
            }
        }
        // 남은 카드들 재정렬 되기
        KardManager.Inst.SetOriginOrder();
        KardManager.Inst.CardAlignment();

        // 더하기 계산
        GameManager.Instance.Calculation();
    }

    // 버리기 버튼을 클릭했을 때
    public void OnDeleteButtonClick()
    {
        GameManager.Instance.DeCountDelete();

        isButtonActive = false;

        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // 저장된 카드의 스크립트 가져오기
            Card selectedCard = PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Card>();

            // 저장된 프리팹의 위치값을 변경하기 위해 컴포넌트 가져오기
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            // 위치를 HandCardPoints로 이동
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(HandCardPoints.DeleteCardpos.transform.position, 0.5f);
            // 회전 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DORotate(new Vector3(58, 122, 71), 3);

            // myCards 리스트에서 해당 카드 제거 (버퍼에서 가져와서 저장하는 곳)
            if (selectedCard != null && KardManager.Inst.myCards.Contains(selectedCard))
            {
                KardManager.Inst.myCards.Remove(selectedCard);
            }
        }

        GameManager.Instance.StartDeleteCard();

        // 남은 카드들 재정렬 되기
        KardManager.Inst.SetOriginOrder();
        KardManager.Inst.CardAlignment();

    }

    // 이벤트에 들어갈 함수 -> 계산이 시작되면 버튼 상호작용 비활성화
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
