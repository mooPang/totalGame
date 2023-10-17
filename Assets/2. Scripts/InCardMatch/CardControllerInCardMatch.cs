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
            isFlipped = !isFlipped; //클릭할 때마다 계속 뒤집음

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
        if (!isFlipping && !isFlipped)  //현재카드가 뒤집혀지지 않고, 현재 카드가 매치가 되지 않고
            GameManagerInCardMatch.instance.CardClicked(this);

        //뒷면이 보여지고 있다면 뒤집어 볼 수 있고, 앞면이 보여지고 있을 때는 마우스 클릭으로는 뒤집지 못하고, 겜매에서 파악해서 매치되지 않는 경우에만 원복되도록
    }

}
