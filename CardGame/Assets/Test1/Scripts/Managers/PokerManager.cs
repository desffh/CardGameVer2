using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
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

    // �����ص� ����
    [SerializeField] public List<int> saveNum;

    // ���ڰ� ��� �����ϴ��� ������ ��ųʸ� (����, ��� �����ϴ���)
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

    // Ȱ��ȭ �� ī���� ���ڸ� ���� ť

    private void Start()
    {


    }

    // ���� �������� ����Ʈ�� ���� (����������)
    public void SaveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        SuitIDdata.Add(newSuitIDdata);   

        // LinQ�޼��带 ����� ������������ (value �� (���� ����) ��������)
        SuitIDdata = SuitIDdata.OrderBy(x => x.id).ToList();

        Debug.Log(saveNum.Count); // ī��Ʈ�� 0 �̸��̸� ��� x
        

        // for (int i = 0; i < SuitIDdata.Count; i++)
        // {
        //     Debug.Log(SuitIDdata[i].id);
        // }

    }
    
    public void RemoveSuitIDdata(SuitIDdata newSuitIDdata)
    {
        // ����Ʈ���� ���� suit, id ���� ���� ��ü ã��
        SuitIDdata existingData = SuitIDdata.Find(x => x.suit == newSuitIDdata.suit && x.id == newSuitIDdata.id);
        SuitIDdata.Remove(existingData);

    }


    // ����� ��� ī�带 ��ȸ (�ִ� 5��)
    public Dictionary<int,int> Hand()
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
    public bool isFlush()
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

    
    // �ڵ��� ���� Ȯ��
    public void getHandType()
    {
        saveNum.Clear();

        // ��ȯ�� ���� ī��Ʈ ����
        Dictionary <int,int> rankCount = Hand();

        bool flush = false;
        bool straight = false;

        if (SuitIDdata.Count > 0)
        {
            flush = isFlush();
            straight = isStraight();
        }

        var lastElement = rankCount.LastOrDefault(); // ������ ���
        var firstElement = rankCount.FirstOrDefault(); // ó�� ���


        // ��Ʈ����Ʈ �÷��� �� �ξ� ��Ʈ����Ʈ �÷��� ó��
        if (SuitIDdata.Count == 5)
        {
            if (straight && flush)
            {
                if (SuitIDdata[0].id == 10)
                {
                    // �ξ� ��Ʈ����Ʈ �÷���: 10, J, Q, K, A
                    saveNum.Add(lastElement.Key);  // �ξ� ��Ʈ����Ʈ �÷���
                    Debug.Log("�ξ� ��Ʈ����Ʈ �÷���");
                }
                else
                {
                    // ��Ʈ����Ʈ �÷���
                    for (int i = 0; i < SuitIDdata.Count; i++)
                    {
                        saveNum.Add(SuitIDdata[i].id);
                    }
                    Debug.Log("��Ʈ����Ʈ �÷���");
                }
                return;
            }

            // �÷���
            if (flush)
            {
                for (int i = 0; i < SuitIDdata.Count; i++)
                {
                    saveNum.Add(SuitIDdata[i].id);
                }
                Debug.Log("�÷���");
                return;
            }

            // ��Ʈ����Ʈ
            if (straight)
            {
                for (int i = 0; i < SuitIDdata.Count; i++)
                {
                    saveNum.Add(SuitIDdata[i].id);
                }
                Debug.Log("��Ʈ����Ʈ");
                return;
            }

            // Ǯ �Ͽ콺, ��ī�� ó��
            if (rankCount.Count() == 2)
            {
                if (lastElement.Value == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saveNum.Add(lastElement.Key);  // ��ī��
                    }
                    Debug.Log("��ī��");
                }
                else if (firstElement.Value == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saveNum.Add(firstElement.Key);  // ��ī��
                    }
                    Debug.Log("��ī��");
                }
                else
                {
                    saveNum.Add(firstElement.Key);  // Ǯ �Ͽ콺 (3��, 2��)
                    saveNum.Add(lastElement.Key);
                    Debug.Log("Ǯ �Ͽ콺");
                }
                return;
            }
        }

        // Ʈ���� ó��
        if (rankCount.Values.Contains(3))
        {
            // 3�� �Ȱ��� �������� ������ ã��
            foreach (var item in rankCount.Where(x => x.Value == 3))
            {
                for (int i = 0; i < 3; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            Debug.Log("Ʈ����");
            return;
        }

        // ����� ó��
        if (rankCount.Values.Count(v => v == 2) == 2)
        {
            foreach (var item in rankCount.Where(x => x.Value == 2))
            {
                for (int i = 0; i < 2; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            Debug.Log("�� ���");
            return;
        }

        // ����� ó��
        if (rankCount.Values.Contains(2))
        {
            foreach (var item in rankCount.Where(x => x.Value == 2))
            {
                for (int i = 0; i < 2; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
            Debug.Log("�� ���");
            return;
        }

        // ���� ī�� ó��
        if (SuitIDdata.Count != 0)
        {
            saveNum.Add(lastElement.Key); // ���� ū ��
            Debug.Log("���� ī��: " + lastElement.Key);
            return;
        }
    }
    
    
}
