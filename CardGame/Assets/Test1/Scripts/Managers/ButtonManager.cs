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

    // 핸드를 클릭했을 때
    public void OnHandButtonClick()
    {
        for(int i  = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // 저장된 프리팹의 위치값을 변경하기 위해 컴포넌트 가져오기
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
            // 위치를 HandCardPoints로 이동
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.position 
                = HandCardPoints.HandCardpos[i].transform.position;
        }
    }

    // 버리기를 클릭했을 때
    public void OnTreshButtonClick()
    { 
    
    }
}
