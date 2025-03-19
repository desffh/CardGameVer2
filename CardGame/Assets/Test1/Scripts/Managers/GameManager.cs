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
    // ��� �̺�Ʈ
    public UnityEvent calculation;

    [SerializeField] HandCardPoints deleteCardPoint;

    WaitForSeconds waitForSeconds;



    // �ڵ� Ƚ�� & ������ Ƚ��
    
    [SerializeField] public int Hand;
    [SerializeField] public int Delete;

    // ���ڸ� ��� �ϳ��� ���� ���� ť
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

        // Ƚ�� �ʱ�ȭ
        Hand = 4;
        Delete = 4;
    }

    public void StageEnd()
    {
        Debug.Log("�������� ����");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �����Ϳ��� �÷��� ��带 ����
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


    // �̺�Ʈ : 1. ť�� �� �� �ֱ� 2. ��� �� �ؽ�Ʈ ���� 3. ī�� ���� �� ��Ȱ��ȭ
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
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1).SetDelay(i * 0.2f);
            // ȸ�� 0
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


    // ����� ������ �� & �ٽ� ī�尡 ��ġ�Ǵ� ��
    IEnumerator delayActive()
    {
        yield return waitForSeconds;
        DelaySetActive();

        // ��� �� �ϰ� ����Ʈ �ʱ�ȭ
        PokerManager.Instance.SuitIDdata.Clear();
        PokerManager.Instance.saveNum.Clear();
        UIupdate();

        // �ٽ� �ݶ��̴� Ȱ��ȭ
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
                Debug.Log("�ڷ�ƾ ���� (��� ����)");
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
            // ť���� ���鼭 üũ
            int saveNumber = Num.Dequeue();
            Plussum += saveNumber;

            // �ִϸ��̼� ȣ��
            animationManager.PlayCardAnime(SaveNumber(saveNumber));

            // Ÿ���� ��� (ī�� ���� ����) - �Լ��� ȣ��
            //textManager.IndexScore(saveNumber);
        }
        textManager.PlusTextUpdate(Plussum);
    }
    
    // �ִϸ��̼��� ȣ���ϱ� ���� ���
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
                // ����Ʈ���� �����ع����� deleteZone���� �̵��� �� ����
                //PokerManager.Instance.SuitIDdata.RemoveAt(i);
                break;
            }
        }
        return game;
    }

    
    /*
    // ī�� ���� ���� �ؽ�Ʈ �ִϸ��̼��� ȣ���ϱ� ���� ���
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


    // �ִϸ��̼� �Ǵܿ��� bool�迭 �ʱ�ȭ
    private void CheckReset()
    {
        for(int i = 0; i < 5; i++)
        {
            savenumberCheck[i] = false;
        }
    }


    // ���� �� ����
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
        yield return waitForSeconds;  // ���
        TotalScoreCal();  // TotalScoreCal ����
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

        // ����Ʈ �ʱ�ȭ
        PokerManager.Instance.SuitIDdata.Clear();
        PokerManager.Instance.saveNum.Clear();

        KardManager.Instance.AddCardSpawn();
        UIupdate();

        // �ٽ� �ݶ��̴� Ȱ��ȭ
        KardManager.Instance.card.StartCollider();
        ButtonManager.Instance.ButtonInactive();
        yield break;
    }

    public void StartDeleteCard()
    {
        // ������ ���� ī���� �ݶ��̴� ��Ȱ��ȭ
        KardManager.Instance.card.QuitCollider();

        // ī�� ��Ȱ��ȭ & �ݶ��̴� Ȱ��ȭ 
        StartCoroutine(deleteCard());
    }
}
