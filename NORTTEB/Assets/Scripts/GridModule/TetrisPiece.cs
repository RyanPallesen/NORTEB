using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetrisPiece : ScriptableObject {

    public enum ResourceType
    {
    Air,
    Metal,
    Fuel
    }

    struct Square
    {
        int xOffset;
        int yOffset;

        [HideInInspector] public ResourceType resourceType;
    }

}
