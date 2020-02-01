using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private static Hand _instance;

    public static Hand Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [HideInInspector] public List<CardDisplay> cards = new List<CardDisplay>();

    public int HandSize = 4;
    public GameObject prefabHandHolder;

    private List<GameObject> positions = new List<GameObject>();
    private Vector3 scale = Vector3.one;
    public int cardWidth;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateHandSize(HandSize);
    }

    private void UpdateHandSize(int final)
    {
        HandSize = final;

        foreach (GameObject go in positions)
        {
            Destroy(go);
        }
        positions.Clear();
        RebuildHandPositions();
    }

    public void UpdateHandSize()
    {
        UpdateHandSize(HandSize);
    }

    public void RebuildHandPositions()
    {
        float translatePer = 1420 / HandSize;

        float currentTranslation = -(1420 / 2);

        if (translatePer < cardWidth)
        {
            scale.x = translatePer / cardWidth;
            scale.y = translatePer / cardWidth;
        }
        else
        {
            scale = Vector3.one;
        }



        currentTranslation += translatePer / 2;
        for (int i = 0; i < HandSize; i++)
        {

            GameObject newGameObject = null;

            if (positions.Count > i && positions[i])
            {
                newGameObject = positions[i];
            }
            else
            {
                newGameObject = Instantiate<GameObject>(prefabHandHolder, transform);
                positions.Add(newGameObject);

            }

            (newGameObject.transform as RectTransform).localPosition = (new Vector3(currentTranslation, 0, 0));
            currentTranslation += translatePer;
        }

       
        for(int i =0; i < cards.Count; i++)
        {
            if (positions.Count > i)
            {
                cards[i].transform.parent.SetParent(positions[i].transform,false);
                cards[i].transform.localPosition = Vector3.zero;
                cards[i].transform.localScale = scale;
            }
        }


    }

    public void DiscardCard(BaseCard.BaseCardType cardType)
    {
        foreach(CardDisplay cardDisplay in cards)
        {
            if (cardDisplay.Card.cardPrimary.baseCardType == cardType)
            {
                DiscardCard(cardDisplay);

                return;
            }
        }

    }

    public void DiscardCard(int card)
    {
        DiscardCard(cards[card]);
    }

    public void DiscardCard(CardDisplay card)
    {
        cards.Remove(card);
        Destroy(card.transform.parent.gameObject);
        RebuildHandPositions();
    }

    public void DrawCard()
    {
        if (cards.Count <= HandSize)
        {
            GameObject card = Deck.Instance.GetCard(0);
            
            CardDisplay drawnCard = card.GetComponentInChildren<CardDisplay>(); ;

            cards.Add(drawnCard);
            RebuildHandPositions();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DrawCard();
        }
    }
}