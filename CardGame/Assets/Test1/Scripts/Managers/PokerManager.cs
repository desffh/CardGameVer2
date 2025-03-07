using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerManager : MonoBehaviour
{

    // 싱글톤
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

    // 에디터에서 스크립트 할당
    [SerializeField] Card card;

    [SerializeField] ItemData itemData;

    // 카드의 suit, id를 파악 후 딕셔너리에 저장 (키값, 벨류값)
    [SerializeField] public Dictionary<string, int> dictionary
        = new Dictionary<string, int>();


    private void Start()
    {
        dictionary.Add(card.SuitData(), card.NumData());
    }

}
