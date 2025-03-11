using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct SuitIDdata
{
    public string suit;
    public int id;
    public GameObject Cardclone;

    public SuitIDdata(string suit, int id, GameObject cardclone)
    {
        this.suit = suit;
        this.id = id;
        this.Cardclone = cardclone;
    }
}


public class PokerManager : MonoBehaviour
{
    // 최대 5개의 카드를 넣어둘 리스트
    [SerializeField] public List<SuitIDdata> SuitIDdata = new List<SuitIDdata>(5);

    [SerializeField] TextManager TextManager;

    private static PokerManager instance;
    public static PokerManager Instance { get { return instance; } }

    // 저장해둘 숫자
    public List<int> saveNum;

    // 숫자가 몇번 등장하는지 저장할 딕셔너리 (숫자, 몇번 등장하는지)
    [SerializeField] private Dictionary<int, int> dictionary;

    private void Awake()
    {
        saveNum = new List<int>();

        dictionary = new Dictionary<int, int>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // 활성화 된 카드의 숫자를 넣을 큐

    private void Start()
    {


    }

    // 값을 가져오고 리스트에 저장 (순차적으로)
    public void SaveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        SuitIDdata.Add(newSuitIDdata);

        // LinQ메서드를 사용한 오름차순정렬 (value 값 (숫자 갯수) 기준으로)
        SuitIDdata = SuitIDdata.OrderBy(x => x.id).ToList();

        // Debug.Log(saveNum.Count); // 카운트가 0 미만이면 출력 x


        // for (int i = 0; i < SuitIDdata.Count; i++)
        // {
        //     Debug.Log(SuitIDdata[i].id);
        // }

    }

