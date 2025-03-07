using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public struct SuitIDdata
{
    public string suit;
    public int id;

    public SuitIDdata(string suit, int id)
    {
        this.suit = suit;
        this.id = id;
    }
}

public class PokerManager : MonoBehaviour
{
    // 최대 5개의 카드를 넣어둘 리스트
    [SerializeField] public List<SuitIDdata> SuitIDdata = new List<SuitIDdata>(5);

    private static PokerManager instance;
    public static PokerManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    // 값을 가져오고 리스트에 저장 (순차적으로)
    public void SaveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        SuitIDdata.Add(newSuitIDdata);
        Debug.Log(SuitIDdata.Count); // 리스트의 크기
        

        // LinQ메서드를 사용한 오름차순정렬
        SuitIDdata = SuitIDdata.OrderBy(x => x.id).ToList();

        // for (int i = 0; i < SuitIDdata.Count; i++)
        // {
        //     Debug.Log(SuitIDdata[i].id);
        // }
        
    }
    
    public void RemoveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        Debug.Log("데이터 빼기");

        // 리스트에서 같은 suit, id 값을 가진 객체 찾기
        SuitIDdata existingData = SuitIDdata.Find(x => x.suit == newSuitIDdata.suit && x.id == newSuitIDdata.id);


        SuitIDdata.Remove(existingData);
        Debug.Log("카드가 리스트에서 제거됨");

    }

    // 숫자가 몇번 등장하는지 저장할 딕셔너리 (숫자, 몇번 등장하는지)
    [SerializeField] private Dictionary<int, int> dictionary;

    // 저장된 모든 카드를 순회 (최대 5개)
    public Dictionary<int,int> Hand()
    {
        for (int i = 0; i < SuitIDdata.Count; i++)
        {
            dictionary[SuitIDdata[i].id]++;
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
    bool isFlush()
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

    /*
    // 핸드의 종류 확인
    string getHandType()
    {
        // 반환된 숫자 카운트 저장
        Dictionary <int,int> rankCount = Hand();
        bool flush = isFlush();
        bool straight = isStraight();

        if (straight && flush)
        {
            // 스트레이트 플러시
            if (SuitIDdata[0].id == 10)
            {
                return "Royal Flush";
            }
            else
            {
                return "Straight Flush";
            }
        }

        var lastElement = rankCount.LastOrDefault();

        // 크기가 2면, 서로 다른 숫자 2개 뿐이라는 것 (1, 4) (2, 3)
        if (rankCount.Count() == 2)
        {
            // 풀 하우스 또는 포카드
            if (lastElement.Value == 4)
            {
                // 키 값을 저장 해야함
                rankCount;
                return "Four of a Kind";

            }
            else if (rankCount.end()->second == 4)
            {
                rankCount.end()->first;
                return "Four of a Kind";
            }
            else
            {
                return "Full House";
            }
        }
        if (flush)
        {
            return "Flush";
        }

        if (straight)
        {
            return "Straight";
        }

        if (rankCount.Count() == 3) // (3, 1, 1) (2, 2, 1)
        {
            // 트리플 또는 투페어
            if (rankCount.begin()->second == 3
                || (--rankCount.end())->second == 3)
            {
                return "Three of a Kind";
            }
            else
            {
                return "Two Pair";
            }
        }

        // 한자리에 2카운트가 되어있는 상태 (2,1,1,1)
        if (rankCount.Count() == 4)
        {
            return "One Pair";
        }

        else
        {
            Debug.Log("가장 높은 수의 값 : " +
                SuitIDdata[4].id); // 정렬된 상태에서 마지막 인덱스
            return "High Card";
        }
    }
    */
    
    
}
