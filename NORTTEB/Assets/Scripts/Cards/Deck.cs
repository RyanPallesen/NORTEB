using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour, IPointerEnterHandler
{
    public BaseCard.BaseCardType DeckType;
    public GameObject cardPrefab;

    public Dictionary<int, List<BaseCard>> keyValuePairs = new Dictionary<int, List<BaseCard>>();



    private static readonly Deck[] _instance = new Deck[3];

    public static Deck Resource { get { return _instance[0]; } }
    public static Deck Event { get { return _instance[1]; } }
    public static Deck Action { get { return _instance[2]; } }


    private void Awake()
    {
        switch (DeckType)
        {
            case BaseCard.BaseCardType.Resource:
                if (_instance[0] != null && _instance[0] != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _instance[0] = this;


                    foreach (ScriptableObject @object in Resources.LoadAll("Resources/Cards/Resource"))
                    {
                        BaseCard card = @object as BaseCard;
                        if (card)
                        {
                            if (!keyValuePairs.ContainsKey(card.cardTier))
                            {
                                keyValuePairs[card.cardTier] = new List<BaseCard>();
                            }

                            keyValuePairs[card.cardTier].Add(card);
                        }
                    }
                }
                break;
            case BaseCard.BaseCardType.Event:
                if (_instance[1] != null && _instance[1] != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    foreach (ScriptableObject @object in Resources.LoadAll("Resources/Cards/Event"))
                    {
                        BaseCard card = @object as BaseCard;
                        if (card)
                        {
                            if (!keyValuePairs.ContainsKey(card.cardTier))
                            {
                                keyValuePairs[card.cardTier] = new List<BaseCard>();
                            }

                            keyValuePairs[card.cardTier].Add(card);
                        }
                    }
                    _instance[1] = this;
                }
                break;
            case BaseCard.BaseCardType.Action:
                if (_instance[2] != null && _instance[2] != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Created action deck");
                    foreach (ScriptableObject @object in Resources.LoadAll<ScriptableObject>("Cards/Action"))
                    {
                        Debug.Log("Loaded an object");

                        BaseCard card = @object as BaseCard;
                        if (card)
                        {
                            Debug.Log("Object existed as basecard");

                            if (!keyValuePairs.ContainsKey(card.cardTier))
                            {
                                Debug.Log("Created keyvaluepair");
                                keyValuePairs[card.cardTier] = new List<BaseCard>();
                            }

                            keyValuePairs[card.cardTier].Add(card);
                        }
                    }
                    _instance[2] = this;
                }
                break;
        }


    }

    public GameObject GetCard(int tier)
    {
        Card card = new Card();

        card.cardPrimary = GetCardOfTier(tier);
        card.cardSecondary = GetCardOfTier(tier);
        card.cardTier = tier;

        GameObject go = Instantiate(cardPrefab,this.transform);
        go.GetComponentInChildren<CardDisplay>().Card = card;

        return go;
    }


    public BaseCard GetCardOfTier(int tier)
    {
        List<BaseCard> cards = keyValuePairs[tier];
        int random = Random.Range(0, cards.Count);
        Debug.Log(random);
        return (cards[random]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipHandler.Instance.textmesh.text = "The " + DeckType.ToString() + " deck";
    }
}
