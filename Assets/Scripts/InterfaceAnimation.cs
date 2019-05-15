using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InterfaceAnimation : MonoBehaviour
{

    public static InterfaceAnimation instance;

    public CanvasGroup canvasGroup;
    public RectTransform animationSquare;
    public Sprite noTargetSprite;
    public Sprite targetSprite;

    private void Awake()
    {
        instance = this;
        canvasGroup.alpha = 0;
        animationSquare.DOSizeDelta(animationSquare.sizeDelta / 1.4f, .4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void Show(bool state)
    {
        float alpha = state ? 1 : 0;
        canvasGroup.DOFade(alpha, .2f);
    }

    public void Target(bool state)
    {
        Sprite s = state ? targetSprite : noTargetSprite;
        for (int i = 0; i < animationSquare.transform.childCount; i++)
        {
            animationSquare.transform.GetChild(i).GetComponent<Image>().sprite = s;
        }
    }

}
