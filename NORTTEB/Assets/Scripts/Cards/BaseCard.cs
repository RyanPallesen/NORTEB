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
        public TetrisPiece tetrisPiece;
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
}