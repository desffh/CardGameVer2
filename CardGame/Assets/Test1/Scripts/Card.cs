using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.U2D;
using System.Linq;
using Newtonsoft.Json.Bson;
using UnityEngine.Events;
using System;

// Card에 들어갈 스크립트
public class Card : MonoBehaviour
{
    private Transform cardPrefabs;

    [SerializeField] SpriteRenderer card; // 앞면
    [SerializeField] SpriteRenderer cardBack; // 뒷면은 통일

    [SerializeField] Sprite cardback; // 뒷면 이미지
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;
    [SerializeField] SpriteRenderer spriteCards2;

    [SerializeField] BoxCollider2D Collider2D;
    
    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // 카드 원본위치를 담은 PRS 클래스

    // 모든 텍스쳐를 다 넣어둘 배열
    Sprite[] sprites;

    // SetUp 함수로 뽑은 카드의 구조체 정보를 받아와서 저장
    public ItemData itemdata;
    
    // 카드가 눌렸는지 확인
    [SerializeField] private bool checkCard = false;

    private void Awake()
    {
        Collider2D = GetComponent<BoxCollider2D>();

        cardPrefabs = GetComponent<Transform>();
    }

    private void Start()
    {
        Collider2D.enabled = true;
    }

    public void Setup(ItemData item)
    {
        spriteCards = transform.GetChild(0).GetComponent<SpriteRenderer>();

        this.itemdata = item;
        // 카드 스프라이트 이름 받아옴
        string spriteName = item.front;

        // 모든 스프라이트 배열에 다 넣기
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites.Length > 0)
        {
            Sprite selctedSprite = sprites.FirstOrDefault(s => s.name == spriteName);

            if (selctedSprite != null)
            {
                spriteCards.sprite = selctedSprite;  // 선택한 스프라이트를 카드에 적용
            }
        }

        if(cardback != null)
        {
            spriteCards2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            cardBack.sprite = cardback;
        }
    }

    // 카드 정렬 애니메이션
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime).SetDelay(0.2f);
            transform.DORotateQuaternion(prs.rot, dotweenTime).SetDelay(0.2f);
            transform.DOScale(prs.scale, dotweenTime).SetDelay(0.2f);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    
    // 마우스로 클릭하면 CardIDdata 리스트에 카드 넣기 (최대5개)
    // 콜라이더가 부착된 Card오브젝트를 클릭 할 수 있다
    public void OnMouseDown()
    {
        // 리스트가 꽉 찼다면
        if (checkCard && PokerManager.Instance.CardIDdata.Count <= 5)
        {
            // 이 스크립트가 달린 Card를 매개변수로 전달
            PokerManager.Instance.RemoveSuitIDdata(this);
            
            // 기존 위치로 이동
            cardPrefabs.DOMove(new Vector3(cardPrefabs.transform.position.x,
               cardPrefabs.transform.position.y -0.5f,
               cardPrefabs.transform.position.z), 0.2f);

            checkCard = false;
        }
        // 리스트가 덜 찼다면
        else if(PokerManager.Instance.CardIDdata.Count < 5)
        {
            PokerManager.Instance.SaveSuitIDdata(this);
            
            // 클릭 애니메이션
            cardPrefabs.DOMove(new Vector3(cardPrefabs.transform.position.x,
               cardPrefabs.transform.position.y + 0.5f,
               cardPrefabs.transform.position.z), 0.2f);
            
            checkCard = true;
        }
        else
        {
            Debug.Log("더이상 카드를 누를 수 없음");

            // 카드를 클릭하면 애니메이션
            cardPrefabs.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });
        }
             
        // 족보 확인
        if (PokerManager.Instance.CardIDdata.Count >= 0)
        {
            PokerManager.Instance.Hand();
            PokerManager.Instance.getHandType();
        }
    }

    // 카드 콜라이더 비활성화
    public void QuitCollider()
    {
        Debug.Log("카드들 콜라이더 비활성화");
        
        // 소지중인 카드
        for (int i = 0; i < KardManager.Instance.myCards.Count; i++)
        {
            KardManager.Instance.myCards[i].Collider2D.enabled = false;
        }

        // 계산중인 카드
        for (int i = 0; i < PokerManager.Instance.CardIDdata.Count; i++)
        {
            PokerManager.Instance.CardIDdata[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // 콜라이더 활성화
    public void StartCollider()
    {
        Debug.Log("카드들 콜라이더 활성화");

        for(int i = 0; i < KardManager.Instance.myCards.Count; i++)
        {
            KardManager.Instance.myCards[i].Collider2D.enabled = true;  
        }
    }
}