using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Custom Card", menuName = "Custom/Card", order = 1)]
public class BaseCard : ScriptableObject
{
    public enum BaseCardType
    {
        Resource,
        Event,
        Action
    }

    public enum EventType
    {
        HandSizeChange,
        ResourceUsed, //Removal from bottom
        ResourceDamaged, //Removal from edges
        DrawCardType, //Draw from a deck
        ResourceCard, //Begin placing the shape
        NixEvent,//When an event of a type occurs, cancel it.
    }

    [System.Serializable]
    public class Event
    {
        public EventType eventType;

        [Header("Variables\n")]
        [Space(5f)]
        public Vector2Int valueRange;
        public BaseCardType cardType;
        public TetrisPiece.ResourceType resourceType;
        public EventType eventTypeNix;
        public TetrisPiece.PieceType tetrisPiece;
    }

    [System.Serializable]
    public class DelayedEvent : Event
    {
        public int turnDelay = 1;
    }

    public bool isFullCard = false;

    public BaseCardType baseCardType = BaseCardType.Action;

    public int cardTier = 1;
    public string cardName = "New Card";
    public string cardDescription = "DescriptionText";
    public string flavourText = "FlavourText";

    public List<Event> events = new List<Event>();
    public List<DelayedEvent> delayedEvents = new List<DelayedEvent>();

    public void DoCardBehaviour()
    {
        foreach (Event cardEvent in events)
        {
            switch (cardEvent.eventType)
            {
                case EventType.HandSizeChange:
                    ChangeHandSize(cardEvent.valueRange);
                    break;
                case EventType.ResourceUsed:
                    UseResource(cardEvent.resourceType);
                    break;
                case EventType.ResourceDamaged:
                    DamageResource(cardEvent.resourceType);
                    break;
                case EventType.DrawCardType:
                    DrawCardType(cardEvent.cardType, cardEvent.valueRange);
                    break;
                case EventType.ResourceCard:
                    UseResourceCard(cardEvent.tetrisPiece, cardEvent.resourceType);
                    break;
                case EventType.NixEvent:
                    break;
            }
        }
    }

    public void ChangeHandSize(Vector2 num)
    {
        Hand.Instance.HandSize += (int)Random.Range(num.x, num.y);
        Hand.Instance.UpdateHandSize();
    }

    public void UseResource(TetrisPiece.ResourceType resourceType)
    {

    }

    public void DamageResource(TetrisPiece.ResourceType resourceType)
    {

    }

    public void DrawCardType(BaseCard.BaseCardType cardType, Vector2 num)
    {
        int random = (int)Random.Range(num.x, num.y);

        if (random < 0)
        {
            for (int i = 0; i < random; i++)
            {
                Hand.Instance.DiscardCard(cardType);
            }
        }
        else if (random > 0)
        {
            for (int i = 0; i < random; i++)
            {
                Hand.Instance.DrawCard();
            }
        }
    }

    public void UseResourceCard(TetrisPiece.PieceType pieceType, TetrisPiece.ResourceType resourceType)
    {

        switch (pieceType)
        {
            case TetrisPiece.PieceType.RandomNonDot:
                UseResourceCard((TetrisPiece.PieceType)Random.Range(3, 9), resourceType);
                return;
            case TetrisPiece.PieceType.Random:
                UseResourceCard((TetrisPiece.PieceType)Random.Range(2, 9), resourceType);
                return;
            case TetrisPiece.PieceType.Dot:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/Dot");
                break;
            case TetrisPiece.PieceType.I:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/I");
                break;
            case TetrisPiece.PieceType.S:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/S");
                break;
            case TetrisPiece.PieceType.Z:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/Z");
                break;
            case TetrisPiece.PieceType.O:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/O");
                break;
            case TetrisPiece.PieceType.T:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/T");
                break;
            case TetrisPiece.PieceType.L:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/L");
                break;
            case TetrisPiece.PieceType.J:
                TetrisHandler.Instance.TetrisPiece = Resources.Load<TetrisPiece>("Tetrominoes/J");
                break;
        }

        switch (resourceType)
        {
            case TetrisPiece.ResourceType.Randomized:

                TetrisPiece.ResourceType primary = (TetrisPiece.ResourceType)Random.Range(1, 3);
                TetrisPiece.ResourceType secondary = (TetrisPiece.ResourceType)Random.Range(1, 3);

                for (int i = 0; i < TetrisHandler.Instance.TetrisPiece.squares.Count; i++)
                {
                    if (Random.Range(0, 100) < 50)
                    {
                        TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = primary;
                    }
                    else
                    {
                        TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = secondary;
                    }
                }

                break;
            case TetrisPiece.ResourceType.Air:
                for (int i = 0; i < TetrisHandler.Instance.TetrisPiece.squares.Count; i++)
                {
                    TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = TetrisPiece.ResourceType.Air;
                }
                break;
            case TetrisPiece.ResourceType.Metal:
                for (int i = 0; i < TetrisHandler.Instance.TetrisPiece.squares.Count; i++)
                {
                    TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = TetrisPiece.ResourceType.Metal;
                }
                break;
            case TetrisPiece.ResourceType.Fuel:
                for (int i = 0; i < TetrisHandler.Instance.TetrisPiece.squares.Count; i++)
                {
                    TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = TetrisPiece.ResourceType.Fuel;
                }
                break;
            case TetrisPiece.ResourceType.Flexible:
                for (int i = 0; i < TetrisHandler.Instance.TetrisPiece.squares.Count; i++)
                {
                    TetrisHandler.Instance.TetrisPiece.squares[i].resourceType = TetrisPiece.ResourceType.Flexible;
                }
                break;
        }

        TetrisHandler.Instance.isPlacing = true;
    }
}