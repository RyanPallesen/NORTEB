using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardObject : ScriptableObject
{

    //How many copies of this object are in the card pool
    [Range(0, 128)] public float Frequency;

    //The turn at which this will begin being shuffled into card pools
    public float StartShuffleTurn;

    public Utils.DeckType cardType;

}
