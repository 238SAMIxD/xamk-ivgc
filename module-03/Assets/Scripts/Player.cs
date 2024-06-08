using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDropHandler
{
    internal static readonly int MAX_HEALTH = 9;
    internal static readonly int MAX_MANA = 5;

    public Image playerImage = null;
    public Image mirrorImage = null;
    public Image healthImage = null;
    public Image glowImage = null;

    public int health = MAX_HEALTH;
    public int mana = 1;
    public bool isPlayer;
    public bool isFire;

    public GameObject[] manaBalls = new GameObject[5];

    public AudioSource cardAudio = null;
    public AudioSource mirrorAudio = null;
    public AudioSource smashAudio = null;
    public AudioSource healAudio = null;
    public AudioSource dieAudio = null;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        UpdateHealth();
        UpdateMana();
    }

    internal void HitAnimation() {
        if(animator != null) {
            animator.SetTrigger("Hit");
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (!GameController.instance.isPlayable) return;
        Card card = eventData.pointerDrag.GetComponent<Card>();
        if(card != null) {
            GameController.instance.UseCard(card, this, GameController.instance.playerHand);
        }
    }

    internal void UpdateHealth() {
        if (health >= 0 && health < GameController.instance.health.Length) {
            healthImage.sprite = GameController.instance.health[health];
        }
    }

    internal void SetMirror(bool on) {
        mirrorImage.gameObject.SetActive(on);
    }

    internal bool HasMirror() {
        return mirrorImage.gameObject.activeInHierarchy;
    }

    internal void UpdateMana() {
        for(int i = 0; i < manaBalls.Length; i++) {
            manaBalls[i].SetActive(mana > i);
        }
    }
}
