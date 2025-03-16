using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;

    private void Awake()
    {
        GameOverPanel = this.gameObject; // 현재 스크립트가 붙은 객체 참조 (에디터 할당도 가능)
    }

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    // 핸드가 0이고 종료되었을때,
    public void GameOver()
    {

    }
}
