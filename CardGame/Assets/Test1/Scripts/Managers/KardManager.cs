using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KardManager : Singleton<KardManager>
{
    // 참조
    [SerializeField] ItemDataReader ItemDataReader;
    [SerializeField] public Card card;

    // ItemData 타입을 담을 List 선언
    [SerializeField] public List<ItemData> itemBuffer;

    // Card 타입을 담을 리스트 (내 카드)
    [SerializeField] public List<Card> myCards;

    // 카드 생성위치
    [SerializeField] Transform cardSpawnPoint; 

    // 카드 정렬 시작, 끝 위치
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    // 에디터에서 카드 프리팹 연결 (Instantiate)
    [SerializeField] GameObject cardPrefabs;
    
    // 부모 게임 오브젝트 할당 (배치될 위치)
    [SerializeField] GameObject ParentCardPrefab;

    // 버퍼에 카드 넣기
    void SetupItemBuffer()
    {
        // 크기 동적할당
        itemBuffer = new List<ItemData>(52);

        for (int i = 0; i < 52; i++)
        {
            // 각각의 스프레드 시트 정보를 담은 카드들을 itemBuffer에 저장
            ItemData item = ItemDataReader.DataList[i];

            itemBuffer.Add(item);
        }
        // 아이템 버퍼 안의 카드 섞기
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            ItemData temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    // 버퍼에서 카드 뽑기
    public ItemData PopItem()
    {
        // 다 뽑았으면 다시 버퍼 채우기 
        if (itemBuffer.Count == 0)
        {
            SetupItemBuffer();
        }

        ItemData item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // 리스트 메서드 (0번째 요소 제거)
        return item;
    }

    // 8장의 배치될 카드 추가 
    void AddCard()
    {
        if(myCards.Count < 8)
        {
            var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI); // 게임 오브젝트 타입

            // 부모의 아래에 생성 (하이라키창 계층구조)
            cardObject.transform.SetParent(ParentCardPrefab.transform);
        
            // 동적 생성된카드 오브젝트
            var card = cardObject.GetComponent<Card>(); // 생성된 카드의 스크립트 가져오기 (Card)
            card.Setup(PopItem()); // 뽑은 카드에 ItemData 정보 저장 & 스프라이트 셋팅

            // 동적 생성된 카드 오브젝트는 Card 스크립트를 가지고 있어서 Card타입 리스트에 담을 수 있다
            myCards.Add(card);
        }
        // 카드 정렬
        SetOriginOrder();
        CardAlignment();
    }

    // 리스트 전체 정렬 (먼저 추가한 카드가 제일 뒷쪽에 보임)
    public void SetOriginOrder()
    {
        int count = myCards.Count;

        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];

            // ? -> targerCard가 null이 아니면 컴포넌트 가져오기
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }
    
    // 카드 정렬
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
        List<PRS> results = new List<PRS>(objCount); // objCount 만큼 용량 미리 할당

        switch (objCount)
        {
            // 고정값 : 1,2,3개 일때 (회전이 없기 때문)
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;

            // 카드가 4개 이상일때 부터 회전값이 들어감
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        // 원의 방정식
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;

            // 카드가 4개 이상이라면 회전값 들어감
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

    // 나중에 수정
    public void AddCardSpawn()
    {
        for (int i = myCards.Count; i < 8; i++) // 8장까지의 카드를 생성
        {
            AddCard(); // 카드 생성 함수 호출
        }
    }

    private void Start()
    {
        SetupItemBuffer();

        // 현재는 myCards 리스트에 저장되어있다 (8장의 카드 Card 타입)
        AddCardSpawn();
    }

    // 배치된 카드 & 계산중인 카드 콜라이더 비활성화
    public void ColliderQuit()
    {   
        card.QuitCollider();
        PokerManager.Instance.QuitCollider2();
    }
}
