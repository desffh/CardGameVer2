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
    // �ִ� 5���� ī�带 �־�� ����Ʈ
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

   

    // ���� �������� ����Ʈ�� ���� (����������)
    public void SaveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        SuitIDdata.Add(newSuitIDdata);
        Debug.Log(SuitIDdata.Count); // ����Ʈ�� ũ��
        

        // LinQ�޼��带 ����� ������������
        SuitIDdata = SuitIDdata.OrderBy(x => x.id).ToList();

        // for (int i = 0; i < SuitIDdata.Count; i++)
        // {
        //     Debug.Log(SuitIDdata[i].id);
        // }
        
    }
    
    public void RemoveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        Debug.Log("������ ����");

        // ����Ʈ���� ���� suit, id ���� ���� ��ü ã��
        SuitIDdata existingData = SuitIDdata.Find(x => x.suit == newSuitIDdata.suit && x.id == newSuitIDdata.id);


        SuitIDdata.Remove(existingData);
        Debug.Log("ī�尡 ����Ʈ���� ���ŵ�");

    }

    // ���ڰ� ��� �����ϴ��� ������ ��ųʸ� (����, ��� �����ϴ���)
    [SerializeField] private Dictionary<int, int> dictionary;

    // ����� ��� ī�带 ��ȸ (�ִ� 5��)
    public Dictionary<int,int> Hand()
    {
        for (int i = 0; i < SuitIDdata.Count; i++)
        {
            dictionary[SuitIDdata[i].id]++;
        }
        return dictionary;
    }

    // ��Ʈ����Ʈ���� Ȯ�� (ex 1 2 3 4 5)
    public bool isStraight()
    {
        for (int i = 1; i < SuitIDdata.Count; i++)
        {
            // ����ī��� �ٷ� �� ī���� ���� ���̰� 1���� Ȯ��
            // �ϳ��� �ٸ��� �ٷ� false��ȯ
            if (SuitIDdata[i].id != SuitIDdata[i - 1].id + 1)
            {
                return false;
            }
        }
        return true;
    }

    // �÷������� Ȯ�� (ex ���̾� 5��)
    bool isFlush()
    {
        // SuitŸ���� ������ ����
        string firstSuit = SuitIDdata[0].suit;
        for (int i = 0; i < SuitIDdata.Count; i++)
        {
            // [0]��°�� hand �ε����� �ϳ��� �ٸ��� false��ȯ
            if (SuitIDdata[i].suit != firstSuit)
            {
                return false;
            }
        }
        return true;
    }

    /*
    // �ڵ��� ���� Ȯ��
    string getHandType()
    {
        // ��ȯ�� ���� ī��Ʈ ����
        Dictionary <int,int> rankCount = Hand();
        bool flush = isFlush();
        bool straight = isStraight();

        if (straight && flush)
        {
            // ��Ʈ����Ʈ �÷���
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

        // ũ�Ⱑ 2��, ���� �ٸ� ���� 2�� ���̶�� �� (1, 4) (2, 3)
        if (rankCount.Count() == 2)
        {
            // Ǯ �Ͽ콺 �Ǵ� ��ī��
            if (lastElement.Value == 4)
            {
                // Ű ���� ���� �ؾ���
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
            // Ʈ���� �Ǵ� �����
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

        // ���ڸ��� 2ī��Ʈ�� �Ǿ��ִ� ���� (2,1,1,1)
        if (rankCount.Count() == 4)
        {
            return "One Pair";
        }

        else
        {
            Debug.Log("���� ���� ���� �� : " +
                SuitIDdata[4].id); // ���ĵ� ���¿��� ������ �ε���
            return "High Card";
        }
    }
    */
    
    
}
