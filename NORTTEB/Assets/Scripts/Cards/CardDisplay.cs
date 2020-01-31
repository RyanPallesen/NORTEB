using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI card1Title;
    public TextMeshProUGUI card1Description;

    public TextMeshProUGUI cardType;

    public TextMeshProUGUI card2Title;
    public TextMeshProUGUI card2Description;

    public BaseCard card1;
    public BaseCard card2;

    private void Start()
    {
        if(card1.isFullCard)
        {
            card2 = card1;
        }
        else if(card2.isFullCard)
        {
            card1 = card2;
        }
    }

    bool card1Displayed = true;

    public void UpdateText()
    {
        card1Title.text = card1.cardName;
        card1Description.text = card1.cardDescription;

        if(card1Displayed)
        {
            cardType.text = card1.baseCardType.ToString();
        }
        else
        {
            cardType.text = card2.baseCardType.ToString();
        }


    }

    public void RotateCard()
    {
        card1Displayed = !card1Displayed;
        transform.Rotate(new Vector3(0, 0, 180));
        UpdateText();
    }

    void FixedUpdate()
    {
    }
}
