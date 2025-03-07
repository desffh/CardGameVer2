using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerManager : MonoBehaviour
{

    // �̱���
    public static PokerManager Inst { get; private set; }

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �����Ϳ��� ��ũ��Ʈ �Ҵ�
    [SerializeField] Card card;

    [SerializeField] ItemData itemData;

    // ī���� suit, id�� �ľ� �� ��ųʸ��� ���� (Ű��, ������)
    [SerializeField] public Dictionary<string, int> dictionary
        = new Dictionary<string, int>();


    private void Start()
    {
        dictionary.Add(card.SuitData(), card.NumData());
    }

}
