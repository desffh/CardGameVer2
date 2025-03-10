using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // 계산 이벤트
    public UnityEvent calculation;

    [SerializeField] HandCardPoints deleteCardPoint;
    
    WaitForSeconds waitForSeconds;
    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // 숫자를 담고 하나씩 빼기 위한 큐
    private Queue<int> Num;

    [SerializeField] public int Plussum;
    [SerializeField] public int Multiplysum;
    [SerializeField] public int TotalScore;


    [SerializeField] TextManager textManager;

    

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(2.0f);

        Plussum = 0;
        Multiplysum = 0;
        TotalScore = 0;

        Num = new Queue<int>();
 
    }

    public void Calculation()
    {
        calculation.Invoke();
        Debug.Log("계산 시작");
    }


    public void CalSetting()
    {
        for (int i = 0; i < PokerManager.Instance.saveNum.Count; i++)
        {
            Num.Enqueue(PokerManager.Instance.saveNum[i]);
        }
        //Debug.Log("큐에 다 넣기 완료" + Num.Count);

        if (Num.Count > 0)
        {
            StartCoroutine(PlusCalculation());
        }

    }

    // 다 판별하고 버리기
    private void DeleteMove()
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // 저장된 프리팹의 위치값을 변경하기 위해 컴포넌트 가져오기
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
        
            // 위치를 HandCardPoints로 이동
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1);
            // 회전 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DORotate(new Vector3(58, 122,71), 3);

            StartCoroutine(delayActive());
        }
    }
    public void DelaySetActive()
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            PokerManager.Instance.SuitIDdata[i].Cardclone.SetActive(false);
        }
    }

    IEnumerator delayActive()
    {
        yield return waitForSeconds;
        DelaySetActive();
    }

    IEnumerator PlusCalculation()
    {
        while(true)
        {
            if(Num.Count == 0)
            {
                StartCoroutine(DelayedTotalScoreCal());
                Debug.Log("코루틴 종료 (계산 종료)");
                yield break;
            }

            yield return waitForSeconds;

            Calculate();
        }
        
    }
    public void Calculate()
    {
        // 큐에서 뺀 값을 누적해서 저장
        
        if(Num.Count > 0) 
        {
            int saveNumber = Num.Dequeue(); 
            Plussum += saveNumber;
        }

        textManager.PlusTextUpdate(Plussum);
    }

    // 족보 룰 점수
    public void PokerCalculate(int plus, int multiple)
    {
        // 족보 룰 값을 저장
        if (PokerManager.Instance.SuitIDdata.Count > 0)
        {
            Plussum += plus;
            Multiplysum += multiple;
        }
        textManager.PokerUpdate(Plussum, Multiplysum);
    }
    IEnumerator DelayedMove()
    {
        yield return waitForSeconds;
        DeleteMove();
    }

    IEnumerator DelayedTotalScoreCal()
    {
        yield return waitForSeconds; // 1초 대기 
        TotalScoreCal();  // 1초 뒤에 TotalScoreCal 실행
        StartCoroutine(DelayedMove());
    }

    public void TotalScoreCal()
    {
        TotalScore = Plussum * Multiplysum;
        textManager.TotalScoreUpdate(TotalScore);
    }

}
