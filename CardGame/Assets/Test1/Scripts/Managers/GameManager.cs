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


    // 숫자를 담고 하나씩 빼기 위한 큐
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
        //Debug.Log("큐에 다 넣기 완료" + Num.Count);

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

    public void PokerCalculate()
    {

    }
}
