﻿using System.Collections.Generic;
using System.Linq;
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
        ConditionalDamage, //Supply X tetris piece or take Y damage
        ResourceDamaged, //Removal from edges
        DrawCardType, //Draw from a deck
        ResourceCard, //Begin placing the shape
        NixEvent,//When an event of a type occurs, cancel it.
        Junk,//Stays around for one turn
        Move,
        ShuttleDamaged//Permanently destroy a resource-placing square
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

    public int timeInHand;
    public int cardTier = 1;
    public int frequency = 0;
    public string cardName = "New Card";
    public string cardDescription = "DescriptionText";
    public string flavourText = "FlavourText";

    public List<Event> events = new List<Event>();
    public List<DelayedEvent> delayedEvents = new List<DelayedEvent>();

    private List<GameObject> tetrisObject = new List<GameObject>();

    public bool DoCardBehaviour()
    {
        bool shouldDiscard = true;



        foreach (Event cardEvent in events)
        {
            switch (cardEvent.eventType)
            {
                case EventType.HandSizeChange:
                    ChangeHandSize(cardEvent.valueRange);
                    break;
                case EventType.ConditionalDamage:

                    break;
                case EventType.ResourceDamaged:
                    DamageResource(cardEvent.resourceType, cardEvent.valueRange);
                    break;
                case EventType.DrawCardType:
                    DrawCardType(cardEvent.cardType, cardEvent.valueRange);
                    break;
                case EventType.ResourceCard:
                    AddResourceCache(cardEvent.tetrisPiece, cardEvent.resourceType, cardEvent.valueRange);
                    break;
                case EventType.NixEvent:

                    List<CardDisplay> discards = new List<CardDisplay>();
                    foreach(CardDisplay cardDisplay in Hand.Instance.cards)
                    {
                        if(cardDisplay.Card.cardPrimary.baseCardType == BaseCardType.Event)
                        {
                            discards.Add(cardDisplay);
                        }

                        if (cardDisplay.Card.cardSecondary.baseCardType == BaseCardType.Event)
                        {
                            discards.Add(cardDisplay);
                        }
                    }

                    for(int i = 0; i < discards.Count; i++)
                    {
                        Hand.Instance.DiscardCard(discards[i]);
                    }

                    break;
                case EventType.Junk:
                    shouldDiscard = false;
                    break;
                case EventType.Move:
                    Hand.Instance.Movement += (int)Random.Range(cardEvent.valueRange.x, cardEvent.valueRange.y);
                    break;
                case EventType.ShuttleDamaged:
                    DamageShuttle(cardEvent.resourceType, cardEvent.valueRange);
                    break;
            }
        }

        return shouldDiscard;
    }

    public void ChangeHandSize(Vector2 num)
    {
        Hand.Instance.HandSize += (int)Random.Range(num.x, num.y);
        Hand.Instance.UpdateHandSize();
    }

    public void DamageShuttle(TetrisPiece.ResourceType resourceType, Vector2 num)
    {
        switch (resourceType)

        {
            case TetrisPiece.ResourceType.Randomized:
                DamageShuttle((TetrisPiece.ResourceType)Random.Range(1, 5), num);
                break;
            case TetrisPiece.ResourceType.Air:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.airObject.transform.childCount > TetrisHandler.Instance.airList.Count)
                        {
                            List<Transform> childs = TetrisHandler.Instance.airObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.airObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1)
                                {
                                    truechilds.Add(child);
                                }
                            }

                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            Destroy(randomObject);
                        }
                        else if (TetrisHandler.Instance.airList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.airList.Count);
                            GameObject temp = TetrisHandler.Instance.airList[index];
                            TetrisHandler.Instance.airList.RemoveAt(index);
                            Destroy(temp);
                        }

                    }
                }
                break;
            case TetrisPiece.ResourceType.Metal:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.metalObject.transform.childCount > TetrisHandler.Instance.metalList.Count)
                        {
                            List<Transform> childs = TetrisHandler.Instance.metalObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.metalObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1)
                                {
                                    truechilds.Add(child);
                                }
                            }

                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            Destroy(randomObject);
                        }
                        else if (TetrisHandler.Instance.fuelList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.metalList.Count);
                            GameObject temp = TetrisHandler.Instance.metalList[index];
                            TetrisHandler.Instance.metalList.RemoveAt(index);
                            Destroy(temp);
                        }

                    }
                }
                break;
            case TetrisPiece.ResourceType.Fuel:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.fuelObject.transform.childCount > TetrisHandler.Instance.fuelList.Count)
                        {
                            List<Transform> childs = TetrisHandler.Instance.fuelObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.fuelObject.transform);

                            List<Transform> truechilds = new List<Transform>();
                            foreach (Transform child in childs)
                            {
                                if (child.childCount < 1)
                                {
                                    truechilds.Add(child);
                                }
                            }

                            GameObject randomObject = (GameObject)((Transform)truechilds[Random.Range(0, truechilds.Count)]).gameObject;

                            Destroy(randomObject);
                        }
                        else if (TetrisHandler.Instance.fuelList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.fuelList.Count);
                            GameObject temp = TetrisHandler.Instance.fuelList[index];
                            TetrisHandler.Instance.fuelList.RemoveAt(index);
                            Destroy(temp);
                        }

                    }
                }
                break;
            case TetrisPiece.ResourceType.Flexible:
                break;

        }
    }

    public void DamageResource(TetrisPiece.ResourceType resourceType, Vector2 num)
    {
        switch (resourceType)
        {
            case TetrisPiece.ResourceType.Randomized:
                DamageResource((TetrisPiece.ResourceType)Random.Range(1, 5), num);
                break;
            case TetrisPiece.ResourceType.Air:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.airList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.airList.Count);
                            GameObject temp = TetrisHandler.Instance.airList[index];
                            TetrisHandler.Instance.airList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.airObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.airObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.airObject.transform);
                            GameObject randomObject = (GameObject)((Transform)childs[Random.Range(0, childs.Count)]).gameObject;
                            Destroy(randomObject);
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Metal:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.metalList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.metalList.Count);
                            GameObject temp = TetrisHandler.Instance.metalList[index];
                            TetrisHandler.Instance.metalList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.metalObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.metalObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.metalObject.transform);
                            GameObject randomObject = (GameObject)((Transform)childs[Random.Range(0, childs.Count)]).gameObject;
                            Destroy(randomObject);
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Fuel:
                {
                    int random = (int)Random.Range(num.x, num.y);

                    for (int i = 0; i < random; i++)
                    {
                        if (TetrisHandler.Instance.fuelList.Count > 0)
                        {
                            int index = Random.Range(0, TetrisHandler.Instance.fuelList.Count);
                            GameObject temp = TetrisHandler.Instance.fuelList[index];
                            TetrisHandler.Instance.fuelList.RemoveAt(index);
                            Destroy(temp);
                        }
                        else if (TetrisHandler.Instance.fuelObject.transform.childCount > 0)
                        {
                            List<Transform> childs = TetrisHandler.Instance.fuelObject.GetComponentsInChildren<Transform>().ToList();
                            childs.Remove(TetrisHandler.Instance.fuelObject.transform);
                            GameObject randomObject = (GameObject)((Transform)childs[Random.Range(0, childs.Count)]).gameObject;
                            Destroy(randomObject);
                        }
                    }
                }
                break;
            case TetrisPiece.ResourceType.Flexible:
                break;
        }
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


    [System.Serializable]
    public struct resourceCache
    {
        public TetrisPiece.PieceType PieceType;
        public TetrisPiece.ResourceType ResourceType;
    }

    public void AddResourceCache(TetrisPiece.PieceType pieceType, TetrisPiece.ResourceType resourceType, Vector2 num)
    {
        if (num == Vector2.zero)
        {
            num = Vector2.one;
        }

        int random = (int)Random.Range(num.x, num.y);
        for (int i = 0; i < random; i++)
        {
            Debug.Log(i);
            TetrisHandler.Instance.resources.Add(new resourceCache { PieceType = pieceType, ResourceType = resourceType });
        }
    }

    public static void UseResourceCard(TetrisPiece.PieceType pieceType, TetrisPiece.ResourceType resourceType)
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

                TetrisPiece.ResourceType primary = (TetrisPiece.ResourceType)Random.Range(1, 4);
                TetrisPiece.ResourceType secondary = (TetrisPiece.ResourceType)Random.Range(1, 4);

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

        GameObject ParentObject = new GameObject();
        ParentObject.transform.position = Vector3.zero;

        foreach (TetrisPiece.Square square in TetrisHandler.Instance.TetrisPiece.squares)
        {
            GameObject cube = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/TetrisCube"));

            cube.transform.SetParent(ParentObject.transform);

            cube.transform.localPosition = new Vector3(square.yOffset, square.xOffset);

            cube.GetComponent<Renderer>().material.SetFloat("_Type", (int)square.resourceType);
            cube.GetComponent<TetrisTag>().ResourceType = square.resourceType;

        }

        TetrisHandler.Instance.tetrisObj = ParentObject;


        TetrisHandler.Instance.isPlacing = true;
    }
}