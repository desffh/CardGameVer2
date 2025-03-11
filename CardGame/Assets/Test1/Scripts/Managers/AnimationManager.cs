using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class AnimationManager : MonoBehaviour
{

    // ���ھ�� �ؽ�Ʈ
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
        // ���� ��Ʈ ũ�� ����
        float originalFontSize = scoreText.fontSize;

        // �۾� ũ�⸦ Ű��� �ִϸ��̼�
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.1f, 0.1f)
        .OnComplete(() =>
        {
            // �۾� ũ�⸦ �ٽ� ���� ũ��� ���̴� �ִϸ��̼�
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.1f);
        });
    }
}
