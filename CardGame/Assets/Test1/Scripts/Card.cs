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
    [SerializeField] SpriteRenderer card; // �ո�
    [SerializeField] SpriteRenderer cardBack; // �޸��� ����

    [SerializeField] Sprite cardback; // �޸� �̹���
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;
    [SerializeField] SpriteRenderer spriteCards2;

    [SerializeField] Collider2D Collider2D;

    public ItemData itemdata;

    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    [SerializeField] SuitIDdata SuitIDdata;


    // ��� �ؽ��ĸ� �� �־�� �迭
    Sprite[] sprites;

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
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


    public void OnMouseDown()
    {
        if(PokerManager.Instance.SuitIDdata.Count < 5)
        {
            Debug.Log("ī�尡 Ŭ����");
            SuitIDdata suitidData = new SuitIDdata(itemdata.suit, itemdata.id );

            if (PokerManager.Instance != null)
            {
                PokerManager.Instance.SaveSuitIDdata(suitidData);
            }
        }
        else
        {
            Debug.Log("���̻� ī�带 ���� �� ����");
        }

    }
}