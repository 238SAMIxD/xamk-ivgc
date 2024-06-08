using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Burn : MonoBehaviour, IDropHandler
{
    public AudioSource burnAudio = null;

    public void OnDrop(PointerEventData eventData) {
        if (!GameController.instance.isPlayable) return;
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if(card != null && GameController.instance.isPlayersTurn) {
            burnAudio.Play();
            GameController.instance.playerHand.RemoveCard(card);
            GameController.instance.NextPlayersTurn();
        }
    }
}
