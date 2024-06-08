using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static public GameController instance = null;
    public GameObject cardPrefab = null;
    public Canvas canvas = null;

    public Sprite[] health = new Sprite[10];
    public Sprite[] damage = new Sprite[10];
    public bool isPlayable = false;
    public bool isPlayersTurn = true;
    public int score = 0;
    public int kills = 0;

    public Deck playerDeck = new Deck();
    public Deck enemyDeck = new Deck();
    public List<CardData> cards = new List<CardData>();
    public Hand playerHand = new Hand();
    public Hand enemyHand = new Hand();
    public Player player = null;
    public Player enemy = null;
    public GameObject playerAttack = null;
    public GameObject enemyAttack = null;
    public Sprite fireball = null;
    public Sprite ice = null;
    public Sprite multiFireball = null;
    public Sprite multiIce = null;
    public Sprite firaballAndIce = null;
    public Sprite destroy = null;
    public TMP_Text scoreText = null;
    public TMP_Text turnText = null;
    public Image enemySkipImage = null; 
    public Sprite fireDemon = null;
    public Sprite iceDemon = null;
    public AudioSource monsterAudio = null;

    public void Awake() {
        instance = this;
        Spawn();
        playerDeck.Create();
        enemyDeck.Create();
        StartCoroutine(DealHands());
        UpdateHealth();
    }

    public void Quit() {
        SceneManager.LoadScene(0);
    }

    public void SkipTurn() {
        if(isPlayable && isPlayersTurn) NextPlayersTurn();
    }

    internal IEnumerator DealHands() {
        for (int i = 0; i < playerHand.cards.Length; i++) {
            playerDeck.DealCard(playerHand);
            enemyDeck.DealCard(enemyHand);
            yield return new WaitForSeconds(1);
        }
        isPlayable = true;
    }

    internal bool UseCard(Card card, Player target, Hand hand) {
        if(!isValid(card, target, hand)) {
            return false;
        }
        isPlayable = false;
        CastCard(card, target, hand);
        player.glowImage.gameObject.SetActive(false);
        enemy.glowImage.gameObject.SetActive(false);
        hand.RemoveCard(card);
        return false;
    }

    internal bool isValid(Card card, Player target, Hand hand) {
        if(card == null) return false;
        if(hand.isPlayers) {
            if(card.cardData.cost > player.mana) {
                return false;
            }
            if((target.isPlayer && card.cardData.isDefensive) || (!target.isPlayer && !card.cardData.isDefensive)) {
                return true;
            }
        } else {
            if (card.cardData.cost > enemy.mana) {
                return false;
            }
            if ((!target.isPlayer && card.cardData.isDefensive) || (target.isPlayer && !card.cardData.isDefensive)) {
                return true;
            }
        }
        return false;
    }

    internal void CastCard(Card card, Player target, Hand hand) {
        if (hand.isPlayers) {
            player.mana -= card.cardData.cost;
            player.UpdateMana();
            score += 10 * card.cardData.damage;
            UpdateScore();
        } else {
            enemy.mana -= card.cardData.cost;
            enemy.UpdateMana();
        }
        if (card.cardData.isMirror) {
            target.SetMirror(true);
            target.mirrorAudio.Play();
            isPlayable = true;
            NextPlayersTurn();
            return;
        }
        if(card.cardData.isDefensive) {
            target.health += card.cardData.damage;
            target.healAudio.Play();
            if(target.health > Player.MAX_HEALTH) {
                target.health = Player.MAX_HEALTH;
            }
            UpdateHealth();
            StartCoroutine(CastHeal(target));
            NextPlayersTurn();
            return;
        }
        CastAttack(card, target);
    }

    private IEnumerator CastHeal(Player target) {
        yield return new WaitForSeconds(0.5f);
        isPlayable = true;
    }

    internal void CastAttack(Card card, Player target) {
        GameObject attackObj = GameObject.Instantiate(target.isPlayer ? enemyAttack : playerAttack, canvas.gameObject.transform);
        Attack attack = attackObj.GetComponent<Attack>();
        if(attack != null) {
            attack.target = target;
            attack.source = card;
            switch(card.cardData.damageType) {
                case DamageType.Fire:
                    attack.attackImage.sprite = card.cardData.isMultiple ? multiFireball : fireball;
                    attack.fireAudio.Play();
                    break;
                case DamageType.Ice:
                    attack.attackImage.sprite = card.cardData.isMultiple ? multiIce : ice;
                    attack.iceAudio.Play();
                    break;
                case DamageType.Both:
                    attack.attackImage.sprite = firaballAndIce;
                    attack.iceAudio.Play();
                    attack.fireAudio.Play();
                    break;
                case DamageType.Destruct:
                    attack.attackImage.sprite = destroy;
                    attack.destroyAudio.Play();
                    break;
            }
        }
    }

    internal void UpdateHealth() {
        player.UpdateHealth();
        enemy.UpdateHealth();

        if(player.health <= 0) {
            player.healthImage.sprite = health[0];
            StartCoroutine(GameOver());
            return;
        }
        if(enemy.health <= 0) {
            score += 100;
            kills++;
            enemy.healthImage.sprite = health[0];
            StartCoroutine(NewEnemy());
        }
        UpdateScore();
    }

    internal void NextPlayersTurn() {
        isPlayersTurn = !isPlayersTurn;
        if(isPlayersTurn) {
            if(player.mana < Player.MAX_MANA) {
                player.mana++;
            }
            player.UpdateMana();
            turnText.text = "Merlin's turn";
            return;
        }

        if(enemy.health <= 0) {
            isPlayersTurn = true;
            if(player.mana < Player.MAX_MANA) {
                player.mana++;
            }
            player.UpdateMana();
            enemy.UpdateMana();
            return;
        }

        if(enemy.health > 0 && enemy.mana < Player.MAX_MANA) {
            enemy.mana++;
        }
        enemy.UpdateMana();
        turnText.text = "Demon's turn";
        DemonTurn();
    }

    private void DemonTurn() {
        Card card = AI.ChooseCard();
        StartCoroutine(AI.CastCard(card));
    }

    internal void TurnCard(Card card) {
        Animator animator = card.GetComponentInChildren<Animator>();
        if(animator) {
            animator.SetTrigger("Flip");
        }
    }

    private IEnumerator GameOver() {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(2);
    }

    private IEnumerator NewEnemy() {
        enemy.gameObject.SetActive(false);
        enemyHand.RemoveHand();
        yield return new WaitForSeconds(0.75f);

        Spawn();
        enemy.gameObject.SetActive(true);
        StartCoroutine(DealHands());
    }

    private void Spawn() {
        enemy.mana = 0;
        enemy.health = Player.MAX_HEALTH/2 + 1;
        enemy.UpdateHealth();
        enemy.UpdateMana();
        enemy.isFire = UnityEngine.Random.Range(0, 2) == 1;
        enemy.playerImage.sprite = enemy.isFire ? fireDemon : iceDemon;
    }

    public void UpdateScore() {
        scoreText.text = "Demons killed: " + kills + " | Score: " + score;
    }
}
