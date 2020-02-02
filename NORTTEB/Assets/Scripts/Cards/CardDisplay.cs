using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public TextMeshProUGUI card1Title;
    public TextMeshProUGUI card1Description;

    public TextMeshProUGUI cardType;

    public TextMeshProUGUI card2Title;
    public TextMeshProUGUI card2Description;

    private Card card;

    public List<TetrisPiece.Square> primarySquares;
    public List<TetrisPiece.Square> secondarySquares;

    public GameObject tetrisObjectPrimary;
    public GameObject tetrisObjectSecondary;

    public void Init()
    {
        if (Card.cardPrimary.isFullCard)
        {
            Card.cardSecondary = Card.cardPrimary;
        }
        else if (Card.cardSecondary.isFullCard)
        {
            Card.cardPrimary = Card.cardSecondary;
        }

        foreach (BaseCard.Event cardEvent in card.cardPrimary.events)
        {
            if (cardEvent.eventType == BaseCard.EventType.ResourceCard)
            {
                GameObject obj = card.cardPrimary.UseResourceCard(cardEvent.tetrisPiece, cardEvent.resourceType, out primarySquares);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(0, 80, 0);

                tetrisObjectPrimary = obj;
            }
        }
        foreach (BaseCard.Event cardEvent in card.cardSecondary.events)
        {
            if (cardEvent.eventType == BaseCard.EventType.ResourceCard)
            {
                GameObject obj = card.cardSecondary.UseResourceCard(cardEvent.tetrisPiece, cardEvent.resourceType, out secondarySquares);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(0, -210, 0);

                tetrisObjectSecondary = obj;
            }
        }
    }

    public bool card1Displayed = true;

    public Card Card { get => card; set { card = value; UpdateText(); } }

    public void UpdateText()
    {
        if (card1Displayed)
        {
            cardType.text = Card.cardPrimary.baseCardType.ToString();

            card1Title.text = Card.cardPrimary.cardName;
            card1Description.text = Card.cardPrimary.cardDescription;

            card2Title.text = Card.cardSecondary.cardName;
            card2Description.text = Card.cardSecondary.cardDescription;
        }
        else
        {
            cardType.text = Card.cardPrimary.baseCardType.ToString();

            card2Title.text = Card.cardPrimary.cardName;
            card2Description.text = Card.cardPrimary.cardDescription;

            card1Title.text = Card.cardSecondary.cardName;
            card1Description.text = Card.cardSecondary.cardDescription;
        }



    }

    public void RotateCard()
    {
        card1Displayed = !card1Displayed;
        Vector3 tempPos = tetrisObjectSecondary.transform.position;

        tetrisObjectSecondary.transform.position = tetrisObjectPrimary.transform.position;
        tetrisObjectPrimary.transform.position = tempPos;
        //transform.Rotate(new Vector3(0, 0, 180));
        UpdateText();
    }

    public void DoCard()
    {
        bool shouldDiscard = false;
        if (card1Displayed)
        {
            shouldDiscard = Card.cardPrimary.DoCardBehaviour(this);
        }
        else
        {
            shouldDiscard = Card.cardSecondary.DoCardBehaviour(this);
        }

        if (shouldDiscard)
        {
            Hand.Instance.DiscardCard(this);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DoCard();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RotateCard();
        }

    }

}
