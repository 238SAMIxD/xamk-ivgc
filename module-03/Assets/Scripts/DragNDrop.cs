using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    public void OnBeginDrag(PointerEventData eventData) {
        originalPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!GameController.instance.isPlayable) return;
        transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        foreach(GameObject hover in eventData.hovered) {
            if(hover.GetComponent<Player>() != null && GameController.instance.isValid(GetComponent<Card>(), hover.GetComponent<Player>(), GameController.instance.playerHand)) {
                hover.GetComponent<Player>().glowImage.gameObject.SetActive(true);
                return;
            }
            if(hover.GetComponent<Burn>() != null) {
                GetComponent<Card>().burnImage.gameObject.SetActive(true);
                return;
            }
            GetComponent<Card>().burnImage.gameObject.SetActive(false);
        }
        GameController.instance.player.glowImage.gameObject.SetActive(false);
        GameController.instance.enemy.glowImage.gameObject.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.position = originalPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
