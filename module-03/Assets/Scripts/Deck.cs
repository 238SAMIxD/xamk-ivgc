using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Deck
{
    public List<CardData> cards = new List<CardData>();

    internal void Create() {
        List<CardData> allCards = new List<CardData>();
        foreach (CardData card in GameController.instance.cards) {
            for(int i = 0; i < card.count; i++) {
                allCards.Add(card);
            }
        }

        while (allCards.Count > 0) {
            int index = Random.Range(0, allCards.Count);
            cards.Add(allCards[index]);
            allCards.RemoveAt(index);
        }
    }

    private CardData RandomCard() {
        if(cards.Count == 0) {
            Create();
        }
        CardData card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    private Card CreateNewCard(Vector3 position, string animation) {
        GameObject newCard = GameObject.Instantiate(GameController.instance.cardPrefab, GameController.instance.canvas.transform);
        newCard.transform.position = position;
        Card card = newCard.GetComponent<Card>();
        if(card) {
            card.cardData = RandomCard();
            card.Initialize();
            Animator animator = newCard.GetComponentInChildren<Animator>();
            if(animator) {
                animator.CrossFade(animation, 0);
            }
        }
        return card;
    }

    internal void DealCard(Hand hand) {
        for (int i = 0; i < hand.cards.Length; i++) {
            if (hand.cards[i] == null) {
                hand.cards[i] = CreateNewCard(hand.postions[i].position, hand.animations[i]);
                if(hand.isPlayers) {
                    GameController.instance.player.cardAudio.Play();
                    return;
                }
                GameController.instance.enemy.cardAudio.Play();
                return;
            }
        }
    }

}
