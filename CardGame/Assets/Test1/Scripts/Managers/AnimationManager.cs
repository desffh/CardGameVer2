using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class AnimationManager : MonoBehaviour
{

    // 스코어들 텍스트
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
        // 원래 폰트 크기 저장
        float originalFontSize = scoreText.fontSize;

        // 글씨 크기를 키우는 애니메이션
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.1f, 0.1f)
        .OnComplete(() =>
        {
            // 글씨 크기를 다시 원래 크기로 줄이는 애니메이션
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.1f);
        });
    }
}
