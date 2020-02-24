using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Position References")]
    public Transform EventDeckTransform;
    private List<CardObject> eventDeck = new List<CardObject>();
    public Transform PlayerDeckTransform;
    private List<CardObject> playerDeck = new List<CardObject>();
    public Transform TemporaryDeckTransform;
    private List<CardObject> unlockedDeck = new List<CardObject>();
    public Transform EventCardHolderTransform;
    private List<CardObject> temporaryDeck = new List<CardObject>();
    public Transform offScreenTransform;

    public List<Transform> CardHolders;

    [Header("Prefabs")]
    public GameObject baseCardObject;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("GameStats")]
    private int currentTurn;
    private int currentMovement;

    // Start is called before the first frame update
    void Start()
    {
        AddCards(Resources.LoadAll<CardObject>("Resources/Cards").ToList());
    }

    private IEnumerable AddCards(List<CardObject> allCards)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(5 / allCards.Count);

        foreach(CardObject cardObject in allCards)
        {
            if (cardObject.StartShuffleTurn < currentTurn)
            {
                for (int i = 0; i < cardObject.Frequency; i++)
                {
                    switch (cardObject.cardType)
                    {
                        case Utils.DeckType.Event:
                            eventDeck.Add(cardObject);
                            break;
                        case Utils.DeckType.Player:
                            playerDeck.Add(cardObject);
                            break;
                        case Utils.DeckType.Temporary:
                            unlockedDeck.Add(cardObject);
                            break;
                    }
                }

                yield return waitForSeconds;
            }
        }
    }

    void DrawCard(Utils.DeckType deckType)
    {
        switch (deckType)
        {
            case Utils.DeckType.Event:
                break;
            case Utils.DeckType.Player:
                break;
            case Utils.DeckType.Temporary:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
