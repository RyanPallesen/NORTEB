using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI card1Title;
    public TextMeshProUGUI card1Description;

    public TextMeshProUGUI cardType;

    public TextMeshProUGUI card2Title;
    public TextMeshProUGUI card2Description;

    private Card card;

    private void Start()
    {
        if(Card.cardPrimary.isFullCard)
        {
            Card.cardSecondary = Card.cardPrimary;
        }
        else if(Card.cardSecondary.isFullCard)
        {
            Card.cardPrimary = Card.cardSecondary;
        }
    }

    public bool card1Displayed = true;

    public Card Card { get => card; set {card = value; UpdateText(); } }

    public void UpdateText()
    {
        card1Title.text = Card.cardPrimary.cardName;
        card1Description.text = Card.cardPrimary.cardDescription;

        card2Title.text = Card.cardSecondary.cardName;
        card2Description.text = Card.cardSecondary.cardDescription;

        if (card1Displayed)
        {
            cardType.text = Card.cardPrimary.baseCardType.ToString();
        }
        else
        {
            cardType.text = Card.cardSecondary.baseCardType.ToString();
        }


    }

    public void RotateCard()
    {
        card1Displayed = !card1Displayed;
        transform.Rotate(new Vector3(0, 0, 180));
        UpdateText();
    }

    public void DoCard()
    {
        if (card1Displayed)
        {
            Card.cardPrimary.DoCardBehaviour();
        }
        else
        {
            Card.cardSecondary.DoCardBehaviour();
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (card1Displayed)
        {
            ToolTipHandler.Instance.textmesh.text = Card.cardPrimary.flavourText;

        }
        else
        {
            ToolTipHandler.Instance.textmesh.text = Card.cardSecondary.flavourText;

        }
    }
}
