using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [SerializeField] int Plussum;

    [SerializeField] TextManager textManager;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(2.0f);

        Plussum = 0;

        Num = new Queue<int>();
 
    }

    public void CalSetting()
    {
        for (int i = 0; i < PokerManager.Instance.saveNum.Count; i++)
        {
            Num.Enqueue(PokerManager.Instance.saveNum[i]);
        }
        //Debug.Log("ť�� �� �ֱ� �Ϸ�" + Num.Count);

        if(Num.Count > 0) 
        {
            StartCoroutine(PlusCalculation());
        }
    }

    IEnumerator PlusCalculation()
    {
        while(true)
        {
            if(Num.Count == 0)
            {
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

    public void PokerCalculate()
    {

    }
}
