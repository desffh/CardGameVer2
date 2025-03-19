using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // 계산 이벤트
    public UnityEvent calculation;

    [SerializeField] HandCardPoints deleteCardPoint;

    WaitForSeconds waitForSeconds;



    // 핸드 횟수 & 버리기 횟수
    
    [SerializeField] public int Hand;
    [SerializeField] public int Delete;

    // 숫자를 담고 하나씩 빼기 위한 큐
    private Queue<int> Num;

    [SerializeField] public int Plussum;
    [SerializeField] public int Multiplysum;
    [SerializeField] public int TotalScore;

    [SerializeField] TextManager textManager;
    [SerializeField] AnimationManager animationManager;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(1.0f);

        Plussum = 0;
        Multiplysum = 0;
        TotalScore = 0;

        Num = new Queue<int>();

        // 횟수 초기화
        Hand = 4;
        Delete = 4;
    }

    public void StageEnd()
    {
        Debug.Log("스테이지 종료");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 플레이 모드를 종료
#endif

    }

    public void DeCountHand()
    {
        --Hand;
        textManager.HandCountUpdate(Hand);
    }
    public void DeCountDelete()
    {
        --Delete;
        textManager.DeleteCountUpdate(Delete);
    }


    // 이벤트 : 1. 큐에 값 다 넣기 2. 계산 후 텍스트 누적 3. 카드 날라간 뒤 비활성화
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
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1).SetDelay(i * 0.2f);
            // 회전 0
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DORotate(new Vector3(-45,-60,-25),0.5f).SetDelay(i * 0.2f);

            StartCoroutine(delayActive());
        }
    }

    private void UIupdate()
    {
        textManager.PokerTextUpdate();
        textManager.PlusTextUpdate();
        textManager.MultipleTextUpdate();
        textManager.BufferUpdate();
    }

    public void DelaySetActive()
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            PokerManager.Instance.SuitIDdata[i].Cardclone.SetActive(false);
        }
    }


    // 계산이 끝나는 곳 & 다시 카드가 배치되는 곳
    IEnumerator delayActive()
    {
        yield return waitForSeconds;
        DelaySetActive();

        // 계산 다 하고 리스트 초기화
        PokerManager.Instance.SuitIDdata.Clear();
        PokerManager.Instance.saveNum.Clear();
        UIupdate();

        // 다시 콜라이더 활성화
        KardManager.Instance.card.StartCollider();
        ButtonManager.Instance.ButtonInactive();

        CheckReset();

        if(Hand <= 0)
        {
            StageEnd();
            
        }
        KardManager.Instance.AddCardSpawn();

        yield break;
    }

    IEnumerator PlusCalculation()
    {
        while (true)
        {
            if (Num.Count == 0)
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
        if (Num.Count > 0)
        {
            // 큐에서 빼면서 체크
            int saveNumber = Num.Dequeue();
            Plussum += saveNumber;

            // 애니메이션 호출
            animationManager.PlayCardAnime(SaveNumber(saveNumber));

            // 타이핑 모션 (카드 위에 생성) - 함수를 호출
            //textManager.IndexScore(saveNumber);
        }
        textManager.PlusTextUpdate(Plussum);
    }
    
    // 애니메이션을 호출하기 위해 사용
    private GameObject game;

    private bool[] savenumberCheck = new bool[5];
    public GameObject SaveNumber(int saveNumber)
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            if (savenumberCheck[i] == false && PokerManager.Instance.SuitIDdata[i].id == saveNumber)
            {
                game = PokerManager.Instance.SuitIDdata[i].Cardclone;

                savenumberCheck[i] = true;
                // 리스트에서 제거해버리면 deleteZone으로 이동할 수 없음
                //PokerManager.Instance.SuitIDdata.RemoveAt(i);
                break;
            }
        }
        return game;
    }

    
    /*
    // 카드 개별 점수 텍스트 애니메이션을 호출하기 위해 사용
    Transform transforms;

    private bool[] saveIndexCheck = new bool[5];
    public Transform SaveIndex(int saveNumber)
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            if (savenumberCheck[i] == false && PokerManager.Instance.SuitIDdata[i].id == saveNumber)
            {
                transforms = PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();

                saveIndexCheck[i] = true;

                break;
            }
        }
        return transforms;
    }
    */


    // 애니메이션 판단여부 bool배열 초기화
    private void CheckReset()
    {
        for(int i = 0; i < 5; i++)
        {
            savenumberCheck[i] = false;
        }
    }


    // 족보 룰 점수
    public void PokerCalculate(int plus, int multiple)
    {
        if (PokerManager.Instance.SuitIDdata.Count > 0)
        {
            Plussum += plus;
            

            Multiplysum += multiple;
        }
        textManager.PokerUpdate(Plussum, Multiplysum);
    }


    IEnumerator DelayedTotalScoreCal()
    {
        yield return waitForSeconds;  // 대기
        TotalScoreCal();  // TotalScoreCal 실행
        StartCoroutine(DelayedMove());
    }
    IEnumerator DelayedMove()
    {
        yield return waitForSeconds;
        DeleteMove();
    }

    public void TotalScoreCal()
    {
        TotalScore += Plussum * Multiplysum;
        textManager.TotalScoreUpdate(TotalScore);
    }

    IEnumerator deleteCard()
    {
        yield return waitForSeconds;
        DelaySetActive();

        // 리스트 초기화
        PokerManager.Instance.SuitIDdata.Clear();
        PokerManager.Instance.saveNum.Clear();

        KardManager.Instance.AddCardSpawn();
        UIupdate();

        // 다시 콜라이더 활성화
        KardManager.Instance.card.StartCollider();
        ButtonManager.Instance.ButtonInactive();
        yield break;
    }

    public void StartDeleteCard()
    {
        // 버리는 동안 카드의 콜라이더 비활성화
        KardManager.Instance.card.QuitCollider();

        // 카드 비활성화 & 콜라이더 활성화 
        StartCoroutine(deleteCard());
    }
}
