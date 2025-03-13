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

// Card�� �� ��ũ��Ʈ
public class Card : MonoBehaviour
{
    private Transform cardPrefabs;

    [SerializeField] SpriteRenderer card; // �ո�
    [SerializeField] SpriteRenderer cardBack; // �޸��� ����

    [SerializeField] Sprite cardback; // �޸� �̹���
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;
    [SerializeField] SpriteRenderer spriteCards2;

    [SerializeField] BoxCollider2D Collider2D;
    
    public ItemData itemdata;

    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    [SerializeField] SuitIDdata SuitIDdata;

    // ��� �ؽ��ĸ� �� �־�� �迭
    Sprite[] sprites;


    


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
        // ī�� ��������Ʈ �̸� �޾ƿ�
        string spriteName = item.front;

        // ��� ��������Ʈ �迭�� �� �ֱ�
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);


        if (sprites.Length > 0)
        {
            Sprite selctedSprite = sprites.FirstOrDefault(s => s.name == spriteName);

            if (selctedSprite != null)
            {
                spriteCards.sprite = selctedSprite;  // ������ ��������Ʈ�� ī�忡 ����
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
    
    [SerializeField] private bool checkCard = false;

    // ���콺�� Ŭ���ϸ� ����Ʈ�� ī�� �ֱ� (�ִ�5��)
    // �ݶ��̴��� ������ Card������Ʈ�� Ŭ�� �� �� �ִ�
    public void OnMouseDown()
    { 

        SuitIDdata suitidData = new SuitIDdata(itemdata.suit, itemdata.id, this.gameObject);

        if (checkCard && PokerManager.Instance.SuitIDdata.Count <= 5)
        {
            PokerManager.Instance.RemoveSuitIDdata(suitidData);
            
            // ���� ��ġ�� �̵�
            cardPrefabs.DOMove(new Vector3(cardPrefabs.transform.position.x,
               cardPrefabs.transform.position.y -0.5f,
               cardPrefabs.transform.position.z), 0.2f);

            checkCard = false;
        }
        else if(PokerManager.Instance.SuitIDdata.Count < 5)
        {
            PokerManager.Instance.SaveSuitIDdata(suitidData);
            
            // Ŭ�� �ִϸ��̼�
            cardPrefabs.DOMove(new Vector3(cardPrefabs.transform.position.x,
               cardPrefabs.transform.position.y + 0.5f,
               cardPrefabs.transform.position.z), 0.2f);
            
            checkCard = true;
        }
        else
        {
            Debug.Log("���̻� ī�带 ���� �� ����");
            
            //cardPrefabs.DORotate(new Vector3(0, 0, 10f), 0.2f);
            // ī�带 Ŭ���ϸ� �ִϸ��̼�
            cardPrefabs.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });
        }
        
        
        if (PokerManager.Instance.SuitIDdata.Count >= 0)
        {
            PokerManager.Instance.Hand();
            PokerManager.Instance.getHandType();
            // Debug.Log(PokerManager.Instance.saveNum.Count); // ī��Ʈ�� 0 �̸��̸� ��� x

        }
    }

    // �ݶ��̴� ��Ȱ��ȭ
    public void QuitCollider()
    {
        Debug.Log("ī��� �ݶ��̴� ��Ȱ��ȭ");

        for (int i = 0; i < KardManager.Inst.myCards.Count; i++)
        {
            KardManager.Inst.myCards[i].Collider2D.enabled = false;
        }
    }

    // �ݶ��̴� Ȱ��ȭ
    public void StartCollider()
    {
        Debug.Log("ī��� �ݶ��̴� Ȱ��ȭ");
        for(int i = 0; i < KardManager.Inst.myCards.Count; i++)
        {
            KardManager.Inst.myCards[i].Collider2D.enabled = true;  
        }
    }
}