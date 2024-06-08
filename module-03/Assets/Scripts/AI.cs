using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    static public Card ChooseCard() {
        List<Card> validCards = new List<Card>();
        for(int i = 0; i < GameController.instance.enemyHand.cards.Length; i++) {
            if(GameController.instance.isValid(GameController.instance.enemyHand.cards[i], GameController.instance.enemy, GameController.instance.enemyHand) || GameController.instance.isValid(GameController.instance.enemyHand.cards[i], GameController.instance.player, GameController.instance.enemyHand)) {
                validCards.Add(GameController.instance.enemyHand.cards[i]);
            }
        }
        if(validCards.Count == 0) {
            GameController.instance.NextPlayersTurn();
            return null;
        }
        return validCards[UnityEngine.Random.Range(0, validCards.Count)];
    }

    static public IEnumerator CastCard(Card card) {
        yield return new WaitForSeconds(0.5f);
        if(card) {
            GameController.instance.TurnCard(card);
            yield return new WaitForSeconds(2);

            GameController.instance.UseCard(card, card.cardData.isDefensive ? GameController.instance.enemy : GameController.instance.player, GameController.instance.enemyHand);
            yield return new WaitForSeconds(1);

            GameController.instance.enemyDeck.DealCard(GameController.instance.enemyHand);
            yield return new WaitForSeconds(1);
        }
        else {
            GameController.instance.enemySkipImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);

            GameController.instance.enemySkipImage.gameObject.SetActive(false);
        }
    }
}
