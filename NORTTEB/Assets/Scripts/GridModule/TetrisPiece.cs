using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomPiece", menuName = "Custom/TetrisPiece", order = 1)]
public class TetrisPiece : ScriptableObject
{

    public enum ResourceType //note; randomize between two of the existing resources
    {
        Randomized,
        Air,
        Metal,
        Fuel,
        Flexible
    }

    public enum PieceType
    {
        RandomNonDot,
        Random,
        Dot,
        I,
        S,
        Z,
        O,
        T,
        L,
        J,
    }

    [System.Serializable]
    public struct Square
    {
        public int xOffset;
        public int yOffset;

        [HideInInspector] public ResourceType resourceType;
    }

    public List<Square> squares = new List<Square>();
}
