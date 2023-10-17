using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardControllerInCardMatch : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private Sprite cardSprite;

    [SerializeField]
    private Sprite backSprite;

    private bool isFlipped = false;
    private bool isFlipping = false;
    public int cardID;

    public void SetCardID(int id)
    {
        cardID = id;
    }

    public void SetCardSprite(Sprite sprite)
    {
        this.cardSprite = sprite;
    }

    public void FlipCard()
    {
        isFlipping = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFlipped = !isFlipped; //Ŭ���� ������ ��� ������

            if (isFlipped)
                cardRenderer.sprite = cardSprite;
            else
                cardRenderer.sprite = backSprite;
        });

        transform.DOScale(originalScale, 0.2f).OnComplete(() =>
        {
            isFlipping = false;
        });
    }

    private void OnMouseDown()
    {
        if (!isFlipping && !isFlipped)  //����ī�尡 ���������� �ʰ�, ���� ī�尡 ��ġ�� ���� �ʰ�
            GameManagerInCardMatch.instance.CardClicked(this);

        //�޸��� �������� �ִٸ� ������ �� �� �ְ�, �ո��� �������� ���� ���� ���콺 Ŭ�����δ� ������ ���ϰ�, �׸ſ��� �ľ��ؼ� ��ġ���� �ʴ� ��쿡�� �����ǵ���
    }

}
