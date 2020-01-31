using UnityEngine;

public class BaseCard : ScriptableObject
{
    public enum BaseCardType
    {
        Resource,
        Event,
        Action
    }

    [HideInInspector] public BaseCardType baseCardType;

    public string cardName;
    public string cardDescription;
}