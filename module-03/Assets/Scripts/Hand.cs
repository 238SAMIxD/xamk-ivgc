using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Hand
{
    public Card[] cards = new Card[3];
    public Transform[] postions = new Transform[3];
    public string[] animations = new string[3];
    public bool isPlayers;

    public void RemoveCard(Card card) {
        for(int i = 0; i < cards.Length; i++) {
            if (cards[i] == card) {
                GameObject.Destroy(cards[i].gameObject);
                cards[i] = null;
                if(isPlayers) {
                    GameController.instance.playerDeck.DealCard(this);
                } else {
                    GameController.instance.enemyDeck.DealCard(this);
                }
                return;
            }
        }
    }

    internal void RemoveHand() {
        for(int i = 0; i < cards.Length; i++) {
            GameObject.Destroy(cards[i].gameObject);
            cards[i] = null;
        }
    }
}
