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

    [SerializeField] Collider2D Collider2D;

    public ItemData itemdata;

    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // 카드 원본위치를 담은 PRS 클래스

    [SerializeField] SuitIDdata SuitIDdata;


    // 모든 텍스쳐를 다 넣어둘 배열
    Sprite[] sprites;

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();

        cardPrefabs = GetComponent<Transform>();
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

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    
    [SerializeField] private bool checkCard = false;

    // 마우스로 클릭하면 리스트에 카드 넣기 (최대5개)
    public void OnMouseDown()
    {
        //cardPrefabs.DORotate(new Vector3(0, 0, 10f), 0.2f);
        // 카드를 클릭하면 애니메이션
        cardPrefabs.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });


        SuitIDdata suitidData = new SuitIDdata(itemdata.suit, itemdata.id, this.gameObject);

        if (checkCard && PokerManager.Instance.SuitIDdata.Count <= 5)
        {
            PokerManager.Instance.RemoveSuitIDdata(suitidData);
            checkCard = false;
        }
        else if(PokerManager.Instance.SuitIDdata.Count < 5)
        {
            PokerManager.Instance.SaveSuitIDdata(suitidData);
            checkCard = true;
        }
        else
        {
            Debug.Log("더이상 카드를 누를 수 없음");
        }

        
        if (PokerManager.Instance.SuitIDdata.Count >= 0)
        {
            PokerManager.Instance.Hand();
            PokerManager.Instance.getHandType();
            // Debug.Log(PokerManager.Instance.saveNum.Count); // 카운트가 0 미만이면 출력 x

        }
    }

    


}