    public void RemoveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        // 리스트에서 같은 suit, id 값을 가진 객체 찾기
        SuitIDdata existingData = SuitIDdata.Find(x => x.suit == newSuitIDdata.suit && x.id == newSuitIDdata.id);
        SuitIDdata.Remove(existingData);

    }


    // 저장된 모든 카드를 순회 (최대 5개)
    public Dictionary<int, int> Hand()
    {
        dictionary.Clear();

        for (int i = 0; i < SuitIDdata.Count; i++)
        {
            if (dictionary.ContainsKey(SuitIDdata[i].id))
            {
                dictionary[SuitIDdata[i].id]++;
            }
            else
            {
                dictionary[SuitIDdata[i].id] = 1;
            }
        }
        return dictionary;
    }

    // 스트레이트인지 확인 (ex 1 2 3 4 5)
    public bool isStraight()
    {
        for (int i = 1; i < SuitIDdata.Count; i++)
        {
            // 현재카드와 바로 앞 카드의 숫자 차이가 1인지 확인
            // 하나라도 다르면 바로 false반환
            if (SuitIDdata[i].id != SuitIDdata[i - 1].id + 1)
            {
                return false;
            }
        }
        return true;
    }

    // 플러시인지 확인 (ex 다이아 5개)
    public bool isFlush()
    {
        // Suit타입을 저장할 변수

        string firstSuit = SuitIDdata[0].suit;
        for (int i = 0; i < SuitIDdata.Count; i++)
        {
            // [0]번째와 hand 인덱스가 하나라도 다르면 false반환
            if (SuitIDdata[i].suit != firstSuit)
            {
                return false;
            }
        }
        return true;
    }

    // 핸드의 종류 확인
    public void getHandType()
    {
        saveNum.Clear();
        GameManager.Instance.Plussum = 0;
        GameManager.Instance.Multiplysum = 0;


        // 반환된 숫자 카운트 저장
        Dictionary<int, int> rankCount = Hand();

        bool flush = false;
        bool straight = false;

        if (SuitIDdata.Count > 0)
        {
            flush = isFlush();
            straight = isStraight();
        }

        var lastElement = rankCount.LastOrDefault(); // 마지막 요소
        var firstElement = rankCount.FirstOrDefault(); // 처음 요소


        // 스트레이트 플러시 및 로얄 스트레이트 플러시 처리
        if (SuitIDdata.Count == 5)
        {
            if (straight && flush)
            {
                if (SuitIDdata[0].id == 10)
                {
                    // 로얄 스트레이트 플러시: 10, J, Q, K, A
                    saveNum.Add(lastElement.Key);  // 로얄 스트레이트 플러시
                    TextManager.PokerTextUpdate("로얄 스트레이트 플러시");
                    GameManager.Instance.PokerCalculate(150, 8);
                    Debug.Log("로얄 스트레이트 플러시");
                }
                else
                {
                    // 스트레이트 플러시
                    for (int i = 0; i < SuitIDdata.Count; i++)
                    {
                        saveNum.Add(SuitIDdata[i].id);
                    }
                    TextManager.PokerTextUpdate("스트레이트 플러시");
                    GameManager.Instance.PokerCalculate(100, 8);
                    Debug.Log("스트레이트 플러시");
                }
                return;
            }

            // 플러시
            if (flush)
            {
                for (int i = 0; i < SuitIDdata.Count; i++)
                {
                    saveNum.Add(SuitIDdata[i].id);
                }
                TextManager.PokerTextUpdate("플러시");
                GameManager.Instance.PokerCalculate(35, 4);

                Debug.Log("플러시");
                return;
            }

            // 스트레이트
            if (straight)
            {
                for (int i = 0; i < SuitIDdata.Count; i++)
                {
                    saveNum.Add(SuitIDdata[i].id);
                }
                TextManager.PokerTextUpdate("스트레이트");
                GameManager.Instance.PokerCalculate(30, 4);

                Debug.Log("스트레이트");
                return;
            }

            // 풀 하우스, 포카드 처리
            if (rankCount.Count() == 2)
            {
                if (lastElement.Value == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saveNum.Add(lastElement.Key);  // 포카드
                    }
                    TextManager.PokerTextUpdate("포 카드");
                    GameManager.Instance.PokerCalculate(60, 7);

                    Debug.Log("포카드");
                }
                else if (firstElement.Value == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saveNum.Add(firstElement.Key);  // 포카드
                    }
                    TextManager.PokerTextUpdate("포 카드");
                    GameManager.Instance.PokerCalculate(60, 7);

                    Debug.Log("포카드");
                }
                else
                {
                    saveNum.Add(firstElement.Key);  // 풀 하우스 (3장, 2장)
                    saveNum.Add(lastElement.Key);
                    TextManager.PokerTextUpdate("풀 하우스");
                    GameManager.Instance.PokerCalculate(40, 4);

                    Debug.Log("풀 하우스");
                }
                return;
            }
        }

        // 트리플 처리
        if (rankCount.Values.Contains(3))
        {
            // 3과 똑같은 벨류값을 가진애 찾기
            foreach (var item in rankCount.Where(x => x.Value == 3))
            {
                for (int i = 0; i < 3; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            TextManager.PokerTextUpdate("트리플");
            GameManager.Instance.PokerCalculate(30, 3);

            Debug.Log("트리플");
            return;
        }

        // 투페어 처리
        if (rankCount.Values.Count(v => v == 2) == 2)
        {
            foreach (var item in rankCount.Where(x => x.Value == 2))
            {
                for (int i = 0; i < 2; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            TextManager.PokerTextUpdate("투 페어");
            GameManager.Instance.PokerCalculate(20, 2);

            Debug.Log("투 페어");
            return;
        }

        // 원페어 처리
        if (rankCount.Values.Contains(2))
        {
            foreach (var item in rankCount.Where(x => x.Value == 2))
            {
                for (int i = 0; i < 2; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            TextManager.PokerTextUpdate("원 페어");
            GameManager.Instance.PokerCalculate(10, 2);

            Debug.Log("원 페어");
            return;
        }

        // 하이 카드 처리
        if (SuitIDdata.Count != 0)
        {
            saveNum.Add(lastElement.Key); // 가장 큰 값
            TextManager.PokerTextUpdate("하이 카드");
            GameManager.Instance.PokerCalculate(5, 1);

            Debug.Log("하이 카드: " + lastElement.Key);
            return;
        }

        // 리스트가 비어있다면 텍스트도 빈 값
        if(SuitIDdata.Count == 0)
        {
            TextManager.PokerTextUpdate("");
            TextManager.PokerUpdate(0, 0);
        }
    }


}
