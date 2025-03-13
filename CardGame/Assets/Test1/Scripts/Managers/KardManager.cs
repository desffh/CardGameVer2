using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KardManager : MonoBehaviour
{
    // �̱���
    public static KardManager Inst { get; private set; }

    private void Awake()
    {
        if( Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // private���� �����Ϳ��� ���̰�
    [SerializeField] ItemDataReader ItemDataReader;

    // ItemData Ÿ���� ���� List ���� (����Ʈ�� �����迭 - ���� ũ�� �Ҵ� �ȵ� ����)
    [SerializeField] public List<ItemData> itemBuffer;

    [SerializeField] public List<Card> myCards; // Card Ÿ���� ���� ����Ʈ (�� ī��)

    [SerializeField] Transform cardSpawnPoint; // ī�� ������ġ �������� 

    [SerializeField] public Card card;

    // ī�� ���� ����, �� ��ġ
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    // �θ� ���� ������Ʈ �Ҵ�
    [SerializeField] private GameObject ParentCardPrefab;

    // ����Ʈ�� �������� �־��� �Լ�
    void SetupItemBuffer()
    {
        // ũ�� �����Ҵ�
        itemBuffer = new List<ItemData>(52); // �̸� �뷮�� ��Ƶα�

        for (int i = 0; i < 52; i++)
        {
            ItemData item = ItemDataReader.DataList[i];

            itemBuffer.Add(item);

        }
        // ������ ���� ���� ī�� ����
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            ItemData temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }


    // ����Ʈ���� ī�� �̱�
    public ItemData PopItem()
    {
        // �� �̾����� �ٽ� ���� ä��� 
        if (itemBuffer.Count == 0)
        {
            SetupItemBuffer();
        }

        ItemData item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // ����Ʈ �޼��� (0��° ��� ����)
        return item;
    }

    // �����Ϳ��� ī�� ������ ����
    [SerializeField] GameObject cardPrefabs;

    void AddCard()
    {
        if(myCards.Count < 8)
        {
            var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI); // ���� ������Ʈ Ÿ��

            // �θ��� �Ʒ��� ���� (���̶�Űâ ��������)
            cardObject.transform.SetParent(ParentCardPrefab.transform);
        
            var card = cardObject.GetComponent<Card>(); // ������ ī���� ��ũ��Ʈ �������� (Card)
            card.Setup(PopItem());

            myCards.Add(card);
        }
        SetOriginOrder();
        CardAlignment();
    }


    // ����Ʈ ��ü �����ϱ� (���� �߰��� ī�尡 ���� ���ʿ� ����)
    public void SetOriginOrder()
    {
        int count = myCards.Count;

        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];

            // ? -> targerCard�� null�� �ƴϸ� ������Ʈ ��������
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }
    
    // ī�� ����
    public void CardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.7f);

        var targetCards = myCards;

        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount); // objCount ��ŭ �뷮 �̸� �Ҵ�

        switch (objCount)
        {
            // ������ : 1,2,3�� �϶� (ȸ���� ���� ����)
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;

            // ī�尡 4�� �̻��϶� ���� ȸ������ ��
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        // ���� ������
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;

            // ī�尡 4�� �̻��̶�� ȸ���� ��
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void AddCardSpawn()
    {
        for (int i = myCards.Count; i < 8; i++) // 8������� ī�带 ����
        {
            AddCard(); // ī�� ���� �Լ� ȣ��
        }
    }

    private void Start()
    {
        SetupItemBuffer();

        AddCardSpawn();
    }

    public void ColliderQuit()
    {   
        card.QuitCollider();
        PokerManager.Instance.QuitCollider2();
    }
}
