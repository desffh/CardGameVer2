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


    public ItemData itemdata;

    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    // ��� �ؽ��ĸ� �� �־�� �迭
    Sprite [] sprites;

    // �̺�Ʈ ���� -> �����Ϳ��� �Լ� ������
    public UnityEvent onEventTrggered;

    private void Start()
    {
        if(onEventTrggered == null)
        {
            onEventTrggered = new UnityEvent();
        }
    }

    private void OnMouseDown()
    {
        if(onEventTrggered != null)
        {
            onEventTrggered.Invoke();
        }    
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

    public String SuitData()
    {
        Debug.Log("���� ��ȯ");
        return itemdata.suit;
    }

    public int NumData()
    {
        Debug.Log("���� ��ȯ");
        return itemdata.id;
    }

    public void ReturnData()
    {
        PokerManager.Inst.dictionary.Add(SuitData(), NumData());
    }    

}
