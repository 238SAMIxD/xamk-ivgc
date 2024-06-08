using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardData cardData = null;
    public TMP_Text titleText = null;
    public TMP_Text descriptionText = null;
    public Image damageImage = null;
    public Image costImage = null;
    public Image cardImage = null;
    public Image frameImage = null;
    public Image burnImage = null;

    public void Initialize() {
        if (cardData == null) return;
        titleText.text = cardData.cardTitle;
        descriptionText.text = cardData.cardDescription;
        cardImage.sprite = cardData.cardImage;
        frameImage.sprite = cardData.frameImage;
        costImage.sprite = GameController.instance.health[cardData.cost];
        damageImage.sprite = GameController.instance.damage[cardData.damage];
    }
}
