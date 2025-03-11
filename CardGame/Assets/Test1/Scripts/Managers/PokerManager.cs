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
    // �ִ� 5���� ī�带 �־�� ����Ʈ
    [SerializeField] public List<SuitIDdata> SuitIDdata = new List<SuitIDdata>(5);

    [SerializeField] TextManager TextManager;

    private static PokerManager instance;
    public static PokerManager Instance { get { return instance; } }

    // �����ص� ����
    public List<int> saveNum;

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

        // Debug.Log(saveNum.Count); // ī��Ʈ�� 0 �̸��̸� ��� x


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
        GameManager.Instance.Plussum = 0;
        GameManager.Instance.Multiplysum = 0;


        // ��ȯ�� ���� ī��Ʈ ����
        Dictionary<int, int> rankCount = Hand();

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
                    TextManager.PokerTextUpdate("�ξ� ��Ʈ����Ʈ �÷���");
                    GameManager.Instance.PokerCalculate(150, 8);
                    Debug.Log("�ξ� ��Ʈ����Ʈ �÷���");
                }
                else
                {
                    // ��Ʈ����Ʈ �÷���
                    for (int i = 0; i < SuitIDdata.Count; i++)
                    {
                        saveNum.Add(SuitIDdata[i].id);
                    }
                    TextManager.PokerTextUpdate("��Ʈ����Ʈ �÷���");
                    GameManager.Instance.PokerCalculate(100, 8);
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
                TextManager.PokerTextUpdate("�÷���");
                GameManager.Instance.PokerCalculate(35, 4);

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
                TextManager.PokerTextUpdate("��Ʈ����Ʈ");
                GameManager.Instance.PokerCalculate(30, 4);

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
                    TextManager.PokerTextUpdate("�� ī��");
                    GameManager.Instance.PokerCalculate(60, 7);

                    Debug.Log("��ī��");
                }
                else if (firstElement.Value == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        saveNum.Add(firstElement.Key);  // ��ī��
                    }
                    TextManager.PokerTextUpdate("�� ī��");
                    GameManager.Instance.PokerCalculate(60, 7);

                    Debug.Log("��ī��");
                }
                else
                {
                    saveNum.Add(firstElement.Key);  // Ǯ �Ͽ콺 (3��, 2��)
                    saveNum.Add(lastElement.Key);
                    TextManager.PokerTextUpdate("Ǯ �Ͽ콺");
                    GameManager.Instance.PokerCalculate(40, 4);

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
            TextManager.PokerTextUpdate("Ʈ����");
            GameManager.Instance.PokerCalculate(30, 3);

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
            TextManager.PokerTextUpdate("�� ���");
            GameManager.Instance.PokerCalculate(20, 2);

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
            TextManager.PokerTextUpdate("�� ���");
            GameManager.Instance.PokerCalculate(10, 2);

            Debug.Log("�� ���");
            return;
        }

        // ���� ī�� ó��
        if (SuitIDdata.Count != 0)
        {
            saveNum.Add(lastElement.Key); // ���� ū ��
            TextManager.PokerTextUpdate("���� ī��");
            GameManager.Instance.PokerCalculate(5, 1);

            Debug.Log("���� ī��: " + lastElement.Key);
            return;
        }

        // ����Ʈ�� ����ִٸ� �ؽ�Ʈ�� �� ��
        if(SuitIDdata.Count == 0)
        {
            TextManager.PokerTextUpdate("");
            TextManager.PokerUpdate(0, 0);
        }
    }


}
