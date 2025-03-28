using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PokerText;

    [SerializeField] TextMeshProUGUI PlusText;
    [SerializeField] TextMeshProUGUI MultiplyText;

    [SerializeField] TextMeshProUGUI TotalScoreText;

    [SerializeField] AnimationManager animationmanager;

    private RectTransform plusTextPosition;
    private RectTransform MultiTextPosition;

    [SerializeField] TextMeshProUGUI HandText;
    [SerializeField] TextMeshProUGUI DeleteText;

    [SerializeField] TextMeshProUGUI TotalCards;
    [SerializeField] TextMeshProUGUI HandCards;


    // ī�� ���� �� ���� �ؽ�Ʈ -> �ϳ��� ���������� ���ڸ� �ٲ㼭 ����
    //[SerializeField] GameObject Indexscore;

    
    private void Awake()
    {

    }

    private void Start()
    {
        HandText.text = GameManager.Instance.Hand.ToString();
        DeleteText.text = GameManager.Instance.Delete.ToString();

        // �ϴ� �ѹ� ȣ���ؼ� UI �� ����
        BufferUpdate();
        HandCardUpdate();
    }

    private void Update()
    {
        HandCardUpdate();
    }


    // ����Ʈ�� �ƹ� ���� ������� ������ �� �� ""
    public void PokerTextUpdate(string pokertext = "")
    {
        PokerText.text = pokertext;
    }
    // ���ϱ�
    public void PlusTextUpdate(int plussum = 0)
    {
        PlusText.text = plussum.ToString();
        animationmanager.CaltransformAnime(PlusText);
    }

    // ���ϱ�
    public void MultipleTextUpdate(int multisum = 0)
    {
        MultiplyText.text = multisum.ToString();

    }

    // ������ �ϼ��Ǹ� ȣ��
    public void PokerUpdate(int plus, int multiple)
    {
        PlusText.text = plus.ToString();
        MultiplyText.text = multiple.ToString();
    }

    // ��ü ����
    public void TotalScoreUpdate(int totalscore)
    {
        TotalScoreText.text = totalscore.ToString();
        animationmanager.CaltransformAnime(TotalScoreText);
    }

    public void HandCountUpdate(int handcount)
    {
        HandText.text = handcount.ToString();
        animationmanager.CaltransformAnime(HandText);
    }

    public void DeleteCountUpdate(int deletecount)
    {
        DeleteText.text = deletecount.ToString();
        animationmanager.CaltransformAnime(DeleteText);
    }

    public void BufferUpdate()
    {
        TotalCards.text = KardManager.Instance.itemBuffer.Count.ToString()
            + " / "+ KardManager.Instance.itemBuffer.Capacity.ToString();
    }

    public void HandCardUpdate()
    {
        HandCards.text = (KardManager.Instance.myCards.Capacity - PokerManager.Instance.CardIDdata.Count).ToString() + " / "
            + KardManager.Instance.myCards.Capacity.ToString();
    }

    /*[SerializeField] private GameObject parentTransform;

    public void IndexScore(int score)
    {
        // ������ �ؽ�Ʈ�� ���ڿ� �Ҵ�
        TextMeshProUGUI textComponent = Indexscore.GetComponent<TextMeshProUGUI>();

        textComponent.text = score.ToString();

        GameObject indexscore = Instantiate(Indexscore, parentTransform.transform);

        // ���� ��ǥ�� �ؽ�Ʈ ��ġ ���� (ī���� ��ġ�� �������� �߰�)
        Vector3 cardPosition = GameManager.Instance.SaveIndex(score).position;
        indexscore.transform.position = new Vector3(cardPosition.x + 5f,
                                                    cardPosition.y + 5f,
                                                    cardPosition.z + 5f);

        // ī���� ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.Instance.SaveIndex(score).position);

        // UI ��Ұ� ĵ������ ����� ��ġ�ϵ��� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        indexscore.transform.position = parentTransform.GetComponent<Canvas>().worldCamera.ScreenToWorldPoint(screenPos);


        animationmanager.IndexScoreAnime(indexscore, 1f);
    }*/

}
