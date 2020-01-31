using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [HideInInspector] public List<CardDisplay> cards = new List<CardDisplay>();

    public int HandSize = 4;
    public GameObject prefabHandHolder;

    private List<GameObject> positions = new List<GameObject>();
    private Vector3 scale = Vector3.one;
    public int cardWidth;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateHandSize(5);
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

    public void RebuildHandPositions()
    {
        float translatePer = 1420 / HandSize;

        float currentTranslation = -(1420 / 2);

        currentTranslation += translatePer / 2;
        for (int i = 0; i < HandSize; i++)
        {
            GameObject newGameObject = Instantiate<GameObject>(prefabHandHolder, transform);

            (newGameObject.transform as RectTransform).localPosition = (new Vector3(currentTranslation, 0, 0));
            currentTranslation += translatePer;
            positions.Add(newGameObject);
        }

        if (translatePer < cardWidth)
        {
            scale.x = translatePer / cardWidth;
        }
        else
        {
            scale = Vector3.one;
        }
    }

    public void DrawCard(BaseCard.BaseCardType deckType)
    {
        GameObject card = null;
        CardDisplay drawnCard = null;

        switch (deckType)
        {
            case BaseCard.BaseCardType.Resource:
                card = (Deck.Resource.GetCard(0));
                break;
            case BaseCard.BaseCardType.Event:
                card = (Deck.Event.GetCard(0));
                break;
            case BaseCard.BaseCardType.Action:
                card = (Deck.Action.GetCard(0));
                break;
        }

        drawnCard = card.GetComponentInChildren<CardDisplay>();

        card.transform.parent = positions[cards.Count].transform;
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = scale;

        cards.Add(drawnCard);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            DrawCard(BaseCard.BaseCardType.Action);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DrawCard(BaseCard.BaseCardType.Event);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCard(BaseCard.BaseCardType.Resource);
        }
    }
}