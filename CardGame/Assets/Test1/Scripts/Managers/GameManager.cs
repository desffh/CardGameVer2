using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // ��� �̺�Ʈ
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


    // ���ڸ� ��� �ϳ��� ���� ���� ť
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
        Debug.Log("��� ����");
    }


    public void CalSetting()
    {
        for (int i = 0; i < PokerManager.Instance.saveNum.Count; i++)
        {
            Num.Enqueue(PokerManager.Instance.saveNum[i]);
        }
        //Debug.Log("ť�� �� �ֱ� �Ϸ�" + Num.Count);

        if (Num.Count > 0)
        {
            StartCoroutine(PlusCalculation());
        }

    }

    // �� �Ǻ��ϰ� ������
    private void DeleteMove()
    {
        for (int i = 0; i < PokerManager.Instance.SuitIDdata.Count; i++)
        {
            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.SuitIDdata[i].Cardclone.GetComponent<Transform>();
        
            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.SuitIDdata[i].Cardclone.transform.
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1);
            // ȸ�� 0
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
                Debug.Log("�ڷ�ƾ ���� (��� ����)");
                yield break;
            }

            yield return waitForSeconds;

            Calculate();
        }
        
    }
    public void Calculate()
    {
        // ť���� �� ���� �����ؼ� ����
        
        if(Num.Count > 0) 
        {
            int saveNumber = Num.Dequeue(); 
            Plussum += saveNumber;
        }

        textManager.PlusTextUpdate(Plussum);
    }

    // ���� �� ����
    public void PokerCalculate(int plus, int multiple)
    {
        // ���� �� ���� ����
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
        yield return waitForSeconds; // 1�� ��� 
        TotalScoreCal();  // 1�� �ڿ� TotalScoreCal ����
        StartCoroutine(DelayedMove());
    }

    public void TotalScoreCal()
    {
        TotalScore = Plussum * Multiplysum;
        textManager.TotalScoreUpdate(TotalScore);
    }

}
